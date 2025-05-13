import { Component, HostListener } from '@angular/core';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'ratingsysweb';
  showRTG: boolean = false;
  showShare: boolean = false;
  showAverage: boolean = false;

  constructor(private authService: AuthService) { }

  toggleShowRTG(): void {
    this.showRTG = !this.showRTG;
    this.showShare = false;
    this.showAverage = false;
  }
  toggleShowShare(): void {
    this.showShare = !this.showShare;
    this.showRTG = false;
    this.showAverage = false;
  
  }
  toggleShowAverage(): void {
    this.showAverage = !this.showAverage;
    this.showRTG = false;
    this.showShare = false;
  
  }

  @HostListener('window:beforeunload', ['$event'])
  clearTokenOnClose(event: Event): void {
    this.authService.logout();
  }
}
