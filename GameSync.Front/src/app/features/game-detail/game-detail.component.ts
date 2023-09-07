import {ChangeDetectionStrategy, Component, OnInit} from '@angular/core';
import {map, Observable, of, switchMap} from 'rxjs';
import {GameDetail} from "../../models/models";
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
  game?: GameDetail;
  inCollection: boolean = false;

  constructor(
    private gamesService: GamesService,
    private route: ActivatedRoute,
    protected loadingService: LoadingService,
    private messagesService: MessagesService,
    protected authService: AuthService
  ) {
  }

  ngOnInit(): void {
    this.route.url.pipe(
      switchMap(segments => {
        this.isCustom = segments[0].path === 'custom';
        if (this.isCustom) {
          return this.gamesService
            .getCustomGameDetail(segments[1].path)
            .pipe(map(game => ({inCollection: false, game})));
        }
        return this.gamesService.getGameDetail(segments[0].path);
      }))
      .subscribe(result => {
        this.game = result.game;
        this.inCollection = result.inCollection;
      });
  }

  addToCollection() {
    if (this.game) {
      this.gamesService
        .addGameToCollection(this.game.id)
        .subscribe(() => {
          this.messagesService.success('Jeu ajouté à la collection.');
          this.inCollection = true;
        });
    }
  }

  removeFromCollection() {
    if (this.game) {
      this.gamesService
        .deleteGameFromCollection(this.game.id, this.isCustom)
        .subscribe(() => {
          this.messagesService.success('Jeu retiré à la collection.');
          this.inCollection = false;
        });
    }
  }

  formatDescription(description: string): Observable<string> {
    //TODO: The backend should encode correctly
    return of(description.replace(/&#10;&#10;/g, '\n\r\n\r'));
  }
}
