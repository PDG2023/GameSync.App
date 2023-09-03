import {ComponentFixture, TestBed} from '@angular/core/testing';

import {CollectionComponent} from './collection.component';
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {MatMenuModule} from "@angular/material/menu";
import {MatIconModule} from "@angular/material/icon";
import {GameItemComponent} from "../../components/game-item/game-item.component";
import {MatDialogModule} from "@angular/material/dialog";
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {MatCardModule} from "@angular/material/card";

describe('CollectionComponent', () => {
  let component: CollectionComponent;
  let fixture: ComponentFixture<CollectionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CollectionComponent, GameItemComponent],
      imports: [
        HttpClientTestingModule,
        MatMenuModule,
        MatIconModule,
        MatDialogModule,
        MatSnackBarModule,
        MatCardModule
      ]
    });
    fixture = TestBed.createComponent(CollectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
