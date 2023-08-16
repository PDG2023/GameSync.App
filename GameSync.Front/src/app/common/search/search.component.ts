import {Component, OnInit} from '@angular/core';
import {map, Observable, of, startWith} from "rxjs";
import {FormControl} from "@angular/forms";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {
  options: string[] = ['Cluedo', 'Loups-garoux', 'Uno'];
  autoComplete = new FormControl('');
  filteredOptions$: Observable<string[]> = of(['']);

  ngOnInit() {
    //filters options based on user input value
    this.filteredOptions$ = this.autoComplete.valueChanges.pipe(
      startWith(''),
      map(value => this.options.filter(
        option => option.toLowerCase().includes(value?.toLowerCase() || '')
      ))
    );
  }
}
