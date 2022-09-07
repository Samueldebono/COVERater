import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminResultsComponent } from './components/admin-results/admin-results.component';
import { ExperienceComponent } from './components/experience/experience.component';
import { FinishedComponent } from './components/finished/finished.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { HomeComponent } from './components/home/home.component';
import { LogInComponent } from './components/log-in/log-in.component';
import { RegisterComponent } from './components/register/register.component';
import { StageOneComponent } from './components/stageOne/stageOne.component';
const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  { path: 'login/:route', component: LogInComponent },
  { path: 'login', component: LogInComponent },
  { path: 'register/:role', component: RegisterComponent },
  { path: 'stageOne/:id', component: StageOneComponent },
  { path: 'finish/:id', component: FinishedComponent },
  { path: 'home', component: HomeComponent },
  { path: 'experience', component: ExperienceComponent },
  { path: 'forgotPassword', component: ForgotPasswordComponent },
  { path: 'adminResults', component: AdminResultsComponent },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
