import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Product } from 'src/models/product';
import { retry, catchError } from 'rxjs/operators';
import { baseEnvironment } from 'src/environments/base-environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(
    private _httpClient: HttpClient
  ) { }

  getAll(pageIndex = 0, pageSize = 10): Observable<Product[]> {
    return this._httpClient.get<Array<Product>>(baseEnvironment.productURL +
      '?pageIndex=' + pageIndex + '&' + 'pageSize=' + pageSize)
      .pipe(
        retry(baseEnvironment.retryLimit),
        catchError((error) => {
          // TODO: Error Logging
          console.error(error);
          throw new Error('Returning product list encountered a fatal error');
        })
      );
  }

  search(keyword: string): Observable<Product[]> {
    return this._httpClient.get<Product[]>(baseEnvironment.productURL + 'search' + '?searchKeyword=' + keyword)
    .pipe(
      retry(baseEnvironment.retryLimit),
      catchError((error) => {
        // TODO: Error Logging
        console.error(error);
        throw new Error('Searching products encountered a fatal error');
      })
    );
  }

  countAll(): Observable<number> {
    return this._httpClient.get<number>(baseEnvironment.productURL + 'count')
      .pipe(
        retry(baseEnvironment.retryLimit),
        catchError((error) => {
          // TODO: Error Logging
          console.error(error);
          throw new Error('Counting products encountered a fatal error');
        })
      );
  }

  get(id: number): Observable<Product> {
    return this._httpClient.get<Product>(baseEnvironment.productURL + id)
      .pipe(
        retry(baseEnvironment.retryLimit),
        catchError((error) => {
          // TODO: Error Logging
          console.error(error);
          throw new Error('Returning the product encountered a fatal error');
        })
      );
  }

  insert(product: Product): Observable<boolean> {
    return this._httpClient.post<boolean>(baseEnvironment.productURL, product)
      .pipe(
        retry(baseEnvironment.retryLimit),
        catchError((error) => {
          // TODO: Error Logging
          console.error(error);
          throw new Error('Inserting the product encountered a fatal error');
        })
      );
  }

  update(id: number, product: Product): Observable<boolean> {
    return this._httpClient.put<boolean>(baseEnvironment.productURL + id, product)
      .pipe(
        retry(baseEnvironment.retryLimit),
        catchError((error) => {
          // TODO: Error Logging
          console.error(error);
          throw new Error('Updating the product encountered a fatal error');
        })
      );
  }

  delete(id: number): Observable<boolean> {
    return this._httpClient.delete<boolean>(baseEnvironment.productURL + id)
      .pipe(
        retry(baseEnvironment.retryLimit),
        catchError((error) => {
          // TODO: Error Logging
          console.error(error);
          throw new Error('Deleting the product encountered a fatal error');
        })
      );
  }

}
