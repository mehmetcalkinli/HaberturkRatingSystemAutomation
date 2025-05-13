import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReceivedList } from './receivedlist.interface';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  baseUrl=`https://jsonplaceholder.typicode.com`;
  apiUrl=`https://localhost:7238/RatingSystem/GetExcelData`;


  constructor(private http: HttpClient) { }
  fetchData(){
    //return this.http.get(`${this.baseUrl}/comments`);
    return this.http.get<ReceivedList[]>(`${this.apiUrl}`);

  }
}
