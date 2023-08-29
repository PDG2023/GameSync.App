import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import {MessagesService} from "../services/messages.service";
import {HttpErrorResponseDetail} from "../models/models";

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(
    private messagesService: MessagesService
  ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(
        errorResponse => {
          console.log('fdp', errorResponse);
          switch (errorResponse.status) {
            case 0:
              this.messagesService.error('Le serveur ne répond pas.');
              break;
            case 400:
              if(errorResponse.error.errors && errorResponse.error.errors.length > 0) {
                const errorConcat = errorResponse.error.errors
                  .map((err: HttpErrorResponseDetail) => err.description)
                  .join('\r\n');
                this.messagesService.error(errorConcat);
              }
              break;
            case 500:
              this.messagesService.error('Une erreur est survenue sur le serveur.');
              break;
          }
          return of()
        }
      )
    );
  }
}
