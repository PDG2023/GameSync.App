import {Component} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-chose-name-dialog',
  templateUrl: './chose-name-dialog.component.html',
  styleUrls: ['./chose-name-dialog.component.scss']
})
export class ChoseNameDialogComponent {
  nameForm: FormGroup = this.fb.group({
    userName: [null, Validators.required],
  })

  constructor(
    private fb: FormBuilder
  ) {
  }
}
