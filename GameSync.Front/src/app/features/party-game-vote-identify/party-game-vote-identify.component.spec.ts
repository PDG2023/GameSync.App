import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PartyGameVoteIdentifyComponent } from './party-game-vote-identify.component';
import {MatIconModule} from "@angular/material/icon";
import {LoginComponent} from "../login/login.component";
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {MatCardModule} from "@angular/material/card";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {ReactiveFormsModule} from "@angular/forms";
import {NoopAnimationsModule} from "@angular/platform-browser/animations";

describe('PartyGameVoteIdentifyComponent', () => {
  let component: PartyGameVoteIdentifyComponent;
  let fixture: ComponentFixture<PartyGameVoteIdentifyComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PartyGameVoteIdentifyComponent, LoginComponent],
      imports: [
        MatIconModule,
        HttpClientTestingModule,
        MatSnackBarModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule,
        NoopAnimationsModule
      ]
    });
    fixture = TestBed.createComponent(PartyGameVoteIdentifyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
