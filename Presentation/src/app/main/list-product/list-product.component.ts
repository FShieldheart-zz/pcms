import { Product } from 'src/models/product';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatTableDataSource, MatSort, MatDialog, PageEvent } from '@angular/material';
import { ProductService } from 'src/services/product.service';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-list-product',
  templateUrl: './list-product.component.html',
  styleUrls: ['./list-product.component.scss']
})
export class ListProductComponent implements OnInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = ['id', 'name', 'actions'];
  dataSource: MatTableDataSource<any>;

  productLength: number;
  length: number;

  constructor(
    private _productService: ProductService,
    private _matDialog: MatDialog
  ) { }

  ngOnInit() {
    this._productService.getAll(0, 2).subscribe(products => {
      this.dataSource = new MatTableDataSource<Product>(products);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this._productService.countAll().subscribe(productCount => {
        this.productLength = productCount;
        this.length = this.productLength;
      });
    });
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  deleteProduct(id: number) {
    const dialogRef = this._matDialog.open(ConfirmationDialogComponent, {
      width: '250px',
      data: 'Are you sure wanting to delete?'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this._productService.delete(id).subscribe(deleteResult => {
          let products = (this.dataSource.data as Product[]);
          products = products.filter(p => p.id !== id);
          this.dataSource.data = products;
        });
      }
    });
  }

  public getServerData(event: PageEvent) {
    this._productService.getAll(event.pageIndex, event.pageSize).subscribe(products => {
      const existingProducts = this.dataSource.data as Product[];
      products.forEach(element => {
        if (!existingProducts.find(p => p.id === element.id)) {
          existingProducts.push(element);
        }
      });
      this.dataSource.data = existingProducts;
      this.length = this.productLength;
    });
    return event;
  }

}
