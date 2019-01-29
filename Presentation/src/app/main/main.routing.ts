import { ListCampaignComponent } from './list-campaign/list-campaign.component';
import { Routes } from '@angular/router';
import { ListProductComponent } from './list-product/list-product.component';
import { HomeComponent } from './home/home.component';
import { AddUpdateProductComponent } from './add-update-product/add-update-product.component';
import { AddUpdateCampaignComponent } from './add-update-campaign/add-update-campaign.component';

export const MainRoutes: Routes =
    [
        {
            path: '',
            component: HomeComponent
        },
        {
            path: 'product',
            component: ListProductComponent
        },
        {
            path: 'campaign',
            component: ListCampaignComponent
        },
        {
            path: 'add-product',
            component: AddUpdateProductComponent
        },
        {
            path: 'update-product',
            component: AddUpdateProductComponent
        },
        {
            path: 'add-campaign',
            component: AddUpdateCampaignComponent
        },
        {
            path: 'update-campaign',
            component: AddUpdateCampaignComponent
        }
    ];
