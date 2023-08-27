import { Component } from '@angular/core';
import { Observable, of } from 'rxjs';
import { GameDetail } from 'src/app/models/models';

@Component({
  selector: 'app-game-detail',
  templateUrl: './game-detail.component.html',
  styleUrls: ['./game-detail.component.scss']
})
export class GameDetailComponent {
  game$: Observable<GameDetail> = of({
    id: 0,
    name: 'Les Loups-Garous de Thiercelieux',
    yearPublished: 2015,
    minPlayer: 8,
    maxPlayer: 10,
    minAge: 10,
    durationMinute: 30,
    description: 'Comment, vous ne connaissez pas Thiercelieux ? Un si joli petit village de l\'est, bien à l\'abri des vents et du froid, niché entre de charmantes forêts et de bons pâturages.' +
      'Les habitants de Thiercelieux sont d\'affables paysans, heureux de leur tranquillité et fiers de leur travail. Autour d\'eux, on trouve des personnages aussi divers qu\'une voyante, une sorcière, un voleur, le capitaine (qui radote sans cesse), Cupidon (qui noue les coeurs et les idylles), et même une petite fille aux couettes charmantes.',
    userId: 0,

  })
}
