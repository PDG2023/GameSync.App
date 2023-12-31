import {ComponentFixture, TestBed} from '@angular/core/testing';

import {PartyItemComponent} from './party-item.component';
import {MatCardModule} from "@angular/material/card";
import {MatIconModule} from "@angular/material/icon";
import {RouterTestingModule} from "@angular/router/testing";
import {MatDialogModule} from "@angular/material/dialog";
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('PartyItemComponent', () => {
  let component: PartyItemComponent;
  let fixture: ComponentFixture<PartyItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PartyItemComponent],
      imports: [
        MatCardModule,
        MatIconModule,
        RouterTestingModule,
        MatDialogModule,
        MatSnackBarModule,
        HttpClientTestingModule
      ]
    });
    fixture = TestBed.createComponent(PartyItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
