import {Component} from '@angular/core';
import {PartiesService} from "../../services/parties.service";
import {Observable, of} from "rxjs";
import {Party} from "../../models/models";
import {Router} from "@angular/router";

@Component({
  selector: 'app-parties',
  templateUrl: './parties.component.html',
  styleUrls: ['./parties.component.scss']
})
export class PartiesComponent {

  myParties$: Observable<Party[]> = of();

  constructor(
    private partiesService: PartiesService,
    private router: Router,
  ) {
    this.refresh();
  }

  refresh() {
    this.myParties$ = this.partiesService.getMyParties();
  }

  addParty() {
    this.router.navigateByUrl('/add-party');
  }


}
