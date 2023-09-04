import {ChangeDetectionStrategy, Component, OnInit} from '@angular/core';
import {map, Observable, of, switchMap} from 'rxjs';
import {GameDetail, GameDetailResult} from "../../models/models";
import {GamesService} from "../../services/games.service";
import {ActivatedRoute} from "@angular/router";
import {LoadingService} from "../../services/loading.service";
import {MessagesService} from "../../services/messages.service";
import {AuthService} from "../../services/auth.service";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-game-detail',
  templateUrl: './game-detail.component.html',
  styleUrls: ['./game-detail.component.scss']
})
export class GameDetailComponent implements OnInit {

  isCustom: boolean = false;
  gameResult$: Observable<GameDetailResult> = of();


  constructor(
    private gamesService: GamesService,
    private route: ActivatedRoute,
    protected loadingService: LoadingService,
    private messagesService: MessagesService,
    protected authService: AuthService
  ) {
  }

  ngOnInit(): void {
    this.refresh();
  }

  refresh() {
    this.gameResult$ = this.route.url.pipe(
      switchMap(params => {
        this.isCustom = params[0].path === 'custom';
        if (this.isCustom) {
          return this.gamesService
            .getCustomGameDetail(params[1].path)
            .pipe(map(this.wrapToResult));
        }

      return this.gamesService.getGameDetail(params[0].path);
    }));
  }

  wrapToResult(game: GameDetail): GameDetailResult {
    return {
      inCollection: false,
      game
    }
  }

  addToCollection() {
    this.gameResult$.subscribe(res => {
      this.gamesService.addGameToCollection(res.game.id).subscribe(() => {
        this.messagesService.success('Jeu ajouté à la collection.');
        this.refresh();
      })
    })
  }

  removeFromCollection() {
    this.gameResult$.subscribe(res => {
      this.gamesService.deleteGameFromCollection(res.game.id, this.isCustom).subscribe(() => {
        this.messagesService.success('Jeu retiré de la collection.');
        this.refresh();
      })
    })
  }

  formatDescription(description: string): Observable<string> {
    //TODO: The backend should encode correctly
    return of(description.replace(/&#10;&#10;/g, '\n\r\n\r'));
  }
}
