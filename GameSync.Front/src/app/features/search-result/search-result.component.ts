import {ChangeDetectionStrategy, Component, OnInit, ViewChild} from '@angular/core';
import {GamesService} from "../../services/games.service";
import {Observable, of, skip, switchMap, tap} from "rxjs";
import {GameSearchResult} from "../../models/models";
import {ActivatedRoute, Params, Router} from "@angular/router";
import {LoadingService} from "../../services/loading.service";
import {MatPaginator, PageEvent} from "@angular/material/paginator";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.scss']
})
export class SearchResultComponent implements OnInit {
  searchResult$: Observable<GameSearchResult> = of();

  pageIndex: number = 0;
  pageSize: number = 0;

  constructor(
    private gamesService: GamesService,
    private router: Router,
    protected route: ActivatedRoute,
    protected loadingService: LoadingService
  ) {}

  ngOnInit(): void {
    this.searchResult$ = this.route.queryParams
      .pipe(
        switchMap(params => this.gamesService.getGames({
          query: params['Query'],
          pageSize: params['PageSize'],
          page: params['Page']
        })),
        tap(result => {
          const queryParams = this.route.snapshot.queryParams;
          this.pageIndex = queryParams['Page']
          this.pageSize = queryParams['PageSize']
        })
      )
  }

  updatePage(pageEvent: PageEvent) {
    const queryParams: Params = {
      Query: this.route.snapshot.queryParams['Query'],
      PageSize: pageEvent.pageSize,
      Page: pageEvent.pageIndex
    }
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams,
      queryParamsHandling: 'merge'
    });
  }
}
