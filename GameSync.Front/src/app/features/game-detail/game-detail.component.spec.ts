import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameDetailComponent } from './game-detail.component';
import {MatIconModule} from "@angular/material/icon";

describe('GameDetailComponent', () => {
  let component: GameDetailComponent;
  let fixture: ComponentFixture<GameDetailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GameDetailComponent],
      imports: [MatIconModule]
    });
    fixture = TestBed.createComponent(GameDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
