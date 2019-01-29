import { Product } from './../../../models/product';
import { ProductService } from './../../../services/product.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { DialogHelperService } from 'src/services/dialog-helper.service';
import { Campaign } from 'src/models/campaign';
import { CampaignService } from 'src/services/campaign.service';
import { debounceTime, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-add-update-campaign',
  templateUrl: './add-update-campaign.component.html',
  styleUrls: ['./add-update-campaign.component.scss']
})
export class AddUpdateCampaignComponent implements OnInit {

  nameFC: FormControl;
  startDateFC: FormControl;
  endDateFC: FormControl;
  productFC: FormControl;
  isActiveFC: FormControl;
  selectedCampaign: Campaign;
  maxLengthValue = 50;
  searchedProducts: Product[];
  selectedProduct: Product;

  constructor(
    private _campaignService: CampaignService,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private _dialogHelperService: DialogHelperService,
    private _productService: ProductService
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

    this.endDateFC = new FormControl(
      { value: '', disabled: true },
      [
        Validators.required
      ]);

    this.productFC = new FormControl('',
      [
        Validators.required
      ]);

    this.isActiveFC = new FormControl(false);

    this.productFC.valueChanges.pipe(
      debounceTime(250)
    ).subscribe(searchKeyword => {
      this._productService.search(searchKeyword).subscribe(products => {
        this.searchedProducts = products;
      });
    });

    // Getting the campaign using query params (if any)
    this._activatedRoute.queryParams.subscribe(queryParams => {
      if (queryParams['campaignid']) {
        this._campaignService.get(queryParams['campaignid']).subscribe(campaign => {
          this.selectedCampaign = campaign;
          this.nameFC.setValue(this.selectedCampaign.name);
          this.startDateFC.setValue(this.selectedCampaign.start_date);
          this.endDateFC.setValue(this.selectedCampaign.end_date);
          this.isActiveFC.setValue(this.selectedCampaign.is_active);
          this._productService.get(this.selectedCampaign.product.id).subscribe(
            product => {
              this.searchedProducts = new Array<Product>();
              this.searchedProducts.push(product);
              this.productFC.setValue(this.selectedCampaign.product);
              this.selectedProduct = product;
            }
          );
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
              this.selectedCampaign.start_date = this.startDateFC.value;
              this.selectedCampaign.end_date = this.endDateFC.value;
              this.selectedCampaign.product_id = (this.productFC.value as Product).id;
              this.selectedCampaign.is_active = this.isActiveFC.value;
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
      campaign.product_id = (this.productFC.value as Product).id;
      campaign.is_active = this.isActiveFC.value,
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

  displayProductName(product: Product): any {
    return product.name;
  }

  onSearchKeywordChange() {
    this.selectedProduct = null;
  }

  onProductSelect(product: Product) {
    this.selectedProduct = product;
  }

}
