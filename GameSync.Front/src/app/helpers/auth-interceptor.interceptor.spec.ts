import { TestBed } from '@angular/core/testing';

import { AuthInterceptor } from './auth-interceptor.interceptor';
import {MatSnackBarModule} from "@angular/material/snack-bar";

describe('AuthInterceptorInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      AuthInterceptor
      ],
    imports: [
      MatSnackBarModule
    ]
  }));

  it('should be created', () => {
    const interceptor: AuthInterceptor = TestBed.inject(AuthInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
