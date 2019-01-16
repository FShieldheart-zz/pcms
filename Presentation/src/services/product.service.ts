import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Product } from 'src/models/product';
import { retry } from 'rxjs/operators';
import { baseEnvironment } from 'src/environments/base-environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private _httpClient: HttpClient) { }

  getAll(pageIndex = 0, pageSize = 10): Observable<Product[]> {
    return this._httpClient.get<Array<Product>>(baseEnvironment.productURL +
      '?pageIndex=' + pageIndex + '&' + 'pageSize=' + pageSize)
      .pipe(
        retry(baseEnvironment.retryLimit)
      );
  }

  countAll(): Observable<number> {
    return this._httpClient.get<number>(baseEnvironment.productURL + 'count')
      .pipe(
        retry(baseEnvironment.retryLimit)
      );
  }

  get(id: number): Observable<Product> {
    return this._httpClient.get<Product>(baseEnvironment.productURL + id)
      .pipe(
        retry(baseEnvironment.retryLimit)
      );
  }

  insert(product: Product): Observable<boolean> {
    return this._httpClient.post<boolean>(baseEnvironment.productURL, product)
      .pipe(
        retry(baseEnvironment.retryLimit)
      );
  }

  update(id: number, product: Product): Observable<boolean> {
    return this._httpClient.put<boolean>(baseEnvironment.productURL + id, product)
      .pipe(
        retry(baseEnvironment.retryLimit)
      );
  }

  delete(id: number): Observable<boolean> {
    return this._httpClient.delete<boolean>(baseEnvironment.productURL + id)
      .pipe(
        retry(baseEnvironment.retryLimit)
      );
  }

}
