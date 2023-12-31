import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {
  BaseParty,
  Party,
  PartyDetail,
  PartyDetailRequest,
  PartyGameDetail,
  PartyGameRequest,
  VoteRequest
} from '../models/models';
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
    const url = req.id ? `/users/me/parties/${req.id}` : `/parties/${req.invitationToken}`;
    return this.httpClient.get<PartyDetail>(`${environment.apiUrl}${url}`);
  }

  getGameDetailFromParty(partyId: number, partyGameId: number, invitationToken: string) {
    const tokenUrl = invitationToken ? '?InvitationToken=' + invitationToken : '';
    return this.httpClient.get<PartyGameDetail>(`${environment.apiUrl}/users/me/parties/${partyId}/games/${partyGameId}${tokenUrl}`);
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

  addGameToParty(idParty: number, model: PartyGameRequest): Observable<PartyGameRequest> {
    return this.httpClient.post<PartyGameRequest>(`${environment.apiUrl}/users/me/parties/${idParty}/games`, model);
  }

  deleteGameFromParty(idParty: number, idPartyGame: number) {
    return this.httpClient.delete(`${environment.apiUrl}/users/me/parties/${idParty}/games/${idPartyGame}`);
  }

  getInvitationToken(idParty: number): Observable<string> {
    return this.httpClient.get<string>(`${environment.apiUrl}/users/me/parties/${idParty}/invitation-token`);
  }

  vote(req: VoteRequest) {
    const url = req.partyId ? `/users/me/parties/${req.partyId}` : `/parties/${req.invitationToken}`;
    return this.httpClient.put(`${environment.apiUrl}${url}/games/${req.partyGameId}/vote`, req.voteInfo);
  }
}
