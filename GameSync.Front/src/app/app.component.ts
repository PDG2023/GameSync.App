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
  gameBoxContent: string | null = null;
  validationMessage: string | null = null;
  constructor(http: HttpClient) {
    this.http = http;
    this.games = [];
  }

  addGameButtonClicked() {
    if (this.gameBoxContent === null) {
      this.validationMessage = "Veuillez entrer un nom.";
      return;
    }

    this.http
      .post<Game>(`http://localhost/api/game/${this.gameBoxContent}`, null)
      .subscribe((enteredGame: Game) => {
        this.validationMessage = `Jeu ${enteredGame.name} correctement ajouté !`;
        this.games.push(enteredGame);
      });
  }

  searchEventFired(event: any) {
    this.gameBoxContent = event.target.value;
    this.http
      .get<Game[]>(`http://localhost/api/game/search?term=${event.target.value}`)
      .subscribe((games: Game[]) => this.games = games);
  }
}
