import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Campaign } from 'src/models/campaign';
import { retry, catchError } from 'rxjs/operators';
import { baseEnvironment } from 'src/environments/base-environment';

@Injectable({
  providedIn: 'root'
})
export class CampaignService {

  constructor(
    private _httpClient: HttpClient
  ) { }

  getAll(pageIndex = 0, pageSize = 10): Observable<Campaign[]> {
    return this._httpClient.get<Array<Campaign>>(baseEnvironment.campaignURL +
      '?pageIndex=' + pageIndex + '&' + 'pageSize=' + pageSize)
      .pipe(
        retry(baseEnvironment.retryLimit),
        catchError((error) => {
          // TODO: Error Logging
          console.error(error);
          throw new Error('Returning campaign list encountered a fatal error');
        })
      );
  }

  countAll(): Observable<number> {
    return this._httpClient.get<number>(baseEnvironment.campaignURL + 'count')
      .pipe(
        retry(baseEnvironment.retryLimit),
        catchError((error) => {
          // TODO: Error Logging
          console.error(error);
          throw new Error('Counting campaigns encountered a fatal error');
        })
      );
  }

  get(id: number): Observable<Campaign> {
    return this._httpClient.get<Campaign>(baseEnvironment.campaignURL + id)
      .pipe(
        retry(baseEnvironment.retryLimit),
        catchError((error) => {
          // TODO: Error Logging
          console.error(error);
          throw new Error('Returning the campaign encountered a fatal error');
        })
      );
  }

  insert(campaign: Campaign): Observable<boolean> {
    return this._httpClient.post<boolean>(baseEnvironment.campaignURL, campaign)
      .pipe(
        retry(baseEnvironment.retryLimit),
        catchError((error) => {
          // TODO: Error Logging
          console.error(error);
          throw new Error('Inserting the campaign encountered a fatal error');
        })
      );
  }

  update(id: number, campaign: Campaign): Observable<boolean> {
    return this._httpClient.put<boolean>(baseEnvironment.campaignURL + id, campaign)
      .pipe(
        retry(baseEnvironment.retryLimit),
        catchError((error) => {
          // TODO: Error Logging
          console.error(error);
          throw new Error('Updating the campaign encountered a fatal error');
        })
      );
  }

  delete(id: number): Observable<boolean> {
    return this._httpClient.delete<boolean>(baseEnvironment.campaignURL + id)
      .pipe(
        retry(baseEnvironment.retryLimit),
        catchError((error) => {
          // TODO: Error Logging
          console.error(error);
          throw new Error('Deleting the campaign encountered a fatal error');
        })
      );
  }

}
