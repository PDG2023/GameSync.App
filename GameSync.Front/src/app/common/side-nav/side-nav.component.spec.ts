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
import {SearchComponent} from "../search/search.component";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {MatAutocompleteModule} from "@angular/material/autocomplete";
import {ReactiveFormsModule} from "@angular/forms";

describe('SideNavComponent', () => {
  let component: SideNavComponent;
  let fixture: ComponentFixture<SideNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SideNavComponent, SearchComponent],
      imports: [
        MatToolbarModule,
        MatSidenavModule,
        MatIconModule,
        NoopAnimationsModule,
        MatListModule,
        RouterTestingModule,
        MatFormFieldModule,
        MatInputModule,
        MatAutocompleteModule,
        ReactiveFormsModule
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
