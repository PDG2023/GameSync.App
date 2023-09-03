import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmMailComponent } from './confirm-mail.component';
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {RouterTestingModule} from "@angular/router/testing";
import {MatSnackBarModule} from "@angular/material/snack-bar";

describe('ConfirmMailComponent', () => {
  let component: ConfirmMailComponent;
  let fixture: ComponentFixture<ConfirmMailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ConfirmMailComponent],
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        MatSnackBarModule
    ]
    });
    fixture = TestBed.createComponent(ConfirmMailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
