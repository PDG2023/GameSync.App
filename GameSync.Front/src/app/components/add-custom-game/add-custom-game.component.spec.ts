import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCustomGameComponent } from './add-custom-game.component';

describe('AddCustomGameComponent', () => {
  let component: AddCustomGameComponent;
  let fixture: ComponentFixture<AddCustomGameComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddCustomGameComponent]
    });
    fixture = TestBed.createComponent(AddCustomGameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
