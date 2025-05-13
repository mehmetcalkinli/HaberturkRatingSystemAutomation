import { Component } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { AuthService } from '../auth.service';
import { UserService } from '../user.service';
import { ProgsettService } from '../progsett.service';
@Component({
  selector: 'app-progsett',
  templateUrl: './progsett.component.html',
  styleUrl: './progsett.component.css'
})

export class ProgsettComponent {

  MailAdress: string ='';
  MailPassword:string='';
  KantarMail: string='';
  GonderilenMail:string='';
  IngestMail: string='';

  
  logg:any;
  constructor(private http: HttpClient,private userService:UserService,private progsettService:ProgsettService) { }

  ngOnInit() {

    this.progsettService.getConfig().subscribe((comments) => {
      this.logg = comments;
      debugger
      this.MailAdress= comments.MailSettings.MailAdress;
      this.MailPassword= comments.MailSettings.MailPassword;
      this.KantarMail= comments.MailSettings.KantarMail;
      this.GonderilenMail= comments.MailSettings.GonderilenMail;
      this.IngestMail= comments.MailSettings.IngestMail;

    console.log(this.logg);
  });
  }

  saveProp(){
    this.progsettService.setConfig(this.MailAdress,this.MailPassword,this.KantarMail,this.IngestMail,this.GonderilenMail).subscribe((comments) => {
      this.logg = comments;
    console.log(this.logg);
  });

    
  }
//   addUser() {
  
// this.userService.addUser(this.username,this.password).subscribe((comments) => {
//   this.logg = comments;
// console.log(this.logg);

// });
//   }

 
//   changePassword(){
//     this.userService.changePassword(this.username,this.newPassword).subscribe((comments) => {
//       this.logg = comments;
//     console.log(this.logg);
    
//     });
//   }

//   deleteUser(){
//     this.userService.deleteUser(this.username).subscribe((comments) => {
//       this.logg = comments;
//     console.log(this.logg);
    
//     });
//   }
}
