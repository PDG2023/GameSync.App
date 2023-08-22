import {NgModule} from '@angular/core';
import {RouterModule, Routes} from "@angular/router";
import {HomeComponent} from "./features/home/home.component";
import {CollectionComponent} from "./features/collection/collection.component";
import {PartiesComponent} from "./features/parties/parties.component";
import {LoginComponent} from "./features/login/login.component";
import {SecurityLayoutComponent} from "./common/security-layout/security-layout.component";
import {RegisterComponent} from "./features/register/register.component";
import {authGuardChild} from "./guards/auth.guard";
import {anonymousGuardChild} from "./guards/anonymous.guard";

const anonymousOnlyRoutes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent}
]

const authOnlyRoutes: Routes = [
  {path: 'parties', component: PartiesComponent},
  {path: 'collection', component: CollectionComponent}
]

const routes: Routes = [
  {path: '', component: HomeComponent, pathMatch: 'full'},
  {path: '', canActivateChild: [authGuardChild], children: authOnlyRoutes},
  {
    path: '',
    canActivateChild: [anonymousGuardChild],
    component: SecurityLayoutComponent,
    children: anonymousOnlyRoutes
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
