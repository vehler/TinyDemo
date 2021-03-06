import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { AppInitializerProvider, JWTInterceptorProvider, ErrorInterceptorProvider } from './helpers/app.providers';

import { WithLoadingPipe } from './pipes/with-loading.pipe';
import { NavMenuComponent, FooterComponent,  ShellComponent } from '@app/shared/components';
import { NotFoundComponent } from './components/not-found/not-found.component';




@NgModule({
  declarations: [
    NavMenuComponent,
    WithLoadingPipe,
    FooterComponent,
    ShellComponent,
    NotFoundComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
    CommonModule,
    RouterModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,

    ShellComponent,
    NavMenuComponent,
    WithLoadingPipe,
    FooterComponent,
    NotFoundComponent
  ],
  providers: [
    AppInitializerProvider,
    JWTInterceptorProvider,
    ErrorInterceptorProvider
  ],

})
export class SharedModule { }
