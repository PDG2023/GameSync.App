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
import {PartyDetailComponent} from "./features/party-detail/party-detail.component";
import {PartyGameVoteIdentifyComponent} from "./features/party-game-vote-identify/party-game-vote-identify.component";
import {GameDetailComponent} from "./features/game-detail/game-detail.component";
import {SearchResultComponent} from "./features/search-result/search-result.component";
import {AddEditCustomGameComponent} from "./components/add-custom-game/add-edit-custom-game.component";
import { ConfirmMailComponent } from './features/confirm-mail/confirm-mail.component';

const anonymousOnlyRoutes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'confirm-mail', component: ConfirmMailComponent}
]

const authOnlyRoutes: Routes = [
  {path: 'parties', component: PartiesComponent},
  {path: 'collection', component: CollectionComponent},
  {
    path: 'collection',
    children: [
      {path: 'add-game', component: AddEditCustomGameComponent},
      {path: 'edit-game/:id', component: AddEditCustomGameComponent}
    ]
  },
  {path: 'parties/:id', component: PartyDetailComponent},
  {path: 'parties/:id/vote-guest', component: PartyGameVoteIdentifyComponent},
]

const routes: Routes = [
  {path: '', component: HomeComponent, pathMatch: 'full'},
  {path: 'games', component: SearchResultComponent},
  {
    path: 'games',
    children: [
      {path: ':id', component: GameDetailComponent},
      {path: 'custom/:id', component: GameDetailComponent}
    ],
  },
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
