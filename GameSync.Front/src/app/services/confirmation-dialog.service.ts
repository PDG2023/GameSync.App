import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {map, Observable} from 'rxjs';
import {DialogYesNoComponent} from "../common/dialog-yes-no/dialog-yes-no.component";

@Injectable({
  providedIn: 'root'
})
export class ConfirmationDialogService {

  constructor(
    private matDialog: MatDialog
  ) { }

  askConfirmation(message: string): Observable<any> {
    return this.matDialog.open(DialogYesNoComponent, {
      closeOnNavigation: true,
      data: {
        message
      }
    }).afterClosed().pipe(
      map(res => {
        return res === true;
      })
    );
  }

}
