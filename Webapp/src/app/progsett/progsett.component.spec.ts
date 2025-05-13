import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgsettComponent } from './progsett.component';

describe('ProgsettComponent', () => {
  let component: ProgsettComponent;
  let fixture: ComponentFixture<ProgsettComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgsettComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProgsettComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
