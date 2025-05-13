import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CommentsComponent } from './comments/comments.component';
import { HttpClientModule } from '@angular/common/http';
import { WjGridModule } from '@grapecity/wijmo.angular2.grid';
import { FormsModule } from '@angular/forms'; // Import FormsModule
import { MatTableModule } from '@angular/material/table';
import { DateListComponent } from './date-list/date-list.component';
import { MatIconModule } from '@angular/material/icon';
import { LoginComponent } from './login/login.component';
import { ParentComponent } from './parent/parent.component';
import { MatError } from '@angular/material/form-field';
import { UserComponent } from './user/user.component';
import { CookieService } from 'ngx-cookie-service';
import { ProgsettComponent } from './progsett/progsett.component';
 

@NgModule({
  declarations: [
    AppComponent,
    CommentsComponent,
    DateListComponent,
    LoginComponent,
    ParentComponent,
    UserComponent,
    ProgsettComponent,
    
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    WjGridModule,
    FormsModule,
    MatTableModule,
    MatIconModule,
    MatError
  ],
  providers: [CookieService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
