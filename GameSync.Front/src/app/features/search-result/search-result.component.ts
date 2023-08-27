import {Component, OnInit} from '@angular/core';
import {GamesService} from "../../services/games.service";
import {Observable, of} from "rxjs";
import {GameSearchRequest, GameSearchResult} from "../../models/models";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.scss']
})
export class SearchResultComponent implements OnInit {
  searchRequest: GameSearchRequest = {
    query: '',
    pageSize: 0,
    page: 0
  };
  searchResult$: Observable<GameSearchResult> = of();

  constructor(
    private gamesService: GamesService,
    private route: ActivatedRoute
  ) {
    this.route.queryParams
      .subscribe(params => this.searchRequest = {
        query: params['Query'],
        pageSize: params['PageSize'],
        page: params['Page']
      })
  }

  ngOnInit(): void {
    this.searchResult$ = this.gamesService.getGames(this.searchRequest);
  }
}
