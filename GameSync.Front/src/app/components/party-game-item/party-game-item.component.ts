import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {GameVoteInfo, VoteInfo} from "../../models/models";
import {MatDialog} from "@angular/material/dialog";
import {WhoVotedDialogComponent} from "../../features/who-voted-dialog/who-voted-dialog.component";
import {AuthService} from "../../services/auth.service";
import {ChoseNameDialogComponent} from "../../features/chose-name-dialog/chose-name-dialog.component";

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

  totalVote: number = 0;
  voteRatio: number = 0;

  constructor(
    private dialog: MatDialog,
    private authService: AuthService
  ) {
  }

  ngOnInit() {
    if (this.gameVoteInfo) {
      this.totalVote = this.gameVoteInfo.countVotedYes + this.gameVoteInfo.countVotedNo;
      this.totalVote === 0 ?
        this.voteRatio = 0 :
        this.voteRatio = Math.round(this.gameVoteInfo.countVotedYes / this.totalVote);
    }
  }

  deleteMe() {
    this.gameRemovedFromParty.emit();
  }

  viewVoteDetail() {
    this.dialog.open(WhoVotedDialogComponent, {data: this.gameVoteInfo});
  }

  vote(votedYes: boolean) {
    this.authService.connectedUserSubject$.subscribe(user => {
      if (user) {
        this.voted.emit({
          userName: user.userName,
          voteYes: votedYes
        });
      } else {
        this.dialog.open(ChoseNameDialogComponent).afterClosed()
          .subscribe(userName => {
            if (userName) {
              this.voted.emit({
                userName: userName,
                voteYes: votedYes
              })
            }
          })
      }
    })
  }
}
