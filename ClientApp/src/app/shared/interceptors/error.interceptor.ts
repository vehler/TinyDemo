import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';
import { catchError } from 'rxjs/operators';

const UNAUTHENTICATED = 401;
const UNAUTHROIZED = 403;

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private authService: AuthenticationService) { }

  public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((err: HttpResponse<any>) => {

        if ([UNAUTHENTICATED, UNAUTHROIZED].includes(err.status) && this.authService.userValue) {
          // auto logout if 401 or 403 response returned from api
          this.authService.logout();
        }

        const error = (err?.body?.error?.message) || err.statusText;
        console.error(err);
        return throwError(error);

      })
    );
  }

}
