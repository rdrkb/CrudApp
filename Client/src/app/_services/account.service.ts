import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Token} from "../_models/token";
import {jwtDecode} from "jwt-decode";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  register(user: any): Observable<Token> {
    return this.http.post<Token>(`${this.apiUrl}Account/register`, user);
  }

  login(user: any): Observable<Token> {
    return this.http.post<Token>(`${this.apiUrl}Account/login`, user);
  }

  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }
  logout(): void {
    localStorage.removeItem('token');
  }
  getToken(): string | null {
    return localStorage.getItem('token');
  }
  getCurrentUser(): any {
    const token = localStorage.getItem('token');
    if(token != null) {
      return jwtDecode(token);
    }
    return  null;
  }
  isAuthenticated(): boolean {
    const token = this.getToken();
    return !!token;
  }
}
