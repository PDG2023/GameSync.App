import {ChangeDetectionStrategy, Component, OnInit} from '@angular/core';
import {delay, finalize, map, Observable, of, switchMap, take, tap} from 'rxjs';
import {GameDetail} from "../../models/models";
import {GamesService} from "../../services/games.service";
import {ActivatedRoute} from "@angular/router";
import {LoadingService} from "../../services/loading.service";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-game-detail',
  templateUrl: './game-detail.component.html',
  styleUrls: ['./game-detail.component.scss']
})
export class GameDetailComponent implements OnInit {

  game$: Observable<GameDetail> = of();

  constructor(
    private gamesService: GamesService,
    private route: ActivatedRoute,
    protected loadingService: LoadingService
  ) {
  }

  ngOnInit(): void {
    this.game$ = this.route.params.pipe(
      switchMap(params => this.gamesService.getGameDetail(params['id']))
    );
  }
}
