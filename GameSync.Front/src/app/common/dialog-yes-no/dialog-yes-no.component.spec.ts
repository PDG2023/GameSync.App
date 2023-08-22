import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogYesNoComponent } from './dialog-yes-no.component';

describe('DialogYesNoComponent', () => {
  let component: DialogYesNoComponent;
  let fixture: ComponentFixture<DialogYesNoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DialogYesNoComponent]
    });
    fixture = TestBed.createComponent(DialogYesNoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
