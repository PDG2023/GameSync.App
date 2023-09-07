import {Component, Input, OnInit} from '@angular/core';
import {PartiesService} from "../../services/parties.service";
import {ActivatedRoute, Router} from "@angular/router";
import {Observable, of, tap} from "rxjs";
import {GameCollectionItem, PartyDetail, PartyGameRequest, PartyGameRequestItem, VoteInfo} from "../../models/models";
import {FormBuilder, FormGroup} from "@angular/forms";
import {MessagesService} from "../../services/messages.service";
import {MatDialog} from "@angular/material/dialog";
import {AddGameToPartyDialogComponent} from "../add-game-to-party-dialog/add-game-to-party-dialog.component";
import {Clipboard} from "@angular/cdk/clipboard";
import {ConfirmationDialogService} from "../../services/confirmation-dialog.service";

@Component({
  selector: 'app-party-detail',
  templateUrl: './party-detail.component.html',
  styleUrls: ['./party-detail.component.scss']
})
export class PartyDetailComponent implements OnInit {
  @Input() readonly = false;

  partyDetail$: Observable<PartyDetail> = of();
  partyDetailForm: FormGroup = this.fb.group({
    location: [{value: null, disabled: this.readonly}],
    name: [{value: null, disabled: this.readonly}],
    dateTime: [{value: null, disabled: this.readonly}]
  });

  isOwner = false;
  protected readonly idParty = this.route.snapshot.params['id'];

  constructor(
    private partiesService: PartiesService,
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private messagesService: MessagesService,
    private clipboard: Clipboard,
    private confirmationDialogService: ConfirmationDialogService,
    public dialog: MatDialog
  ) {
  }

  ngOnInit(): void {
    this.refresh();
  }

  refresh() {
    this.partyDetail$ = this.partiesService.getPartyDetail({
      id: this.idParty ?? '',
      invitationToken: this.route.snapshot.params['token']
    }).pipe(
      tap(partyDetail => {
        this.isOwner = partyDetail.isOwner
        this.partyDetailForm.patchValue({
          ...partyDetail
        });
        if (!partyDetail.isOwner) {
          this.partyDetailForm.disable();
        }
      })
    );
  }


  editParty() {
    this.partiesService.editParty({...this.partyDetailForm.value}, this.idParty)
      .subscribe(() => {
        this.messagesService.success('Modifications enregistrées.');
        this.refresh();
      });
  }

  openAddGameToPartyDialog() {
    this.dialog.open(AddGameToPartyDialogComponent).afterClosed()
      .subscribe((selectedItems: GameCollectionItem[]) => {
        if (selectedItems) {
          const req: PartyGameRequest = {
            games: selectedItems.map(item => this.toPartyGameReq(item))
          }
          this.partiesService.addGameToParty(this.idParty, req)
            .subscribe(() => {
              this.messagesService.success('Jeu(x) ajouté(s) à la soirée');
              this.refresh()
            });
        }
      });
  }

  getInvitationToken(idParty: number) {
    this.messagesService.success("Lien d'invitation copié dans le presse-papier")
    this.partiesService.getInvitationToken(idParty).subscribe(
      token => this.clipboard.copy(token)
    );
  }

  private toPartyGameReq(item: GameCollectionItem): PartyGameRequestItem {
    return {
      id: item.id,
      isCustom: item.isCustom
    }
  }

  deleteMe(idGame: number) {
    this.confirmationDialogService
      .askConfirmation('Voulez-vous supprimer ce jeu de la soirée ?')
      .subscribe(res => {
        if (res) {
          this.partiesService.deleteGameFromParty(this.idParty, idGame).subscribe(() => {
            this.messagesService.success('Jeu supprimé de la soirée.');
            this.refresh();
          });
        }
      });
  }

  sendVote(voteInfo: VoteInfo, partyGameId: number) {
    this.partiesService.vote({
      partyId: this.idParty ?? '',
      invitationToken: this.route.snapshot.params['token'],
      partyGameId: partyGameId,
      voteInfo: voteInfo
    }).subscribe(res => {
      this.refresh();
    });
  }

  toGameDetail(partyGameId: number, partyId: number) {
    const token = this.route.snapshot.params['token'] ?? '';
    this.partiesService.getGameDetailFromParty(partyId, partyGameId, token).subscribe(gameDetail => {
      gameDetail.isCustom ? this.router.navigateByUrl('games/custom' + gameDetail.game.id)
        : this.router.navigateByUrl('games/' + gameDetail.game.id);
    })
  }
}
