import {ComponentFixture, TestBed} from '@angular/core/testing';

import {ChoseNameDialogComponent} from './chose-name-dialog.component';

describe('ChoseNameDialogComponent', () => {
  let component: ChoseNameDialogComponent;
  let fixture: ComponentFixture<ChoseNameDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ChoseNameDialogComponent]
    });
    fixture = TestBed.createComponent(ChoseNameDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
