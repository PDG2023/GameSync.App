import {ChangeDetectorRef, Component, OnDestroy, OnInit} from '@angular/core';
import {MediaMatcher} from "@angular/cdk/layout";
import {map, Observable, of, startWith} from "rxjs";
import {FormControl} from "@angular/forms";

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss']
})
export class SideNavComponent implements OnInit, OnDestroy {
  mobileQuery: MediaQueryList;
  options: string[] = ['Cluedo', 'Loups-garoux', 'Uno'];
  autoComplete = new FormControl('');
  filteredOptions$: Observable<string[]> = of(['']);

  private mobileQueryListener: () => void;

  constructor(changeDetectorRef: ChangeDetectorRef, media: MediaMatcher) {
    this.mobileQuery = media.matchMedia('(max-width: 600px)');
    this.mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addEventListener('change', this.mobileQueryListener);
  }

  ngOnInit() {
    //filters options based on user input value
    this.filteredOptions$ = this.autoComplete.valueChanges.pipe(
      startWith(''),
      map(value => this.options.filter(
        option => option.toLowerCase().includes(value?.toLowerCase() || '')
      ))
    );
  }

  ngOnDestroy(): void {
    this.mobileQuery.removeEventListener('change', this.mobileQueryListener);
  }
}
