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
import {SecurityLayoutComponent} from './common/security-layout/security-layout.component';
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {AuthInterceptor} from "./helpers/auth-interceptor.interceptor";
import {LocationStrategy, NgOptimizedImage, PathLocationStrategy} from "@angular/common";
import {PartyItemComponent} from "./components/party-item/party-item.component";
import {PartyDetailComponent} from './features/party-detail/party-detail.component';
import {PartyGameItemComponent} from './components/party-game-item/party-game-item.component';
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {PartyGameVoteIdentifyComponent} from './features/party-game-vote-identify/party-game-vote-identify.component';
import {MatMenuModule} from "@angular/material/menu";
import {GameItemComponent} from './components/game-item/game-item.component';
import {DialogYesNoComponent} from './common/dialog-yes-no/dialog-yes-no.component';
import {MatDialogModule} from "@angular/material/dialog";
import {GameDetailComponent} from './features/game-detail/game-detail.component';
import {SearchResultComponent} from './features/search-result/search-result.component';
import {LoadingInterceptor} from "./helpers/loading.interceptor";
import {MatPaginatorModule} from "@angular/material/paginator";
import {AddEditCustomGameComponent} from "./components/add-custom-game/add-edit-custom-game.component";
import {AddPartyComponent} from "./features/add-party/add-party.component";
import {CollectionGameItemComponent} from './components/collection-game-item/collection-game-item.component';
import { ConfirmMailComponent } from './features/confirm-mail/confirm-mail.component';
import { ChangePasswordComponent } from './features/change-password/change-password.component';
import { ForgotPasswordComponent } from './features/forgot-password/forgot-password.component';
import {MatCheckboxModule} from "@angular/material/checkbox";
import { AddGameToPartyDialogComponent } from './features/add-game-to-party-dialog/add-game-to-party-dialog.component';

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
    PartyItemComponent,
    PartyDetailComponent,
    PartyGameItemComponent,
    PartyGameVoteIdentifyComponent,
    GameItemComponent,
    DialogYesNoComponent,
    GameDetailComponent,
    SearchResultComponent,
    AddEditCustomGameComponent,
    CollectionGameItemComponent,
    AddEditCustomGameComponent,
    AddPartyComponent,
    AddGameToPartyDialogComponent,
    ConfirmMailComponent,
    ChangePasswordComponent,
    ForgotPasswordComponent,
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
    MatProgressBarModule,
    MatMenuModule,
    MatDialogModule,
    MatPaginatorModule,
    MatCheckboxModule,
    NgOptimizedImage,
  ],
  providers: [

    {
      provide: LocationStrategy,
      useClass: PathLocationStrategy
    },
    {provide: HTTP_INTERCEPTORS, multi: true, useClass: HttpErrorInterceptor},
    {provide: HTTP_INTERCEPTORS, multi: true, useClass: AuthInterceptor},
    {provide: HTTP_INTERCEPTORS, multi: true, useClass: LoadingInterceptor}
  ],


  bootstrap: [AppComponent]
})
export class AppModule {
}
