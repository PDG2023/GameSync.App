import {ComponentFixture, TestBed} from '@angular/core/testing';

import {CollectionGameItemComponent} from './collection-game-item.component';
import {MatDialogModule} from "@angular/material/dialog";
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('CollectionGameItemComponent', () => {
  let component: CollectionGameItemComponent;
  let fixture: ComponentFixture<CollectionGameItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CollectionGameItemComponent],
      imports: [
        MatDialogModule,
        MatSnackBarModule,
        HttpClientTestingModule
      ]
    });
    fixture = TestBed.createComponent(CollectionGameItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
