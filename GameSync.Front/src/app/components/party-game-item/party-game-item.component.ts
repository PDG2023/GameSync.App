import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {GameVoteInfo, VoteInfo} from "../../models/models";
import {MatDialog} from "@angular/material/dialog";
import {WhoVotedDialogComponent} from "../../features/who-voted-dialog/who-voted-dialog.component";
import {AuthService} from "../../services/auth.service";
import {ChoseNameDialogComponent} from "../../features/chose-name-dialog/chose-name-dialog.component";
import {take} from "rxjs";
import {PartiesService} from "../../services/parties.service";

@Component({
  selector: 'app-party-game-item',
  templateUrl: './party-game-item.component.html',
  styleUrls: ['./party-game-item.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PartyGameItemComponent implements OnInit {
  @Input() gameVoteInfo?: GameVoteInfo;
  @Input() readonly = false;

  @Output() gameRemovedFromParty = new EventEmitter();
  @Output() voted = new EventEmitter<VoteInfo>();
  @Output() requestDetail = new EventEmitter();

  totalVote: number = 0;
  voteRatio: number = 0;

  constructor(
    private dialog: MatDialog,
    private authService: AuthService,
    private partiesService: PartiesService
  ) {
  }

  ngOnInit() {
    if (this.gameVoteInfo) {
      this.totalVote = this.gameVoteInfo.countVotedYes + this.gameVoteInfo.countVotedNo;
      this.totalVote === 0 ?
        this.voteRatio = 0 :
        this.voteRatio = this.gameVoteInfo.countVotedYes / this.totalVote * 100;
    }
  }

  deleteMe() {
    this.gameRemovedFromParty.emit();
  }

  viewVoteDetail() {
    this.dialog.open(WhoVotedDialogComponent, {data: this.gameVoteInfo});
  }

  vote(votedYes: boolean) {
    this.authService.connectedUserSubject$.pipe(take(1))
      .subscribe(user => {
        if (user) {
          this.sendVote(user.userName, votedYes);
        } else {
          this.guestVote(votedYes);
        }
      })
  }

  private guestVote(votedYes: boolean) {
    this.authService.guestUserSubject$.pipe(take(1))
      .subscribe(guest => {
        if (!guest) {
          this.dialog.open(ChoseNameDialogComponent).afterClosed()
            .subscribe(userName => {
              if (userName) {
                this.authService.setGuestUser(userName);
              }
            });
        }
        this.sendVote(guest!, votedYes);
      })
  }

  private sendVote(userName: string, votedYes: boolean) {
    this.voted.emit({
      userName: userName,
      voteYes: votedYes
    })
  }

  toGameDetail() {
    this.requestDetail.emit();
  }
}
