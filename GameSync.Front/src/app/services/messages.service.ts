import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  private readonly FERMER = 'Fermer';


  constructor(
    private snackBar: MatSnackBar
  ) { }

  success(message: string) {
    this.snackBar.open(message, this.FERMER, {
      panelClass: ['success-snackbar']
    });
  }

  error(message: string) {
    this.snackBar.open(message, this.FERMER, {
      duration: 5000,
      panelClass: ['error-snackbar']
    });
  }
}
