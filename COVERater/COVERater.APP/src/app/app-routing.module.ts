import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home';
import { AuthGuard } from './_helpers';

const accountModule = () => import('./account/account.module').then(x => x.AccountModule);
const quizPageModule = () => import('./quiz/quiz-page/quiz-page.component').then(x => x.QuizPageComponent);
const quizLoaderModule = () => import('./quiz/quiz-loader/quiz-loader.component').then(x => x.QuizLoaderComponent);
const usersModule = () => import('./users/users.module').then(x => x.UsersModule);
const homeModule = () => import('./home/home-page/home-page.component').then(x => x.HomePageComponent);

const routes: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'users', loadChildren: usersModule, canActivate: [AuthGuard] },
    { path: 'account', loadChildren: accountModule },
    { path: 'home', loadChildren: homeModule, canActivate: [AuthGuard] },
    { path:'quiz',loadChildren: quizPageModule, canActivate: [AuthGuard]  },
    { path:'quizLoader',loadChildren: quizLoaderModule, canActivate: [AuthGuard]  },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
