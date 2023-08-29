import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import {AddGame, GameCollection, GameDetail, GameSearchRequest, GameSearchResult} from "../models/models";
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

  getGames(req: GameSearchRequest): Observable<GameSearchResult> {
    return this.httpClient.get<GameSearchResult>(
      `${environment.apiUrl}/games/search?Query=${req.query}&PageSize=${req.pageSize}&Page=${req.page}`
    );
  }

  addGame(model: AddGame) {
    return this.httpClient.post(`${environment.apiUrl}/users/me/games`, model);
  }

  addGameToCollection(id: number) {
    return this.httpClient.post(`${environment.apiUrl}/users/me/games/from-bgg/${id}`, {id});
  }

  deleteGameFromCollection(id: number) {
    return this.httpClient.delete(`${environment.apiUrl}/users/me/games/${id}`);
  }

  getMyGames(): Observable<GameCollection[]> {
    return this.httpClient.get<GameCollection[]>(`${environment.apiUrl}/users/me/games`);
  }
}
