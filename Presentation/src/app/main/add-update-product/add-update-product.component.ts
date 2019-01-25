import { DialogHelperService } from 'src/services/dialog-helper.service';
import { Product } from './../../../models/product';
import { ProductService } from './../../../services/product.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-add-update-product',
  templateUrl: './add-update-product.component.html',
  styleUrls: ['./add-update-product.component.scss']
})
export class AddUpdateProductComponent implements OnInit {

  nameFC: FormControl;
  selectedProduct: Product;
  maxLengthValue = 50;

  constructor(
    private _productService: ProductService,
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

    // Getting the product using query params (if any)
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

    // Product Update
    if (this.selectedProduct) {
      this.ShowConfirmationDialog(250, 'Are you sure wanting to update?')
        .afterClosed().subscribe(
          result => {
            if (result) {
              this.selectedProduct.name = this.nameFC.value;
              this._productService.update(this.selectedProduct.id, this.selectedProduct).subscribe(updateResult => {
                if (updateResult) {
                  this._router.navigate(['/product']);
                } else {
                  this.ShowMessageDialog(250, 'Updating the product has been failed.');
                }
              });
            }
          },
          error => {
            this.ShowMessageDialog(250, error.message);
          }
        );

      // Product Insert
    } else {
      const product = new Product();
      product.name = this.nameFC.value;
      this._productService.insert(product).subscribe(
        insertResult => {
          if (insertResult) {
            this._router.navigate(['/product']);
          } else {
            this.ShowMessageDialog(250, 'Inserting new product has been failed.');
          }
        },
        error => {
          this._dialogHelperService.showMessageDialog(250, error.message);
        }
      );
    }
  }

}
