import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {AppRoutingModule} from './app-routing.module';
import {HomeComponent} from "./features/home/home.component";
import {MatSidenavModule} from "@angular/material/sidenav";
import {SideNavComponent} from './common/side-nav/side-nav.component';
import {MatListModule} from "@angular/material/list";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatButtonModule} from "@angular/material/button";
import {MatIconModule} from "@angular/material/icon";
import {CollectionComponent} from './features/collection/collection.component';
import {PartiesComponent} from './features/parties/parties.component';
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatAutocompleteModule} from "@angular/material/autocomplete";
import {MatInputModule} from "@angular/material/input";
import {ReactiveFormsModule} from "@angular/forms";
import {SearchComponent} from './common/search/search.component';
import {LoginComponent} from './features/login/login.component';
import {MatCardModule} from "@angular/material/card";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {RegisterComponent} from './features/register/register.component';
import {HttpErrorInterceptor} from "./helpers/http-error.interceptor";
import { SecurityLayoutComponent } from './common/security-layout/security-layout.component';
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {AuthInterceptor} from "./helpers/auth-interceptor.interceptor";
import {LocationStrategy, PathLocationStrategy} from "@angular/common";
import {MatMenuModule} from "@angular/material/menu";
import { CollectionItemComponent } from './components/collection-item/collection-item.component';
import { DialogYesNoComponent } from './common/dialog-yes-no/dialog-yes-no.component';
import {MatDialogModule} from "@angular/material/dialog";
import { GameDetailComponent } from './features/game-detail/game-detail.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    SideNavComponent,
    CollectionComponent,
    PartiesComponent,
    SearchComponent,
    LoginComponent,
    RegisterComponent,
    SecurityLayoutComponent,
    CollectionItemComponent,
    DialogYesNoComponent,
    GameDetailComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MatSidenavModule,
    MatListModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    MatInputModule,
    ReactiveFormsModule,
    MatCardModule,
    HttpClientModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatMenuModule,
    MatDialogModule
  ],
  providers: [
    {
      provide: LocationStrategy,
      useClass: PathLocationStrategy
    },
    {provide: HTTP_INTERCEPTORS, multi: true, useClass: HttpErrorInterceptor},
    {provide: HTTP_INTERCEPTORS, multi: true, useClass: AuthInterceptor}
  ],


  bootstrap: [AppComponent]
})
export class AppModule {
}
