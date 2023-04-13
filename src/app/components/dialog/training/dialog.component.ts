import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';
import { AnswerSuccess } from 'src/app/models/enums.model';

export interface DialogData {
  percentage: number;
  correctPercentage: number;
  answer: AnswerSuccess;
}

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.less'],
})
export class DialogComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<DialogComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  percentage: string;
  correctPercentage: string;
  dialogBox: FormGroup;
  successful: AnswerSuccess;
  AnswerSuccess = AnswerSuccess;

  ngOnInit(): void {
    this.percentage = this.cleanPercentage(
      (this.data.percentage * 100).toFixed(2).toString()
    );
    this.correctPercentage = this.cleanPercentage(
      (this.data.correctPercentage * 100).toFixed(2).toString()
    );
    this.successful = this.data.answer;
    this.dialogBox = this.fb.group({});
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  cleanPercentage(num: string): string {
    if (num.substring(num.length - 2) === '00') {
      num = num.substring(0, num.length - 3);
    } else if (num.substring(num.length - 1) == '0') {
      num = num.substring(0, num.length - 1);
    }
    return num;
  }
}
