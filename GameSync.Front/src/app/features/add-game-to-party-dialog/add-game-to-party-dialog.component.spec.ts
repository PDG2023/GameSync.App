import {ComponentFixture, TestBed} from '@angular/core/testing';

import {AddGameToPartyDialogComponent} from './add-game-to-party-dialog.component';

describe('AddGameToPartyDialogComponent', () => {
  let component: AddGameToPartyDialogComponent;
  let fixture: ComponentFixture<AddGameToPartyDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddGameToPartyDialogComponent]
    });
    fixture = TestBed.createComponent(AddGameToPartyDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
