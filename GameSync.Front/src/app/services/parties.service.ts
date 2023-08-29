import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {BaseParty, Party} from '../models/models';
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class PartiesService {

  constructor(
    private httpClient: HttpClient
  ) { }

  getMyParties() {
    return this.httpClient.get<Party[]>(`${environment.apiUrl}/users/me/parties`);
  }

  addParty(model: BaseParty) {
    return this.httpClient.post(`${environment.apiUrl}/users/me/parties`, model);
  }

  deleteParty(id: number) {
    return this.httpClient.delete(`${environment.apiUrl}/users/me/parties/${id}`);
  }
}
