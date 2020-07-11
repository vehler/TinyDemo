import { APP_INITIALIZER } from "@angular/core";
import { HTTP_INTERCEPTORS } from "@angular/common/http";

import { AppInitializer } from "./app.initializer";
import { AuthenticationService } from '@app/shared/services/authentication.service';
import { JwtInterceptor } from '@app/shared/interceptors/jwt.interceptor';
import { ErrorInterceptor } from '@app/shared/interceptors/error.interceptor';

export const AppInitializerProvider = {
  provide: APP_INITIALIZER,
  useFactory: AppInitializer,
  multi: true, deps: [AuthenticationService]
};

export const JWTInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: JwtInterceptor,
  multi: true
};

export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
};
