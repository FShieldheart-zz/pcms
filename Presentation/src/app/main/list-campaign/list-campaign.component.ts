import { Campaign } from 'src/models/campaign';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatTableDataSource, MatSort, PageEvent, MatCheckboxChange } from '@angular/material';
import { CampaignService } from 'src/services/campaign.service';
import { DialogHelperService } from 'src/services/dialog-helper.service';
import { EventEmitter } from 'events';

@Component({
  selector: 'app-list-campaign',
  templateUrl: './list-campaign.component.html',
  styleUrls: ['./list-campaign.component.scss']
})
export class ListCampaignComponent implements OnInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = ['id', 'name', 'product.name', 'start_date', 'end_date', 'is_active', 'actions'];
  dataSource: MatTableDataSource<any>;

  campaignLength: number;
  length: number;
  campaigns: Campaign[];

  constructor(
    private _campaignService: CampaignService,
    private _dialogHelperService: DialogHelperService
  ) { }

  // <--- Helpers --->
  private ShowMessageDialog(width: number, message: string) {
    this._dialogHelperService.showMessageDialog(width, message);
  }

  private ShowConfirmationDialog(width: number, message: string) {
    return this._dialogHelperService.showConfirmationDialog(width, message);
  }

  public getServerData(event: PageEvent) {
    // this._campaignService.getAll(event.pageIndex, event.pageSize).subscribe(campaigns => {
    //   const existingCampaigns = this.dataSource.data as Campaign[];
    //   campaigns.forEach(element => {
    //     if (!existingCampaigns.find(p => p.id === element.id)) {
    //       existingCampaigns.push(element);
    //     }
    //   });
    //   this.dataSource.data = existingCampaigns;
    //   this.length = this.campaignLength;
    // });
    return event;
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  // <--- Helpers --->

  ngOnInit() {

    // Counting the campaigns for server side paging
    this._campaignService.countAll().subscribe(
      campaignCount => {
        this.campaignLength = campaignCount;
        this.length = this.campaignLength;
      },
      error => {
        this.ShowMessageDialog(250, error.message);
      },
      () => {
        // Retrieving first page of the data following the counting
        this._campaignService.getAll().subscribe(
          campaigns => {
            this.campaigns = campaigns;
            this.dataSource = new MatTableDataSource<Campaign>(campaigns);
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
          },
          error => {
            this.ShowMessageDialog(250, error.message);
          }
        );
      }
    );

  }

  // Campaign Deletion
  deleteCampaign(id: number) {
    this.ShowConfirmationDialog(250, 'Are you sure wanting to delete?')
      .afterClosed().subscribe(
        result => {
          if (result) {
            this._campaignService.delete(id).subscribe(deleteResult => {
              if (deleteResult) {
                let campaigns = (this.dataSource.data as Campaign[]);
                campaigns = campaigns.filter(p => p.id !== id);
                this.dataSource.data = campaigns;
              } else {
                this.ShowMessageDialog(250, 'Deleting the campaign has been failed.');
              }
            });
          }
        },
        error => {
          this._dialogHelperService.showMessageDialog(250, error.message);
        }
      );
  }

  isActiveChange(event: MatCheckboxChange) {
    if (event.checked) {
      this.dataSource = new MatTableDataSource<Campaign>(this.campaigns.filter(c => c.is_active === event.checked));
    } else {
      this.dataSource = new MatTableDataSource<Campaign>(this.campaigns);
    }
  }

}
