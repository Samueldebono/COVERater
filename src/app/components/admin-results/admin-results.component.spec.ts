import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminResultsComponent } from './admin-results.component';

describe('AdminResultsComponent', () => {
  let component: AdminResultsComponent;
  let fixture: ComponentFixture<AdminResultsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminResultsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminResultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
