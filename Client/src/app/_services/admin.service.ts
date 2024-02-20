import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  totalAdmins(pageNumber: number, pageSize: number):Observable<number>{
    const url = `${this.apiUrl}Admin/totaladmins?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.get<number>(url, { headers });
  }
  getAdmins(pageNumber: number, pageSize: number): Observable<any> {
    const url = `${this.apiUrl}Admin/getadmins?pageNumber=${pageNumber}&pageSize=${pageSize}`;

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.get<any>(url, { headers });
  }
  getAdmin(username: string): Observable<any>{
    const url = `${this.apiUrl}Admin/getadmin/${username}`;

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<any>(url,{headers});
  }

  addAdmin(adminData: any): Observable<any> {
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.post<any>(`${this.apiUrl}Admin/create`, adminData, { headers });
  }
  updateAdmin(username: string, adminData: any): Observable<any> {
    const url = `${this.apiUrl}Admin/update/${username}`;

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put<any>(url, adminData, { headers });
  }
}
