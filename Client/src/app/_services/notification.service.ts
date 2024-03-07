import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private socket!: WebSocket;

  private apiUrl = 'https://localhost:7290/api/';
  constructor(private http: HttpClient) { }

  connect(url: string): Observable<any> {
    const token = localStorage.getItem('token');
    const urlWithToken = `${url}?token=${token}`;

    return new Observable((observer) => {
      this.socket = new WebSocket(urlWithToken);

      this.socket.onopen = () => {
        console.log('WebSocket connection established.');
      };

      this.socket.onmessage = (event) => {
        const message = JSON.parse(event.data);
        console.log('Received message:', message); // Print received message to console
        observer.next(message);
      };

      this.socket.onerror = (error) => {
        console.error('WebSocket error:', error); // Print error to console
        observer.error(error);
      };

      this.socket.onclose = () => {
        console.log('WebSocket connection closed.');
        observer.complete();
      };

      return () => {
        this.socket.close();
      };
    });
  }
  sendNotificationToWS(message: any) {
    if (this.socket.readyState === WebSocket.OPEN) {
      this.socket.send(JSON.stringify(message));
    } else {
      console.error('WebSocket connection not established.');
    }
  }
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
