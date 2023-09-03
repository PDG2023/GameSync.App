import { TestBed } from '@angular/core/testing';

import { MailConfirmationService } from './mail-confirmation.service';
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('MailConfirmationService', () => {
  let service: MailConfirmationService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ]
    });
    service = TestBed.inject(MailConfirmationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
