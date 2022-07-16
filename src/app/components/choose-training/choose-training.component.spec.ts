import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseTrainingComponent } from './choose-training.component';

describe('ChooseTrainingComponent', () => {
  let component: ChooseTrainingComponent;
  let fixture: ComponentFixture<ChooseTrainingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseTrainingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseTrainingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
