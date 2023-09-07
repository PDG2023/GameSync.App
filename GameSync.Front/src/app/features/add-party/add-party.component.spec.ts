import {ComponentFixture, TestBed} from '@angular/core/testing';

import {AddPartyComponent} from './add-party.component';
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {MatSnackBarModule} from "@angular/material/snack-bar";

describe('AddPartyComponent', () => {
  let component: AddPartyComponent;
  let fixture: ComponentFixture<AddPartyComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddPartyComponent],
      imports: [
        HttpClientTestingModule,
        MatSnackBarModule
      ]
    });
    fixture = TestBed.createComponent(AddPartyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
