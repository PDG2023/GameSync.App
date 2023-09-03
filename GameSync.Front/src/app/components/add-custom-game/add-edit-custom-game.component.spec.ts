import {ComponentFixture, TestBed} from '@angular/core/testing';

import {AddEditCustomGameComponent} from './add-edit-custom-game.component';
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {RouterTestingModule} from "@angular/router/testing";
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {ReactiveFormsModule} from "@angular/forms";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {NoopAnimationsModule} from "@angular/platform-browser/animations";

describe('AddCustomGameComponent', () => {
  let component: AddEditCustomGameComponent;
  let fixture: ComponentFixture<AddEditCustomGameComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddEditCustomGameComponent],
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        MatSnackBarModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        NoopAnimationsModule
      ]
    });
    fixture = TestBed.createComponent(AddEditCustomGameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
