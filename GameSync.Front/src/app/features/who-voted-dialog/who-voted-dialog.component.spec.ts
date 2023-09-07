import {ComponentFixture, TestBed} from '@angular/core/testing';

import {WhoVotedDialogComponent} from './who-voted-dialog.component';
import {MAT_DIALOG_DATA, MatDialogModule, MatDialogRef} from "@angular/material/dialog";

describe('WhoVotedDialogComponent', () => {
  let component: WhoVotedDialogComponent;
  let fixture: ComponentFixture<WhoVotedDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WhoVotedDialogComponent],
      imports: [
        MatDialogModule
      ],
      providers: [
        {provide: MAT_DIALOG_DATA, useValue: {}},
        {provide: MatDialogRef, useValue: {}}
      ]
    });
    fixture = TestBed.createComponent(WhoVotedDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
