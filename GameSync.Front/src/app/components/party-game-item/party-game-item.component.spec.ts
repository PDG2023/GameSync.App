import {ComponentFixture, TestBed} from '@angular/core/testing';

import {PartyGameItemComponent} from './party-game-item.component';
import {MatCardModule} from "@angular/material/card";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {MatIconModule} from "@angular/material/icon";
import {RouterTestingModule} from "@angular/router/testing";
import {MatDialogModule} from "@angular/material/dialog";
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('PartyGameItemComponent', () => {
  let component: PartyGameItemComponent;
  let fixture: ComponentFixture<PartyGameItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PartyGameItemComponent],
      imports: [
        MatCardModule,
        MatProgressBarModule,
        MatIconModule,
        RouterTestingModule,
        MatDialogModule,
        HttpClientTestingModule
      ]
    });
    fixture = TestBed.createComponent(PartyGameItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
