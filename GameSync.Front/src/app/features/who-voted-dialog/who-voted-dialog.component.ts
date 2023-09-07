import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {GameVoteInfo} from "../../models/models";

@Component({
  selector: 'app-who-voted-dialog',
  templateUrl: './who-voted-dialog.component.html',
  styleUrls: ['./who-voted-dialog.component.scss']
})
export class WhoVotedDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: GameVoteInfo) {
  }

}
