import { AppRoutingModule } from './app-routing.module';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA  } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';

import { JwtInterceptor, ErrorInterceptor } from './helpers';
import { LoginComponent } from './login';
import { StudentListComponent, StudentCreateComponent, StudentUpdateComponent } from './student';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { NgPipesModule } from 'ngx-pipes';
import { ClickableHeadComponent } from './clickableHead';


@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        StudentListComponent,
        StudentCreateComponent,
        StudentUpdateComponent,
        ClickableHeadComponent
    ],
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        HttpClientModule,
        AppRoutingModule,
        FormsModule,
        BrowserAnimationsModule,
        ToastrModule.forRoot({
            timeOut: 2000,
            positionClass: 'toast-bottom-right',
            preventDuplicates: false,
        }),
        NgPipesModule
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },

    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
