import {Component, Input, OnInit} from '@angular/core';
import {Observable, of} from 'rxjs';
import {GamesService} from "../../services/games.service";
import {GameCollectionItem} from "../../models/models";
import {Router} from "@angular/router";

@Component({
  selector: 'app-collection',
  templateUrl: './collection.component.html',
  styleUrls: ['./collection.component.scss']
})
export class CollectionComponent implements OnInit {
  @Input() addToPartyMode: boolean = false;

  //Only used in addToPartyMode
  gamesSelected: GameCollectionItem[] = [];
  myGames$: Observable<GameCollectionItem[]> = of();


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
    this.router.navigate(['collection/add-game']);
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

  toggleInArray(toggled: boolean, game: GameCollectionItem) {
    if (toggled) {
      this.gamesSelected.push(game);
    } else {
      const index = this.gamesSelected.findIndex(gameSelected => gameSelected.id === game.id);
      this.gamesSelected.splice(index, 1);
    }
  }
}
