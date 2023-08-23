import {HttpInterceptor, HttpHandler, HttpRequest, HttpEvent} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {catchError, Observable, of} from "rxjs";
import {Router} from "@angular/router";
import {MessagesService} from "../services/messages.service";
import {environment} from "../../environments/environment";


@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private router: Router,
    private messagesService: MessagesService
  ) {
  }

  intercept(req: HttpRequest<any>,
            next: HttpHandler): Observable<HttpEvent<any>> {

    const token = localStorage.getItem(environment.securityStorage);

    const isApiUrl = req.url.startsWith(environment.apiUrl);
    if (isApiUrl && token) {


      let headers = req.headers.set("Authorization",
        "Bearer " + token);

      req = req.clone({
        headers: headers
      });
      // This error can only be handled here
      return next.handle(req).pipe(
        catchError(
          error => {
            if (error.status === 401) {
              localStorage.removeItem(environment.securityStorage);
              this.router.navigateByUrl("/");
              this.messagesService.error('Vous avez été automatiquement déconnecté.');
            }
            return of();
          }
        )
      );
    }
    return next.handle(req);
  }
}
