import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Game, GameList } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class GamesService {

  constructor(
    private httpClient: HttpClient
  ) {
 }

  getGameDetail(gameId: string): Observable<GameDetail> {
    return this.httpClient.get<GameDetail>(`${environment.apiUrl}/games/${gameId}`)
  }

  getGames(req: GameSearchRequest): Observable<GameSearchResult> {
    return this.httpClient.get<GameSearchResult>(
      `${environment.apiUrl}/games/search?Query=${req.query}&PageSize=${req.pageSize}&Page=${req.page}`
    );
  }

  getMyGames(): Observable<GameCollection[]> {
    return of([
      {
        id: 0,
        name: "string",
        yearPublished: 2000,
        minPlayer: 0,
        maxPlayer: 0,
        minAge: 0,
        durationMinute: 0,
        description: "string",
        userId: "string",
        imageUrl: "string"
      },
      {
        id: 0,
        name: "string",
        yearPublished: 2000,
        minPlayer: 0,
        maxPlayer: 0,
        minAge: 0,
        durationMinute: 0,
        description: "string",
        userId: "string",
        imageUrl: "string"
      },
      {
        id: 0,
        name: "string",
        yearPublished: 2000,
        minPlayer: 0,
        maxPlayer: 0,
        minAge: 0,
        durationMinute: 0,
        description: "string",
        userId: "string",
        imageUrl: "string"
      },
      {
        id: 0,
        name: "string",
        yearPublished: 2000,
        minPlayer: 0,
        maxPlayer: 0,
        minAge: 0,
        durationMinute: 0,
        description: "string",
        userId: "string",
        imageUrl: "string"
      },
      {
        id: 0,
        name: "string",
        yearPublished: 2000,
        minPlayer: 0,
        maxPlayer: 0,
        minAge: 0,
        durationMinute: 0,
        description: "string",
        userId: "string",
        imageUrl: "string"
      },
    ]);
  }
}
