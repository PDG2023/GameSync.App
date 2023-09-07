import {ComponentFixture, TestBed} from '@angular/core/testing';

import {PartyDetailComponent} from './party-detail.component';
import {MatIconModule} from "@angular/material/icon";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {NoopAnimationsModule} from "@angular/platform-browser/animations";
import {PartyGameItemComponent} from "../../components/party-game-item/party-game-item.component";
import {MatCardModule} from "@angular/material/card";
import {RouterTestingModule} from "@angular/router/testing";
import {MatButtonModule} from "@angular/material/button";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {MatDialogModule} from "@angular/material/dialog";

describe('PartyDetailComponent', () => {
  let component: PartyDetailComponent;
  let fixture: ComponentFixture<PartyDetailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PartyDetailComponent, PartyGameItemComponent],
      imports: [
        MatIconModule,
        MatFormFieldModule,
        MatInputModule,
        NoopAnimationsModule,
        MatCardModule,
        RouterTestingModule,
        MatButtonModule,
        MatProgressBarModule,
        HttpClientTestingModule,
        MatSnackBarModule,
        MatDialogModule
      ]
    });
    fixture = TestBed.createComponent(PartyDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
