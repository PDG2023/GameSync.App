import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {BaseParty, Party, PartyDetail, PartyDetailRequest} from '../models/models';
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class PartiesService {

  constructor(
    private httpClient: HttpClient
  ) {
  }

  getMyParties() {
    return this.httpClient.get<Party[]>(`${environment.apiUrl}/users/me/parties`);
  }

  getPartyDetail(req: PartyDetailRequest): Observable<PartyDetail> {
    const queryParams = req.invitationToken ? '?InvitaionToken=' + req.invitationToken : '';
    return this.httpClient.get<PartyDetail>(`${environment.apiUrl}/users/me/parties/${req.id}${queryParams}`);
  }

  addParty(req: BaseParty) {
    return this.httpClient.post(`${environment.apiUrl}/users/me/parties`, req);
  }

  editParty(req: BaseParty, id: number) {
    return this.httpClient.patch(`${environment.apiUrl}/users/me/parties/${id}`, req);
  }

  deleteParty(id: number) {
    return this.httpClient.delete(`${environment.apiUrl}/users/me/parties/${id}`);
  }

  addGameToParty(idParty: number, idGame: number) {
    console.log("idparty",idParty)
    console.log("idgame",idGame)
    return this.httpClient.put(`${environment.apiUrl}/users/me/parties/${idParty}/games/${idGame}`, null);
  }
}
