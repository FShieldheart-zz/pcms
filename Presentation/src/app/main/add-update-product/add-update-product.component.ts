import { Product } from './../../../models/product';
import { ProductService } from './../../../services/product.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogRef } from '@angular/material';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-add-update-product',
  templateUrl: './add-update-product.component.html',
  styleUrls: ['./add-update-product.component.scss']
})
export class AddUpdateProductComponent implements OnInit {

  nameFC: FormControl;
  selectedProduct: Product;

  constructor(
    private _productService: ProductService,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private _matDialog: MatDialog
  ) { }

  ngOnInit() {
    this.nameFC = new FormControl('', [
      Validators.required,
      Validators.maxLength(50)
    ]);

    this._activatedRoute.queryParams.subscribe(queryParams => {
      if (queryParams['productid']) {
        this._productService.get(queryParams['productid']).subscribe(product => {
          this.selectedProduct = product;
          this.nameFC.setValue(this.selectedProduct.name);
        });
      }
    });

  }

  saveUpdateProduct() {
    if (this.selectedProduct) {

      const dialogRef = this._matDialog.open(ConfirmationDialogComponent, {
        width: '250px',
        data: 'Are you sure wanting to update?'
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.selectedProduct.name = this.nameFC.value;
          this._productService.update(this.selectedProduct.id, this.selectedProduct).subscribe(updateResult => {
            this._router.navigate(['/product']);
          });
        }
      });
    } else {
      const product = new Product();
      product.name = this.nameFC.value;
      this._productService.insert(product).subscribe(insertResult => {
        this._router.navigate(['/product']);
      });
    }
  }

}
