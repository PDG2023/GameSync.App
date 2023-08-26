import {Component, OnInit} from '@angular/core';
import {catchError, finalize, Observable, of} from 'rxjs';
import {GameDetail} from "../../models/models";
import {GamesService} from "../../services/games.service";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-game-detail',
  templateUrl: './game-detail.component.html',
  styleUrls: ['./game-detail.component.scss']
})
export class GameDetailComponent implements OnInit {
  game$: Observable<GameDetail> = of();
  isLoading: boolean = true;
  isError: boolean = false;


  constructor(
    private gamesService: GamesService,
    private route: ActivatedRoute
  ) {
  }

  ngOnInit(): void {
    this.game$ = this.gamesService.getGameDetail(this.route.snapshot.params['id'])
      .pipe(
        catchError(() => {
          console.log("coucou erreur")
          this.isError = true;
          return of();
        }),
        finalize(() => this.isLoading = false)
      );
  }
}
