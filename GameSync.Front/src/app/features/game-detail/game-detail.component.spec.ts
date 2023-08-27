import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameDetailComponent } from './game-detail.component';
import {MatIconModule} from "@angular/material/icon";
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {RouterTestingModule} from "@angular/router/testing";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";

describe('GameDetailComponent', () => {
  let component: GameDetailComponent;
  let fixture: ComponentFixture<GameDetailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GameDetailComponent],
      imports: [
        MatIconModule,
        HttpClientTestingModule,
        RouterTestingModule,
        MatProgressSpinnerModule
      ]
    });
    fixture = TestBed.createComponent(GameDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
