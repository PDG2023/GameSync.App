import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {LoadingService} from "../../services/loading.service";
import {GamesService} from "../../services/games.service";
import {ActivatedRoute, Router} from "@angular/router";
import {MessagesService} from "../../services/messages.service";

@Component({
  selector: 'app-add-custom-game',
  templateUrl: './add-edit-custom-game.component.html',
  styleUrls: ['./add-edit-custom-game.component.scss']
})
export class AddEditCustomGameComponent implements OnInit {
  addMode: boolean = false;
  gameId: number | null = null;

  gameForm: FormGroup = this.fb.group({
    name: [null, Validators.required],
    minPlayer: [null, Validators.required],
    maxPlayer: [null, Validators.required],
    minAge: [null, Validators.required],
    description: null,
    durationMinute: null
  });

  constructor(
    private fb: FormBuilder,
    protected loadingService: LoadingService,
    private gamesService: GamesService,
    private router: Router,
    private route: ActivatedRoute,
    private messagesService: MessagesService
  ) {
  }

  ngOnInit(): void {
    this.gameId = this.route.snapshot.params['id'];
    this.addMode = !this.gameId;
    if (!this.addMode) {
      this.gamesService.getCustomGameDetail(this.gameId!)
        .subscribe(gameDetail => {
          this.gameForm.patchValue({...gameDetail});
        });
    }
  }

  submit() {
    this.addMode ? this.addGame() : this.editGame();
  }

  private addGame() {
    this.gamesService.addGame({
      ...this.gameForm.value
    }).subscribe(() => {
      this.messagesService.success('Jeu personnalisé ajouté.');
      this.router.navigateByUrl('/collection');
    });
  }

  private editGame() {
    this.gamesService.editGame({
      ...this.gameForm.value
    }, this.gameId!).subscribe(() => {
      this.messagesService.success('Jeu personnalisé modifié.');
      this.router.navigateByUrl('/collection');
    })
  }
}
