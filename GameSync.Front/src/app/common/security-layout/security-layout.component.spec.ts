import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecurityLayoutComponent } from './security-layout.component';
import {RouterTestingModule} from "@angular/router/testing";

describe('SecurityLayoutComponent', () => {
  let component: SecurityLayoutComponent;
  let fixture: ComponentFixture<SecurityLayoutComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SecurityLayoutComponent],
      imports: [
        RouterTestingModule
      ]
    });
    fixture = TestBed.createComponent(SecurityLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
