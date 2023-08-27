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

const anonymousOnlyRoutes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent}
]

const authOnlyRoutes: Routes = [
  {path: 'parties', component: PartiesComponent},
  {path: 'collection', component: CollectionComponent},
  {path: 'parties/:id', component: PartyDetailComponent},
  {path: 'parties/:id/vote-guest', component: PartyGameVoteIdentifyComponent},
]

const routes: Routes = [
  {path: '', component: HomeComponent, pathMatch: 'full'},
  {path: 'games/:id', component: GameDetailComponent},
  {path: '', canActivateChild: [authGuardChild], children: authOnlyRoutes},
  {
    path: '',
    canActivateChild: [anonymousGuardChild],
    component: SecurityLayoutComponent,
    children: anonymousOnlyRoutes
  },
  {path: 'games', component: SearchResultComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
