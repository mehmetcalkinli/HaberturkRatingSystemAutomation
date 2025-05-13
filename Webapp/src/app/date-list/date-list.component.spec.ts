import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DateListComponent } from './date-list.component';

describe('DateListComponent', () => {
  let component: DateListComponent;
  let fixture: ComponentFixture<DateListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DateListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DateListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
