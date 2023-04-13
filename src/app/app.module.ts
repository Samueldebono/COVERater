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
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { FooterComponent } from './components/footer/footer.component';
/* Services */
import { AlertifyService } from './services/alertify/alertify.service';
import { Helper } from './services/helper.service';
import { QuizComponent } from './components/quiz/quiz.component';
/* Timer */
import { CountdownModule } from 'ngx-countdown';
import { FinishedComponent } from './components/finished/finished.component';
import { DialogComponent } from './components/dialog/training/dialog.component';
import { CiteComponent } from './components/dialog/cite/cite.component';
import { ChartsModule } from 'ng2-charts';
import { HomeComponent } from './components/home/home.component';

import { MdbCarouselModule } from 'mdb-angular-ui-kit/carousel';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';

@NgModule({
  declarations: [
    AppComponent,
    LogInComponent,
    LogInComponent,
    NavBarComponent,
    QuizComponent,
    FinishedComponent,
    DialogComponent,
    CiteComponent,
    HomeComponent,
    FooterComponent,
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
    Helper,
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
