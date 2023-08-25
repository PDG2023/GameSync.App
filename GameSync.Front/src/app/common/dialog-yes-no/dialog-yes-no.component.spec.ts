import {ComponentFixture, TestBed} from '@angular/core/testing';

import {DialogYesNoComponent} from './dialog-yes-no.component';
import {MAT_DIALOG_DATA, MatDialogModule, MatDialogRef} from "@angular/material/dialog";

describe('DialogYesNoComponent', () => {
  let component: DialogYesNoComponent;
  let fixture: ComponentFixture<DialogYesNoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DialogYesNoComponent],
      providers: [
        {provide: MAT_DIALOG_DATA, useValue: {}},
        {provide: MatDialogRef, useValue: {}}
      ],
      imports: [
        MatDialogModule
      ]
    });
    fixture = TestBed.createComponent(DialogYesNoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
