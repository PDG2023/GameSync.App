import {Component, OnInit} from '@angular/core';
import {PartiesService} from "../../services/parties.service";
import {ActivatedRoute} from "@angular/router";
import {Observable, of, tap} from "rxjs";
import {GameCollectionItem, PartyDetail} from "../../models/models";
import {FormBuilder, FormGroup} from "@angular/forms";
import {MessagesService} from "../../services/messages.service";
import {MatDialog} from "@angular/material/dialog";
import {AddGameToPartyDialogComponent} from "../add-game-to-party-dialog/add-game-to-party-dialog.component";

@Component({
  selector: 'app-party-detail',
  templateUrl: './party-detail.component.html',
  styleUrls: ['./party-detail.component.scss']
})
export class PartyDetailComponent implements OnInit{
  partyDetail$: Observable<PartyDetail> = of();
  partyDetailForm: FormGroup = this.fb.group({
    location: null,
    name: null,
    dateTime: null
  });

  private readonly partyId = this.route.snapshot.params['id'];

  constructor(
    private partiesService: PartiesService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private messagesService: MessagesService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.partyDetail$ = this.partiesService.getPartyDetail({
      id: this.partyId,
      invitationToken: this.route.snapshot.queryParams['']
    }).pipe(
      tap(partyDetail => {
        this.partyDetailForm.patchValue({
          ...partyDetail
        })
      })
    );
  }


  editParty() {
    this.partiesService.editParty({...this.partyDetailForm.value}, this.partyId)
      .subscribe(() => this.messagesService.success('Modifications enregistrées.'));
  }

  openAddGameToPartyDialog() {
    this.dialog.open(AddGameToPartyDialogComponent).afterClosed()
      .subscribe((selectedItems: GameCollectionItem[]) => {
        selectedItems.forEach(item => {
          this.partiesService.addGameToParty(this.partyId, item.id)
            .subscribe(() => this.messagesService.success('Jeu ajouté à la soirée'));
        });
      });
  }
}
