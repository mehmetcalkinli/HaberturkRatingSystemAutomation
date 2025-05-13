import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { environment } from './appConfig';

@Injectable({
  providedIn: 'root'
})
export class ProgsettService {

  constructor(private http: HttpClient,private cookieService: CookieService) { }



  setConfig(MailAdresi:string,MailŞifresi:string,KantarMediaMail:string,IngestMail:string,GonderilenMail:string): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.cookieService.get('token')}`,
      'Access-Control-Allow-Origin': '*'
    })

    let params = new HttpParams();
    params = params.append('MailAdresi', MailAdresi);
    params = params.append('MailŞifresi', MailŞifresi);
    params = params.append('KantarMediaMail', KantarMediaMail);
    params = params.append('IngestMail', IngestMail);
    params = params.append('GonderilenMail', GonderilenMail);


    return this.http.get<any>(`${environment.apiBaseUrl}/SetConfig`, { params: params, headers: headers });

  }

  getConfig(): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.cookieService.get('token')}`,
      'Access-Control-Allow-Origin': '*'
    })

   
    return this.http.get<any>(`${environment.apiBaseUrl}/GetConfig`, { headers: headers });

  }
}
