import { Injectable } from '@angular/core';
import {HubConnection, HubConnectionBuilder,} from "@microsoft/signalr";
import {environment} from "../../environments/environment";
import {GameVoteInfo, PartyDetail, PartyDetailRequest, PartyGameRequest} from "../models/models";
import {from, Observable, Subject} from "rxjs";
import {PartiesService} from "./parties.service";
import {PartyDetailComponent} from "../features/party-detail/party-detail.component";

@Injectable({
  providedIn: 'root'
})
export class PartyVoteService {

  private hubConnection?: HubConnection;
  private voteFeed$: Subject<any> = new Subject<any>();

  public get VoteFeed(): Observable<any> {
    return this.voteFeed$.asObservable();
  }

  public connectToPartyHub(partyId: number) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/parties/vote`)
      .build();

    this.hubConnection!.on("new-vote", (data: any) => {
      this.voteFeed$.next(data);
    });

    return this.hubConnection
      .start()
      .then(() => this.hubConnection?.invoke("RegisterForVoteFeed", partyId));
  }


  constructor(private partyService: PartiesService) { }
}
