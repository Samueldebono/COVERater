import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FinishedComponent } from './components/finished/finished.component';
import { HomeComponent } from './components/home/home.component';
import { LogInComponent } from './components/log-in/log-in.component';
import { QuizComponent } from './components/quiz/quiz.component';
const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  { path: 'login', component: LogInComponent },
  { path: 'quiz/:id/:userId', component: QuizComponent },
  { path: 'finish/:id', component: FinishedComponent },
  { path: 'home', component: HomeComponent },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
