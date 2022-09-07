import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { AuthService } from 'src/app/services/user/authentication.service';
import { UserExperienceUpdate, UserModel } from 'src/app/models/user.model';

@Component({
  selector: 'app-experience',
  templateUrl: './experience.component.html',
  styleUrls: ['./experience.component.less'],
})
export class ExperienceComponent implements OnInit {
  constructor(
    private router: Router,
    private fb: FormBuilder,
    private authService: AuthService
  ) {}

  quadratsControl: FormGroup;
  quadrats: string[];
  userExp: UserExperienceUpdate;
  user: UserModel;

  ngOnInit(): void {
    this.quadrats = ['1-20', '21-100', 'more than 100'];
    this.createquadratsControlForm();
  }

  createquadratsControlForm() {
    this.quadratsControl = this.fb.group({
      quadrat: [null, [Validators.required]],
    });
  }

  onBookChange() {}

  submit() {
    if (this.quadratsControl.valid) {
      this.userExp = {
        experience: this.quadrats.indexOf(this.quadrat.value) + 2, //offset
        userId: localStorage.getItem('id'),
      };

      this.authService.updateExperience(this.userExp).subscribe(() => {
        this.router.navigate(['/home/']);
      });
    }
  }

  get quadrat() {
    return this.quadratsControl.get('quadrat') as FormControl;
  }
}
