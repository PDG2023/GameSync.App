import {ComponentFixture, TestBed} from '@angular/core/testing';
import {SideNavComponent} from './side-nav.component';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatIconModule} from '@angular/material/icon';
import {NoopAnimationsModule} from '@angular/platform-browser/animations';
import {MatListModule} from '@angular/material/list';
import {MediaMatcher} from '@angular/cdk/layout';
import {ChangeDetectorRef} from '@angular/core';
import {RouterTestingModule} from "@angular/router/testing";

describe('SideNavComponent', () => {
  let component: SideNavComponent;
  let fixture: ComponentFixture<SideNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SideNavComponent],
      imports: [
        MatToolbarModule,
        MatSidenavModule,
        MatIconModule,
        NoopAnimationsModule,
        MatListModule,
        RouterTestingModule
      ],
      providers: [
        ChangeDetectorRef,
        {
          provide: MediaMatcher,
          useValue: {
            matchMedia: () => ({
              addEventListener: () => {
              },
              removeEventListener: () => {
              },
            }),
          },
        },
      ],
    }).compileComponents();
    fixture = TestBed.createComponent(SideNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
