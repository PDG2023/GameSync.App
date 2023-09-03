import {ChangeDetectionStrategy, Component, OnInit} from '@angular/core';
import {Observable, of, switchMap} from 'rxjs';
import {GameDetail} from "../../models/models";
import {GamesService} from "../../services/games.service";
import {ActivatedRoute} from "@angular/router";
import {LoadingService} from "../../services/loading.service";
import {MessagesService} from "../../services/messages.service";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-game-detail',
  templateUrl: './game-detail.component.html',
  styleUrls: ['./game-detail.component.scss']
})
export class GameDetailComponent implements OnInit {

  isCustom: boolean = false;
  game$: Observable<GameDetail> = of();

  constructor(
    private gamesService: GamesService,
    private route: ActivatedRoute,
    protected loadingService: LoadingService,
    private messagesService: MessagesService
  ) {
  }

  ngOnInit(): void {
    this.game$ = this.route.url.pipe(
      switchMap(params => {
        this.isCustom = params[0].path === 'custom';
        let res = this.isCustom ?
        this.gamesService.getCustomGameDetail(params[1].path)
        : this.gamesService.getGameDetail(params[0].path);
        res.subscribe(g => console.log(g));
        console.log(params[1].path);
        return this.isCustom ?
          this.gamesService.getCustomGameDetail(params[1].path)
          : this.gamesService.getGameDetail(params[0].path)
      })
    );
  }

  addToCollection() {
    this.game$.subscribe(res => {
      this.gamesService.addGameToCollection(res.id).subscribe(() => {
        this.messagesService.success('Jeu ajouté à la collection.');
      })
    })
  }

  removeFromCollection() {
    this.game$.subscribe(res => {
      this.gamesService.deleteGameFromCollection(res.id, this.isCustom).subscribe(() => {
        this.messagesService.success('Jeu retiré de la collection.');
      })
    })
  }

  formatDescription(description: string): Observable<string> {
    //TODO: The backend should encode correctly
    return of(description.replace(/&#10;&#10;/g, '\n\r\n\r'));
  }
}
