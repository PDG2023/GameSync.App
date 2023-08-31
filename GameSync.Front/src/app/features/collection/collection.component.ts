import {Component, OnInit} from '@angular/core';
import {Observable, of} from 'rxjs';
import {GamesService} from "../../services/games.service";
import {GameCollection} from "../../models/models";
import {Router} from "@angular/router";

@Component({
  selector: 'app-collection',
  templateUrl: './collection.component.html',
  styleUrls: ['./collection.component.scss']
})
export class CollectionComponent implements OnInit {
  myGames$: Observable<GameCollection[]> = of();


  constructor(
    private gamesService: GamesService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.refresh();
  }

  refresh() {
    this.myGames$ = this.gamesService.getMyGames();
  }

  addGame() {
    this.router.navigateByUrl('/add-game');
  }

  goToSearch() {
    this.router.navigate(
      ['/games'],
      {
        queryParams: {
          Query: 'a',
          PageSize: 20,
          Page: 0
        }
      }
    );
  }
}
