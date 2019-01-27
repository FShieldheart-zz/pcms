import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { DialogHelperService } from 'src/services/dialog-helper.service';
import { Campaign } from 'src/models/campaign';
import { CampaignService } from 'src/services/campaign.service';

@Component({
  selector: 'app-add-update-campaign',
  templateUrl: './add-update-campaign.component.html',
  styleUrls: ['./add-update-campaign.component.scss']
})
export class AddUpdateCampaignComponent implements OnInit {

  nameFC: FormControl;
  startDateFC: FormControl;
  endDateFC: FormControl;
  selectedCampaign: Campaign;
  maxLengthValue = 50;

  constructor(
    private _campaignService: CampaignService,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private _dialogHelperService: DialogHelperService
  ) { }

  // <--- Helpers --->
  private ShowMessageDialog(width: number, message: string) {
    this._dialogHelperService.showMessageDialog(width, message);
  }

  private ShowConfirmationDialog(width: number, message: string) {
    return this._dialogHelperService.showConfirmationDialog(width, message);
  }
  // <--- Helpers --->

  ngOnInit() {
    this.nameFC = new FormControl('', [
      Validators.required,
      Validators.maxLength(this.maxLengthValue)
    ]);

    this.startDateFC = new FormControl(
      { value: '', disabled: true },
      [
        Validators.required
      ]);

    this.endDateFC = new FormControl({ value: '', disabled: true },
      [
        Validators.required
      ]);

    // Getting the campaign using query params (if any)
    this._activatedRoute.queryParams.subscribe(queryParams => {
      if (queryParams['campaignid']) {
        this._campaignService.get(queryParams['campaignid']).subscribe(campaign => {
          this.selectedCampaign = campaign;
          this.nameFC.setValue(this.selectedCampaign.name);
        });
      }
    });

  }

  saveUpdateCampaign() {

    // Campaign Update
    if (this.selectedCampaign) {
      this.ShowConfirmationDialog(250, 'Are you sure wanting to update?')
        .afterClosed().subscribe(
          result => {
            if (result) {
              this.selectedCampaign.name = this.nameFC.value;
              this._campaignService.update(this.selectedCampaign.id, this.selectedCampaign).subscribe(updateResult => {
                if (updateResult) {
                  this._router.navigate(['/campaign']);
                } else {
                  this.ShowMessageDialog(250, 'Updating the campaign has been failed.');
                }
              });
            }
          },
          error => {
            this.ShowMessageDialog(250, error.message);
          }
        );

      // Campaign Insert
    } else {
      const campaign = new Campaign();
      campaign.name = this.nameFC.value;
      campaign.start_date = this.startDateFC.value;
      campaign.end_date = this.endDateFC.value;
      campaign.product_id = 0;
      this._campaignService.insert(campaign).subscribe(
        insertResult => {
          if (insertResult) {
            this._router.navigate(['/campaign']);
          } else {
            this.ShowMessageDialog(250, 'Inserting new campaign has been failed.');
          }
        },
        error => {
          this._dialogHelperService.showMessageDialog(250, error.message);
        }
      );
    }
  }

}
