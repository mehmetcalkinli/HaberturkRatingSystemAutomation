import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { environment } from './appConfig';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  
  private apiUrl = 'https://localhost:7238/RatingSystem/'; // Replace this with your actual API URL

  constructor(private http: HttpClient,private cookieService: CookieService) { }



  addUser(username:string,password:string): Observable<any> {
    // const userData = {
    //   username: this.username,
    //   password: this.password,
    //   newPassword: this.newPassword

    // };
    // 'Content-Type': 'application/json',

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.cookieService.get('token')}`,
      'Access-Control-Allow-Origin': '*'
    })

    let params = new HttpParams();
    params = params.append('username', username);
    params = params.append('password', password);


    return this.http.get<any>(`${environment.apiBaseUrl}/AddUser`, { params: params, headers: headers });

    // this.http.post<any>('https://localhost:7238/RatingSystem/AddUser',{ params: params, headers: headers })
    // .subscribe(
    //   response => {
    //     console.log('User added successfully:', response);
    //     // Handle success response
    //   },
    //   error => {
    //     console.error('Error adding user:', error);
    //     // Handle error response
    //   }
    // );
  }

  changePassword(username:string,newPassword:string): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.cookieService.get('token')}`,
      'Access-Control-Allow-Origin': '*'
    })

    let params = new HttpParams();
    params = params.append('username', username);
    params = params.append('newPassword', newPassword);


    return this.http.get<any>(`${environment.apiBaseUrl}/ChangePassword`, { params: params, headers: headers });

  }


  deleteUser(username:string): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.cookieService.get('token')}`,
      'Access-Control-Allow-Origin': '*'
    })

    let params = new HttpParams();
    params = params.append('username', username);
   

    return this.http.get<any>(`${environment.apiBaseUrl}/DeleteUser`, { params: params, headers: headers });

  }
}
