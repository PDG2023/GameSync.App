import {ComponentFixture, TestBed} from '@angular/core/testing';

import {AddEditCustomGameComponent} from './add-edit-custom-game.component';

describe('AddCustomGameComponent', () => {
  let component: AddEditCustomGameComponent;
  let fixture: ComponentFixture<AddEditCustomGameComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddEditCustomGameComponent]
    });
    fixture = TestBed.createComponent(AddEditCustomGameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
