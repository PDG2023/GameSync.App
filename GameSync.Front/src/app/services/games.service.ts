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
  ) { }

  getMyGames() : Observable<GameList[]> {
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
