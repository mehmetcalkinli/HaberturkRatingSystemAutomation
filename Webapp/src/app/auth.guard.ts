// import { CanActivateFn } from '@angular/router';

// export const authGuard: CanActivateFn = (route, state) => {
//   return true;
// };

import { CanActivateFn } from '@angular/router';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { DateSelectionService } from './date-selection.service';
import { HttpClient, HttpHandler } from '@angular/common/http';
export const authGuard: CanActivateFn = (route, state) => {

  const router: Router = new Router; // Initialize the Router service


  
  // if (AuthService.jwtToken!=null && AuthService.jwtToken!="") {
  //   return true;
  // } else {
  //     router.navigate(['/login']);
  //   return false; 
  // }
  if (document.cookie!=null && document.cookie!="" &&document.cookie!="token=null") {
    return true;
  } else {
      router.navigate(['/login']);
    return false; 
  }
  // if (AuthService.isLoggedIn) {
  //   return true;
  // } else {
  //     router.navigate(['/login']);
  //   return false; 
  // }

 
};

