import { TestBed } from '@angular/core/testing';

import { MessagesService } from './messages.service';
import {MatSnackBarModule} from "@angular/material/snack-bar";

describe('MessagesService', () => {
  let service: MessagesService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        MatSnackBarModule
      ]
    });
    service = TestBed.inject(MessagesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
