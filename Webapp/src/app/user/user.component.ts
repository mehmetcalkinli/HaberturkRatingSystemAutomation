import { Component } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { AuthService } from '../auth.service';
import { UserService } from '../user.service';
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  username: string='';
  password: string='';
  newPassword: string='';

  logg:string='';

  passwordVisible: boolean = false;
  visibilityType:string="password";
  constructor(private http: HttpClient,private userService:UserService) { }

  togglePasswordVisibility() {
    this.passwordVisible = !this.passwordVisible;
    if (this.passwordVisible==true) {
      this.visibilityType="text";
    }
    else if (this.passwordVisible==false) {
      this.visibilityType="password";

    }
  }

  // addUser() {
  //   // const userData = {
  //   //   username: this.username,
  //   //   password: this.password,
  //   //   newPassword: this.newPassword

  //   // };
  //   debugger
  //   const headers = new HttpHeaders({
  //     'Content-Type': 'application/json',
  //     'Authorization': `Bearer ${AuthService.jwtToken}`,
  //     'Access-Control-Allow-Origin': '*'
  //   })

  //   let params = new HttpParams();
  //   params = params.append('username', this.username);
  //   params = params.append('password', this.password);


  //   this.http.post<any>(`https://localhost:7238/RatingSystem/AddUser`, { params: params, headers: headers });

  //   // this.http.post<any>('https://localhost:7238/RatingSystem/AddUser',{ params: params, headers: headers })
  //   // .subscribe(
  //   //   response => {
  //   //     console.log('User added successfully:', response);
  //   //     // Handle success response
  //   //   },
  //   //   error => {
  //   //     console.error('Error adding user:', error);
  //   //     // Handle error response
  //   //   }
  //   // );
  // }

  // changePassword(){
  //   const headers = new HttpHeaders({
  //     'Content-Type': 'application/json',
  //     'Authorization': `Bearer ${AuthService.jwtToken}`,
  //     'Access-Control-Allow-Origin': '*'
  //   })

  //   let params = new HttpParams();
  //   params = params.append('username', this.username);
  //   params = params.append('password', this.newPassword);


  //   this.http.post<any>(`https://localhost:7238/RatingSystem/ChangePassword`, { params: params, headers: headers });

  // }

  // deleteUser(){
  //   const headers = new HttpHeaders({
  //     'Content-Type': 'application/json',
  //     'Authorization': `Bearer ${AuthService.jwtToken}`,
  //     'Access-Control-Allow-Origin': '*'
  //   })

  //   let params = new HttpParams();
  //   params = params.append('username', this.username);
   

  //   this.http.post<any>(`https://localhost:7238/RatingSystem/DeleteUser`, { params: params, headers: headers });

  // }
  addUser() {
    // this.userService.addUser(this.username,this.password);
// debugger

this.userService.addUser(this.username,this.password).subscribe((comments) => {
  this.logg = comments;
console.log(this.logg);

});
  }

  //   this.userService.addUser(this.username,this.password).subscribe({
  //     next: (response: any) =>{
  //       // TO-DO: Handling when the response is succeed
  //     },
  //     error: (err: any) =>{
  //       console.log(err);
        
  //       // TO-DO: Handling when the response is failed
  //     },
  //     complete: () =>{
  //       // TO-DO: Handling when `next` and `error` callback is completed.
  //     }
  //   });
  // }

  changePassword(){
    this.userService.changePassword(this.username,this.newPassword).subscribe((comments) => {
      this.logg = comments;
    console.log(this.logg);
    
    });
  }

  deleteUser(){
    this.userService.deleteUser(this.username).subscribe((comments) => {
      this.logg = comments;
    console.log(this.logg);
    
    });
  }
}
