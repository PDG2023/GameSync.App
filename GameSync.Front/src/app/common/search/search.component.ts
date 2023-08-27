import {Component, OnInit} from '@angular/core';
import {debounceTime, map, Observable, of, startWith, switchMap} from "rxjs";
import {FormControl} from "@angular/forms";
import {GamesService} from "../../services/games.service";
import {GameList, GameSearchResult, GameSearchResultItem} from "../../models/models";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {
  private readonly PAGE_SIZE_PREVIEW = 10;
  private readonly PAGE_NUMBER_PREVIEW = 0;
  autoComplete = new FormControl('');
  gamesSearchedPreview$: Observable<GameSearchResultItem[]> = of([]);

  constructor(
    private gamesService: GamesService
  ) {
  }

  ngOnInit() {
    this.gamesSearchedPreview$ = this.autoComplete.valueChanges.pipe(
      startWith(''),
      debounceTime(300),
      switchMap(value => {
          if (value === '') {
            return of({items: []})
          }
          return this.gamesService.getGames({
            query: value!,
            pageSize: this.PAGE_SIZE_PREVIEW,
            page: this.PAGE_NUMBER_PREVIEW
          });
        }
      ),
      map(result => result.items)
    );
  }
}
