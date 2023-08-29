import { Component } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {LoadingService} from "../../services/loading.service";
import {PartiesService} from "../../services/parties.service";
import {Router} from "@angular/router";
import {MessagesService} from "../../services/messages.service";

@Component({
  selector: 'app-add-party',
  templateUrl: './add-party.component.html',
  styleUrls: ['./add-party.component.scss']
})
export class AddPartyComponent {
  partyForm: FormGroup;

  constructor(
    fb: FormBuilder,
    private loadingService: LoadingService,
    private partiesService: PartiesService,
    private messagesService: MessagesService,
    private router: Router
  ) {
    this.partyForm = fb.group({
      name: [null, Validators.required],
      location: [null, Validators.required],
      dateTime: [null, Validators.required]
    });
  }


  submit() {
    this.partiesService.addParty({
      ...this.partyForm.value
    }).subscribe(res => {
      this.messagesService.success('BRAVO');
      this.router.navigateByUrl('/parties');
    })
  }
}
