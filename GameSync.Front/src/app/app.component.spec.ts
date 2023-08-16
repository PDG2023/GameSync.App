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

describe('AppComponent', () => {
  beforeEach(() => TestBed.configureTestingModule({
    declarations: [AppComponent, SideNavComponent],
    imports: [
      MatToolbarModule,
      MatSidenavModule,
      BrowserAnimationsModule,
      MatIconModule,
      RouterTestingModule,
      MatListModule
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
