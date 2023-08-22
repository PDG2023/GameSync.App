import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CollectionItemComponent } from './collection-item.component';
import {MatDialogModule, MatDialogRef} from "@angular/material/dialog";
import {MatSnackBarModule} from "@angular/material/snack-bar";

describe('CollectionItemComponent', () => {
  let component: CollectionItemComponent;
  let fixture: ComponentFixture<CollectionItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CollectionItemComponent],
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
    fixture = TestBed.createComponent(CollectionItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
