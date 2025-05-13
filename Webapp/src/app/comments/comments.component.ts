
// export class CommentsComponent implements OnInit{
//   comments:any =[];
// constructor(private dataService: DataService){}
//   ngOnInit(): void {
//     this.fetchData();
//   }
// fetchData()
// {
//   this.dataService.fetchData().subscribe(comments =>{
//     console.log(comments);
//     this.comments=comments;
//   });
// 
// } 
import { Component, OnInit } from '@angular/core'
import { DataService } from '../data.service';
import { DateSelectionService } from '../date-selection.service';
import { Comment } from '../comment.interface' // Import the Comment interface
import { ReceivedList } from '../receivedlist.interface';
import { MatTableDataSource } from '@angular/material/table';
import { WjGridModule } from '@grapecity/wijmo.angular2.grid';
import * as wjCore from '@grapecity/wijmo';
import { saveAs } from "file-saver";
import { DateListComponent } from '../date-list/date-list.component';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-comments',
templateUrl: './comments.component.html',
styleUrl: './comments.component.css'
})
export class CommentsComponent implements OnInit {

//comments: ReceivedList[] = [];
receivedlist:any =[];
month: String = '';
monthNumber: number=0;

selectedMonth: string ='';
selectedYear: string ='';


panelOpen: boolean =false;
panelOpen2: boolean=false;

dateSelected: Date = new Date();

dataSource = new MatTableDataSource<any>([]); // Initialize with your data array
selectedItem: string = '';
  apiResponse: any;

showRTG: boolean = false;
showShare: boolean = false;
showAverage: boolean = false;
showWj:boolean=false;

All: boolean = true; // Initial state
NTV: boolean = true; // Initial state
HABERTURK: boolean = true; // Initial state
CNNTURK  : boolean = true; // Initial state
HALKTV     : boolean = true; // Initial state
KRT        : boolean = true; // Initial state
AHABER     : boolean = true; // Initial state
TRTHABER   : boolean = true; // Initial state
TELE1      : boolean = true; // Initial state
TV100      : boolean = true; // Initial state
HABERGLOBAL: boolean = true; // Initial state
SOZCUTV  : boolean = true; // Initial state

usernameCheck: boolean=false;

dateGiven: string="";

constructor(private dateSelectionService: DateSelectionService,private authService: AuthService,private router: Router) {
  

}


ngOnInit() {
  

  


  this.dateSelectionService.getUsername().subscribe(item => {
    if (item=="admin") {

      this.usernameCheck=true;
    }
  });


}

getir()
{
  
  this.dateSelectionService.fetchDataBasedOnSelectedItem(this.convertToInputWithDash(this.dateGiven)).subscribe((comments) => {
    this.receivedlist = comments;

   this.monthClicked();
  });
}

logout ()
{

  this.authService.logout();
  
  this.router.navigate(['/login']);

}


download() {

 this.dateSelectionService.downloadFiles(this.convertToInputWithDash(this.dateGiven)).subscribe(data => saveAs(data, 'Example.zip'));

}

  
 
convertToInputWithDash(dateString:string)
{
  let formattedDate = dateString.split('.').join('-');
  return formattedDate;
}



monthClicked():void{
  // this.month = this.getMonth(this.receivedlist.propertyName3[this.receivedlist.propertyName3.length-2].tableDate.getMonth()+1);
  
  const dateObject = new Date(this.receivedlist.propertyName3[this.receivedlist.propertyName3.length - 2].tableDate);
  console.log(dateObject);
  
  this.monthNumber= dateObject.getMonth() + 1;
  console.log(this.monthNumber);
  
  // Extract the month (zero-based), add 1 to make it 1-based
  // this.month =this.getMonthString(this.monthNumber);
  this.month=this.getMonthName(this.monthNumber);
  console.log(this.month);
  
}
toggleShowWj(): void {
  this.showWj = !this.showWj;
  this.showRTG = false;

  this.showShare = false;
  this.showAverage = false;

  if(this.showWj==true)
{
  this.All =true;

  this.NTV = true;
  this.HABERTURK= true;
  this.CNNTURK  = true;
  this.HALKTV    = true; 
  this.KRT      = true;  
  this.AHABER    = true; 
  this.TRTHABER   = true;
  this.TELE1      = true;
  this.TV100     = true; 
  this.HABERGLOBAL= true;
  this.SOZCUTV  = true;
}else if(this.showWj==false)
{
  this.All =false;

  this.NTV = false;
  this.HABERTURK= false;
  this.CNNTURK  = false;
  this.HALKTV    = false; 
  this.KRT      = false;  
  this.AHABER    = false; 
  this.TRTHABER   = false;
  this.TELE1      = false;
  this.TV100     = false; 
  this.HABERGLOBAL= false;
  this.SOZCUTV  = false;
}
}
toggleShowRTG(): void {
  this.showRTG = !this.showRTG;
  this.showShare = false;
  this.showAverage = false;

  if(this.showRTG==true)
{
  this.All =true;

  this.NTV = true;
  this.HABERTURK= true;
  this.CNNTURK  = true;
  this.HALKTV    = true; 
  this.KRT      = true;  
  this.AHABER    = true; 
  this.TRTHABER   = true;
  this.TELE1      = true;
  this.TV100     = true; 
  this.HABERGLOBAL= true;
  this.SOZCUTV  = true;
}else if(this.showRTG==false)
{
  this.All =false;

  this.NTV = false;
  this.HABERTURK= false;
  this.CNNTURK  = false;
  this.HALKTV    = false; 
  this.KRT      = false;  
  this.AHABER    = false; 
  this.TRTHABER   = false;
  this.TELE1      = false;
  this.TV100     = false; 
  this.HABERGLOBAL= false;
  this.SOZCUTV  = false;
}
}
toggleShowShare(): void {
  this.showShare = !this.showShare;
  this.showRTG = false;
  this.showAverage = false;

if(this.showShare==true)
{
  this.All =true;

  this.NTV = true;
  this.HABERTURK= true;
  this.CNNTURK  = true;
  this.HALKTV    = true; 
  this.KRT      = true;  
  this.AHABER    = true; 
  this.TRTHABER   = true;
  this.TELE1      = true;
  this.TV100     = true; 
  this.HABERGLOBAL= true;
  this.SOZCUTV  = true;
}
else if(this.showShare==false)
{
  this.All =false;

  this.NTV = false;
  this.HABERTURK= false;
  this.CNNTURK  = false;
  this.HALKTV    = false; 
  this.KRT      = false;  
  this.AHABER    = false; 
  this.TRTHABER   = false;
  this.TELE1      = false;
  this.TV100     = false; 
  this.HABERGLOBAL= false;
  this.SOZCUTV  = false;
}


}
toggleShowAverage(): void {
  this.showAverage = !this.showAverage;
  this.showRTG = false;
  this.showShare = false;

  if(this.showAverage==true)
{
  this.All =true;

  this.NTV = true;
  this.HABERTURK= true;
  this.CNNTURK  = true;
  this.HALKTV    = true; 
  this.KRT      = true;  
  this.AHABER    = true; 
  this.TRTHABER   = true;
  this.TELE1      = true;
  this.TV100     = true; 
  this.HABERGLOBAL= true;
  this.SOZCUTV  = true;
}else if(this.showAverage==false)
{
  this.All =false;

  this.NTV = false;
  this.HABERTURK= false;
  this.CNNTURK  = false;
  this.HALKTV    = false; 
  this.KRT      = false;  
  this.AHABER    = false; 
  this.TRTHABER   = false;
  this.TELE1      = false;
  this.TV100     = false; 
  this.HABERGLOBAL= false;
  this.SOZCUTV  = false;
}
}

toggleAll(): void {
 

  if(this.All == true)
{
  this.NTV = true;
  this.HABERTURK= true;
  this.CNNTURK  = true;
  this.HALKTV    = true; 
  this.KRT      = true;  
  this.AHABER    = true; 
  this.TRTHABER   = true;
  this.TELE1      = true;
  this.TV100     = true; 
  this.HABERGLOBAL= true;
  this.SOZCUTV  = true;
}
if(this.All == false)
{
this.NTV = false;
  this.HABERTURK= false;
  this.CNNTURK  = false;
  this.HALKTV    = false; 
  this.KRT      = false;  
  this.AHABER    = false; 
  this.TRTHABER   = false;
  this.TELE1      = false;
  this.TV100     = false; 
  this.HABERGLOBAL= false;
  this.SOZCUTV  = false;
}
}

// getMonthName(monthNumber: number) {
//   const date = new Date();
//   date.setMonth(monthNumber - 1);
// debugger
//   return date.toLocaleString('tr-TR', {
//     month: 'long',
//   });
// }
getMonthName(monthNumber: number) {
  const date = new Date(2000, monthNumber - 1, 1);

  return date.toLocaleString('tr-TR', {
    month: 'long',
  });
}
getDayName(dateStr: string)
{
    var date = new Date(dateStr);
    return date.toLocaleDateString('tr-TR', { weekday: 'long' });        
}

goToUser() {
  this.router.navigate(['/user']);
}
goToProgramSettings() {
  this.router.navigate(['/progsett']);
}

togglePanel() {
  this.panelOpen = !this.panelOpen;
}
togglePanel2() {
  this.panelOpen2 = !this.panelOpen2;
}

}

