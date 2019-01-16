import { Routes } from '@angular/router';
import { ListProductComponent } from './list-product/list-product.component';
import { HomeComponent } from './home/home.component';
import { AddUpdateProductComponent } from './add-update-product/add-update-product.component';

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
            path: 'add-product',
            component: AddUpdateProductComponent
        },
        {
            path: 'update-product',
            component: AddUpdateProductComponent
        }
    ];
