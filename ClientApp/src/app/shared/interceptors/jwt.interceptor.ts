import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '@environments/environment';
import { AuthenticationService } from '@app/shared/services/authentication.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private authenticationService: AuthenticationService) { }

  public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      // add auth header with jwt if user is logged in and request is to the api url
      const user = this.authenticationService.userValue;
      const isLoggedIn = user?.jwtToken;
      const isApiUrl = request.url.startsWith(environment.apiRoot);

      if (isLoggedIn && isApiUrl) {
          request = request.clone({
              setHeaders: { Authorization: `Bearer ${user.jwtToken}` }
          });
      }

      return next.handle(request);
  }

}
