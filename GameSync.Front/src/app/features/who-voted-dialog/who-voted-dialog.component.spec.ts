import {ComponentFixture, TestBed} from '@angular/core/testing';

import {WhoVotedDialogComponent} from './who-voted-dialog.component';

describe('WhoVotedDialogComponent', () => {
  let component: WhoVotedDialogComponent;
  let fixture: ComponentFixture<WhoVotedDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WhoVotedDialogComponent]
    });
    fixture = TestBed.createComponent(WhoVotedDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
