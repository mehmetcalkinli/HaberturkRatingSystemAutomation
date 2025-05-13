import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AppComponent } from './app.component';
import { CommentsComponent } from './comments/comments.component';
import { DateListComponent } from './date-list/date-list.component';
import { ParentComponent } from './parent/parent.component';
import { authGuard } from './auth.guard';
import { UserComponent } from './user/user.component';
import { ProgsettComponent } from './progsett/progsett.component';


const routes: Routes = [
 
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // Default route
  { path: 'user', component: UserComponent,canActivate: [authGuard] },
  { path: 'progsett', component: ProgsettComponent,canActivate: [authGuard] },

  {
  path: 'main', component: ParentComponent, canActivate: [authGuard], children: [

    { path: 'about', component: CommentsComponent },
    { path: 'clients', component: DateListComponent },
  ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
