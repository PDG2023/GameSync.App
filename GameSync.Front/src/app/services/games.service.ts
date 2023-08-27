import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable, of} from 'rxjs';
import {Game, GameList, GameSearchRequest, GameSearchResult, GameSearchResultItem} from '../models/models';
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class GamesService {

  constructor(
    private httpClient: HttpClient
  ) {
  }

  getGames(req: GameSearchRequest): Observable<GameSearchResult> {
    return this.httpClient.get<GameSearchResult>(
      `${environment.apiUrl}/games/search?Query=${req.query}&PageSize=${req.pageSize}&Page=${req.page}`
    );
  }

  getMyGames(): Observable<GameList[]> {
    return of([
      {
        id: 0,
        name: "string",
        minPlayer: 0,
        maxPlayer: 0,
        minAge: 0,
        durationMinute: 0,
        description: "string",
        userId: "string"
      },
      {
        id: 0,
        name: "string",
        minPlayer: 0,
        maxPlayer: 0,
        minAge: 0,
        durationMinute: 0,
        description: "string",
        userId: "string"
      },
      {
        id: 0,
        name: "string",
        minPlayer: 0,
        maxPlayer: 0,
        minAge: 0,
        durationMinute: 0,
        description: "string",
        userId: "string"
      },
      {
        id: 0,
        name: "string",
        minPlayer: 0,
        maxPlayer: 0,
        minAge: 0,
        durationMinute: 0,
        description: "string",
        userId: "string"
      },
      {
        id: 0,
        name: "string",
        minPlayer: 0,
        maxPlayer: 0,
        minAge: 0,
        durationMinute: 0,
        description: "string",
        userId: "string"
      },
    ]);
  }
}
