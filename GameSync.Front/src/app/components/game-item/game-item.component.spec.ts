import {ComponentFixture, TestBed} from '@angular/core/testing';

import {GameItemComponent} from './game-item.component';
import {MatDialogModule, MatDialogRef} from "@angular/material/dialog";
import {MatSnackBarModule} from "@angular/material/snack-bar";

describe('CollectionItemComponent', () => {
  let component: GameItemComponent;
  let fixture: ComponentFixture<GameItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GameItemComponent],
      imports: [
        MatDialogModule,
        MatSnackBarModule
      ],
      providers: [
        {
          provide: MatDialogRef,
          useValue: {}
        }
      ]
    });
    fixture = TestBed.createComponent(GameItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
