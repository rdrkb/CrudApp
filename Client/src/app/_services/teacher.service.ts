import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import { Observable } from "rxjs";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class TeacherService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  totalTeachers(pageNumber: number, pageSize: number, university: string, department: string):Observable<number>{
    const url = `${this.apiUrl}Teacher/totalteachers?pageNumber=${pageNumber}&pageSize=${pageSize}&university=${university}&department=${department}`;

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.get<number>(url, { headers });
  }
  getTeachers(pageNumber: number, pageSize: number, university: string, department: string): Observable<any> {
    const url = `${this.apiUrl}Teacher/getteachers?pageNumber=${pageNumber}&pageSize=${pageSize}&university=${university}&department=${department}`;
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return  this.http.get<any>(url,{ headers });
  }

  getTeacher(username: string): Observable<any>{
    const url = `${this.apiUrl}Teacher/getteacher/${username}`;

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<any>(url,{headers});
  }
  addTeacher(teacherData: any): Observable<any> {
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.post<any>(`${this.apiUrl}Teacher/create`, teacherData, { headers });
  }
  updateTeacher(username: string, teacherData: any): Observable<any> {
    const url = `${this.apiUrl}Teacher/update/${username}`;

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put<any>(url, teacherData, { headers });
  }
}
