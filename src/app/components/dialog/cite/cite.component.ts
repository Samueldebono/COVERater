import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { DialogComponent } from '../training/dialog.component';

@Component({
  selector: 'app-cite',
  templateUrl: './cite.component.html',
  styleUrls: ['./cite.component.less'],
})
//placeholder for later work
export class CiteComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<DialogComponent>,
    private fb: FormBuilder
  ) {}

  dialogBox: FormGroup;
  ngOnInit(): void {
    this.dialogBox = this.fb.group({});
  }

  click(): void {
    this.dialogRef.close();
  }
}
