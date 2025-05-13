import { HttpClient, HttpParams ,HttpClientModule, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ReceivedList } from './receivedlist.interface';
import { AuthService } from './auth.service';
import { CookieService } from 'ngx-cookie-service';
import { environment } from './appConfig';

@Injectable({
  providedIn: 'root'
})

export class DateSelectionService {
  baseUrl=`https://jsonplaceholder.typicode.com`;
  apiUrl=`https://localhost:7238/RatingSystem/GetExcelData`;
  apiUrl2=`https://localhost:7238/RatingSystem/GetValidData`;
  apiUrl3=`https://localhost:7238/RatingSystem/DownloadFiles`;

  apiBaseUrl:string="";

  private selectedItemSubject: BehaviorSubject<string> = new BehaviorSubject<string>('');

  private selectedMonth: BehaviorSubject<string> = new BehaviorSubject<string>('');
  private selectedYear: BehaviorSubject<string> = new BehaviorSubject<string>('');

  private username: BehaviorSubject<string> = new BehaviorSubject<string>('');

  private isToken: BehaviorSubject<string> = new BehaviorSubject<string>('');

  // private selectedMonth: BehaviorSubject<string> = new BehaviorSubject<string>('');
  // private selectedYear: BehaviorSubject<string> = new BehaviorSubject<string>('');


  constructor(private http: HttpClient,private cookieService: CookieService) { }

  // ngOnInit(): void {
  //   this.apiBaseUrl = this.appConfigService.apiBaseUrl;
  // }


  setUsername(item: string) {
    this.username.next(item);
  }

  getUsername(): Observable<string> {
    return this.username.asObservable();
  }

  setToken(item: string) {
    this.isToken.next(item);
  }

  getToken(): Observable<string> {
    return this.isToken.asObservable();
  }

  setSelectedItem(item: string) {
    this.selectedItemSubject.next(item);
  }

  getSelectedItem(): Observable<string> {
    return this.selectedItemSubject.asObservable();
  }

  


  setSelectedMonth(item: string) {
    this.selectedMonth.next(item);
  }

  getSelectedMonth(): Observable<string> {
    return this.selectedMonth.asObservable();
  }

  
  setSelectedYear(item: string) {
    this.selectedYear.next(item);
  }

  getSelectedYear(): Observable<string> {
    return this.selectedYear.asObservable();
  }

  // Method to make API request using the selected item
  fetchDataBasedOnSelectedItem(selectedItem: string): Observable<any> {
    let params = new HttpParams();
    params = params.append('inputDate', selectedItem);


    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.cookieService.get('token')}`,
      'Access-Control-Allow-Origin': '*'
    })

    return this.http.get<ReceivedList[]>(`${environment.apiBaseUrl}/GetExcelData`, { params: params, headers: headers});

    // return this.http.get<ReceivedList[]>(`${this.apiUrl}`, { params: params }).pipe(tap(res=>{
    //   debugger
    // }));


    // return this.http.get<ReceivedList[]>(`${this.apiUrl}/${selectedItem}`);

  }
  getValidList(monthInput: string, yearInput:string): Observable<any> {
    let params = new HttpParams();
    params = params.append('monthInput', monthInput);
    params = params.append('yearInput', yearInput);

    // const headers = new HttpHeaders({
    //   'Authorization': 'Bearer ' + AuthService.jwtToken
    // });
    
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.cookieService.get('token')}`,
      'Access-Control-Allow-Origin': '*'
    })

    return this.http.get<any[]>(`${environment.apiBaseUrl}/GetValidData`, { params: params, headers: headers});
    

    // return this.http.get<ReceivedList[]>(`${this.apiUrl}/${selectedItem}`);

  }
  downloadFiles(dateGiven: string): Observable<any> {
    
    let params = new HttpParams();
    params = params.append('dateString', dateGiven);


    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.cookieService.get('token')}`,
      'Access-Control-Allow-Origin': '*'
    })
    
    return this.http.get(`${environment.apiBaseUrl}/DownloadFiles`, { responseType: 'blob' , params: params, headers:headers },);

   

  }
}