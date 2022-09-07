import { BrowserModule } from '@angular/platform-browser';
/* Routing */
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpErrorInterceptorService } from './services/httperor-interceptor.service';
/* Angular Material */
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularMaterialModule } from './angular-material.module';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { NgxSliderModule } from '@angular-slider/ngx-slider';
/* FormsModule */
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
/* Angular Flex Layout */
import { FlexLayoutModule } from '@angular/flex-layout';
/* Components */
import { LogInComponent } from './components/log-in/log-in.component';
import { RegisterComponent } from './components/register/register.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
/* Services */
import { AlertifyService } from './services/alertify/alertify.service';
import { StageOneComponent } from './components/stageOne/stageOne.component';
/* Timer */
import { CountdownModule } from 'ngx-countdown';
import { FinishedComponent } from './components/finished/finished.component';
import { DialogComponent } from './components/dialog/dialog.component';
import { ChartsModule } from 'ng2-charts';
import { HomeComponent } from './components/home/home.component';
import { ExperienceComponent } from './components/experience/experience.component';

import { MdbCarouselModule } from 'mdb-angular-ui-kit/carousel';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { AdminResultsComponent } from './components/admin-results/admin-results.component';

@NgModule({
  declarations: [
    AppComponent,
    LogInComponent,
    RegisterComponent,
    RegisterComponent,
    LogInComponent,
    ForgotPasswordComponent,
    NavBarComponent,
    StageOneComponent,
    FinishedComponent,
    DialogComponent,
    HomeComponent,
    ExperienceComponent,
    AdminResultsComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    AngularMaterialModule,
    ReactiveFormsModule,
    FormsModule,
    FlexLayoutModule,
    HttpClientModule,
    CountdownModule,
    ChartsModule,
    NgxSliderModule,
    MdbCarouselModule,
    CommonModule,
    MatTableModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptorService,
      multi: true,
    },
    AlertifyService,
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
