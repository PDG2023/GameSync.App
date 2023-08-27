import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable, of} from 'rxjs';
import {Game, GameDetail, GameList} from '../models/models';
import {environment} from "../../environments/environment";

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
        userId: "string",
        imageUrl: "string"
      },
      {
        id: 0,
        name: "string",
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
