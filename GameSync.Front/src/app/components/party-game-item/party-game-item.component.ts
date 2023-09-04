import {Component, Input, OnInit} from '@angular/core';
import {GameVoteInfo} from "../../models/models";

@Component({
  selector: 'app-party-game-item',
  templateUrl: './party-game-item.component.html',
  styleUrls: ['./party-game-item.component.scss']
})
export class PartyGameItemComponent implements OnInit {
  @Input() gameVoteInfo?: GameVoteInfo;

  totalVote: number = 0;
  voteRatio: number = 0;

  constructor() {
  }

  ngOnInit() {
    if (this.gameVoteInfo) {
      this.totalVote = this.gameVoteInfo.countVotedYes + this.gameVoteInfo.countVotedNo;
      this.voteRatio = Math.round(this.gameVoteInfo.countVotedYes / this.totalVote);
    }
  }
}
