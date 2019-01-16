import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListProductComponent } from './list-product/list-product.component';
import { HomeComponent } from './home/home.component';
import { MainRoutingModule } from './main-routing.module';
import { ShareModule } from '../share/share.module';
import { AddUpdateProductComponent } from './add-update-product/add-update-product.component';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';

@NgModule({
  declarations: [
    HomeComponent,
    ListProductComponent,
    AddUpdateProductComponent,
    ConfirmationDialogComponent
  ],
  imports: [
    CommonModule,
    MainRoutingModule,
    ShareModule
  ]
})
export class MainModule { }
