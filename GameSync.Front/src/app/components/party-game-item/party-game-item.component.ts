import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {GameVoteInfo} from "../../models/models";

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

  totalVote: number = 0;
  voteRatio: number = 0;

  constructor() {
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
}
