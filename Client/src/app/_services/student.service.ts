import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import { Observable } from "rxjs";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  totalStudents(pageNumber: number, pageSize: number, university: string, department: string):Observable<number>{
    const url = `${this.apiUrl}Student/totalstudents?pageNumber=${pageNumber}&pageSize=${pageSize}&university=${university}&department=${department}`;
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.get<number>(url, { headers });
  }
  getStudents(pageNumber: number, pageSize: number, university: string, department: string): Observable<any> {
    const url = `${this.apiUrl}Student/getstudents?pageNumber=${pageNumber}&pageSize=${pageSize}&university=${university}&department=${department}`;
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return  this.http.get<any>(url, { headers });
  }

  getStudent(username: string): Observable<any>{
    const url = `${this.apiUrl}Student/getstudent/${username}`;

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<any>(url,{headers});
  }
  addStudent(studentData: any): Observable<any> {

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.post<any>(`${this.apiUrl}Student/create`, studentData, { headers });
  }
  updateStudent(username: string, studentData: any): Observable<any> {
    const url = `${this.apiUrl}Student/update/${username}`;

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put<any>(url, studentData, { headers });
  }
}
