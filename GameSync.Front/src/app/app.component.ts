import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';


interface Game {
  name: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'GameSync.Front';
  http: HttpClient;
  games: Game[];
  constructor(http: HttpClient) {
    this.http = http;
    this.games = [];
  }

  searchEventFired(event: any) {
    
    this.http
      .get<Game[]>(`http://localhost/api/search?term=${event.target.value}`)
      .subscribe((games: Game[]) => this.games = games);
  }
}
