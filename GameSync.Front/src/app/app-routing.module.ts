import {NgModule} from '@angular/core';
import {RouterModule, Routes} from "@angular/router";
import {HomeComponent} from "./features/home/home.component";
import {CollectionComponent} from "./features/collection/collection.component";
import {PartiesComponent} from "./features/parties/parties.component";

const routes: Routes = [
  {path: '', component: HomeComponent, pathMatch: 'full'},
  {path: 'collection', component: CollectionComponent},
  {path: 'parties', component: PartiesComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
