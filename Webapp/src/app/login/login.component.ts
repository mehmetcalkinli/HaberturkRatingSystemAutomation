import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { DateSelectionService } from '../date-selection.service';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  constructor(private authService: AuthService, private router: Router, private dateSelectionService:DateSelectionService,private cookieService: CookieService) {}

  username: string="";
  password: string="";

  isLogin: boolean=false;
  jwtToken: string="";
  showError: boolean = false;

  login(): void {

    this.authService.login(this.username, this.password).subscribe(item => {
      // this.isLogin = item; 
      this.jwtToken = item; 

      this.dateSelectionService.setUsername(this.username);

      this.cookieService.set('token', this.jwtToken);

     
      // AuthService.isLoggedIn=this.isLogin;
      //AuthService.jwtToken=this.jwtToken;

      // this.authService.isLoggedIn=this.isLogin;
      if(this.cookieService.get('token')!="" && this.cookieService.get('token')!=null)
    {
      this.dateSelectionService.setToken(this.cookieService.get('token'));
       this.router.navigate(['/main']); 
    }
    else
    {
      this.showError =true;
    }
    //   if(this.jwtToken!="" && this.jwtToken!=null)
    // {
    //    this.router.navigate(['/main']); 
    // }
    // else
    // {
    //   this.showError =true;
    // }

    //   if(this.isLogin)
    // {
    //    this.router.navigate(['/main']); 
    // }
    // else
    // {
    //   this.showError =true;
    // }
      
    });


    

    
    // this.authService.login(this.username, this.password)
    //   .subscribe({
    //     next: (loggedIn: boolean) => {
    //       if (loggedIn) {
          
    //         this.router.navigate(['/dashboard']); 
    //       } else {
    //         // Handle unsuccessful login here (e.g., display error message)
    //       }
    //     },
    //     error: (error) => {
    //       console.error('Login failed:', error);
    //       // Handle error here (e.g., display error message)
    //     }
    //   });
  }
  
}
