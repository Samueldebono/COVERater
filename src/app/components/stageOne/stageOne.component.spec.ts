import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StageOneComponent } from './stageOne.component';

describe('StageOneComponent', () => {
  let component: StageOneComponent;
  let fixture: ComponentFixture<StageOneComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StageOneComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StageOneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
