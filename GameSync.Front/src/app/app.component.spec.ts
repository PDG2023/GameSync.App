import {TestBed} from '@angular/core/testing';
import {AppComponent} from './app.component';
import {SideNavComponent} from "./common/side-nav/side-nav.component";
import {MatSidenavModule} from "@angular/material/sidenav";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatIconModule} from "@angular/material/icon";
import {RouterTestingModule} from "@angular/router/testing";
import {MatListModule} from "@angular/material/list";
import {MediaMatcher} from "@angular/cdk/layout";
import {ChangeDetectorRef} from "@angular/core";
import {SearchComponent} from "./common/search/search.component";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {MatAutocompleteModule} from "@angular/material/autocomplete";
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('AppComponent', () => {
  beforeEach(() => TestBed.configureTestingModule({
    declarations: [AppComponent, SideNavComponent, SearchComponent],
    imports: [
      MatToolbarModule,
      MatSidenavModule,
      BrowserAnimationsModule,
      MatIconModule,
      RouterTestingModule,
      MatListModule,
      MatFormFieldModule,
      MatInputModule,
      MatAutocompleteModule,
      HttpClientTestingModule
    ],
    providers: [
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
      ChangeDetectorRef,
    ]
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
