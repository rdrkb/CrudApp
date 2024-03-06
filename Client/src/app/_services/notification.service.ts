import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private apiUrl = 'https://localhost:7290/api/';
  constructor(private http: HttpClient) { }

  getNumberOfNotification(): Observable<number>
  {
    const url = `${this.apiUrl}Notification/totalNumberOfNotification`;

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<number>(url, { headers }).pipe(
      map(totalList => {
        return totalList;
      })
    );
  }

  GetNotifications(pageNumber: number, itemPerPage: number): Observable<any>
  {
    const url = `${this.apiUrl}Notification/GetNotifications?pageNumber=${pageNumber}&pageSize=${itemPerPage}`;
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<any>(url, {headers});
  }
}
