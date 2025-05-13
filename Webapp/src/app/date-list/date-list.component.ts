import { Component, OnInit, Input } from '@angular/core';
import { DateSelectionService } from '../date-selection.service';
import { MatListModule } from '@angular/material/list';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';



@Component({
  selector: 'app-date-list',
  templateUrl: './date-list.component.html',
  styleUrl: './date-list.component.css',
  
})


export class DateListComponent implements OnInit {
  // items: string[] = ['19-02-2024', '18-02-2024', '17-02-2024', '12-02-2024']; // Sample list of items
  // selectedItems: string[] = [];
  listValid: string[]=[];
  selectedItem: string = '';



  // selectedOption: string = ''; // Variable to store the selected option

  // options: string[] = ['Option 1', 'Option 2', 'Option 3', 'Option 4']; // Define your options here

  isMonthOpen = false;
  selectedMonth: string ='';
  optionsMonth: string[] = ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'];

  toggleMonthDropdown() {
    this.isMonthOpen = !this.isMonthOpen;
  }

  selectMonthOption(option: string) {
    this.selectedMonth = option;
    this.isMonthOpen = false;

    this.dateSelectionService.setSelectedMonth(this.selectedMonth);

    this.dateSelectionService.getValidList(this.selectedMonth, this.selectedYear).subscribe((list: string[]) => {
      this.listValid = list; // Assign the received list to your component property
    });
    
  }


  isYearOpen = false;
  selectedYear: string  = '';
  optionsYear: string[] = ['2023', '2024','2025','2026','2027','2028','2029','2030','2031','2032','2033','2034'];

  toggleYearDropdown() {
    this.isYearOpen = !this.isYearOpen;
  }

  selectYearOption(option: string) {
    this.selectedYear = option;
    this.isYearOpen = false;

    this.dateSelectionService.setSelectedYear(this.selectedYear);


   
    this.dateSelectionService.getValidList(this.selectedMonth, this.selectedYear).subscribe((list: string[]) => {
      
      this.listValid = list; // Assign the received list to your component property
    });
    
  }





  constructor(private dateSelectionService: DateSelectionService) { }

  ngOnInit(): void {

    
    // console.log(this.selectedMonth,this.selectedYear);
   
    
    // this.dateSelectionService.getValidList(this.selectedMonth, this.selectedYear).subscribe((list: string[]) => {
    //   this.listValid = list; // Assign the received list to your component property
    // });
    
     
    // this.dateSelectionService.getValidList("02", "2024").subscribe((list: string[]) => {
    //   this.listValid = list; // Assign the received list to your component property
    // });
  }
  onItemSelected(event: any) {
    // this.selectedItem = item;
    let extractedDate = event.target.value.split("'")[1]; // Splitting by single quote and taking the second part

    // this.selectedItem = event.target.value;
    this.selectedItem = extractedDate;

    this.dateSelectionService.setSelectedItem(this.selectedItem);
   
    
    // this.listValid=this.dateSelectionService.getValidList("02","2024");
  }
}