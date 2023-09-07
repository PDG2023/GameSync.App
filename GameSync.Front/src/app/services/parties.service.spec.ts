import {TestBed} from '@angular/core/testing';

import {PartiesService} from './parties.service';
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('PartiesService', () => {
  let service: PartiesService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ]
    });
    service = TestBed.inject(PartiesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
