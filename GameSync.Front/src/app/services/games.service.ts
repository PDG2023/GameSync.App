import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {GameDetailResult, GameCollectionItem, GameDetail, GameSearchRequest, GameSearchResult} from "../models/models";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class GamesService {

  constructor(
    private httpClient: HttpClient
  ) {
 }

  getGameDetail(gameId: string): Observable<GameDetailResult> {
    return this.httpClient.get<GameDetailResult>(`${environment.apiUrl}/games/${gameId}`);
  }

  getCustomGameDetail(gameId: number | string): Observable<GameDetail> {
    return this.httpClient.get<GameDetail>(`${environment.apiUrl}/users/me/games/${gameId}`);
  }

  getGames(req: GameSearchRequest): Observable<GameSearchResult> {
    return this.httpClient.get<GameSearchResult>(
      `${environment.apiUrl}/games/search?Query=${req.query}&PageSize=${req.pageSize}&Page=${req.page}`
    );
  }

  addGame(model: GameDetail) {
    return this.httpClient.post(`${environment.apiUrl}/users/me/games`, model);
  }

  editGame(model: GameDetail, id: number) {
    return this.httpClient.patch(`${environment.apiUrl}/users/me/games/${id}`, model);
  }

  addGameToCollection(id: number) {
    return this.httpClient.post(`${environment.apiUrl}/users/me/games/from-bgg/${id}`, {id});
  }

  deleteGameFromCollection(id: number, isCustomGame: boolean) {
    return this.httpClient.delete(`${environment.apiUrl}/users/me/games/${id}?isCustomGame=${isCustomGame}`);
  }

  getMyGames(): Observable<GameCollectionItem[]> {
    return this.httpClient.get<GameCollectionItem[]>(`${environment.apiUrl}/users/me/games`);
  }
}
