import { Injectable } from '@angular/core';
import { HttpClient, HttpParams ,HttpClientModule } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { environment } from './appConfig';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient,private cookieService: CookieService) { }



   static isLoggedIn = false;
   static jwtToken = "";

  // isLoggedIn = false;

  login(username: string, password: string): Observable<any> {

    let params = new HttpParams();
    params = params.append('username', username);
    params = params.append('password', password);

    return this.http.get<string>(`${environment.apiBaseUrl}/Login`, { params: params });

  }

  logout(): void {
    this.clearToken();

    // AuthService.isLoggedIn = false;
  }

   getIsLoggedIn()
  {
    return AuthService.isLoggedIn;
  }
  private clearToken(): void {
    document.cookie = 'token=null';
  }
}
