import { Component, OnInit } from '@angular/core';
import { Observable, finalize, of } from 'rxjs';
import {GamesService} from "../../services/games.service";
import {GameCollection} from "../../models/models";

@Component({
  selector: 'app-collection',
  templateUrl: './collection.component.html',
  styleUrls: ['./collection.component.scss']
})
export class CollectionComponent implements OnInit {

  isLoading = false;

  myGames$: Observable<GameCollection[]> = of();


  constructor(
    private gamesService: GamesService
  ) {}

  ngOnInit(): void {
    this.isLoading = true;

    this.myGames$ = this.gamesService.getMyGames().pipe(
      finalize(() => this.isLoading = false)
    );
  }
}
