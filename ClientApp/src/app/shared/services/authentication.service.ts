import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { BehaviorSubject, Observable } from 'rxjs';

import { User } from '@app/+users';
import { map, filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  public user: Observable<User>;
  private authUrl = `${environment.apiRoot}v1/auth`;
  private userSubject: BehaviorSubject<User>;
  private refreshTokenTimeoutRef: any;

  constructor(
    private readonly router: Router,
    private readonly http: HttpClient) {

    this.userSubject = new BehaviorSubject<User>(null);
    this.user = this.userSubject.asObservable();
  }

  public get userValue(): User {
    return this.userSubject.value;
  }

  public login(email: string, password: string): Observable<User> {
    return this.http.post<User>(this.authUrl, { email, password }, { withCredentials: true })
      .pipe(
        map((user: User) => {
          this.userSubject.next(user);
          this.startRefreshTokenTimer();
          return user;
        })
      );
  }

  public refreshToken(): Observable<User> {
    return this.http.post<User>(`${this.authUrl}/refresh`, null, { withCredentials: true })
      .pipe(
        map((user: User) => {
          if (user) {
            this.userSubject.next(user);
            this.startRefreshTokenTimer();
          }
          return user;
        })
      );
  }

  public logout(): void {
    this.http.post<any>(`${this.authUrl}/revoke`, {}, { withCredentials: true })
      .subscribe(_ => {
        this.stopRefreshTokenTimer();
        this.userSubject.next(null);
        this.router.navigate(['/login']);
      });
  }

  // helpers

  private startRefreshTokenTimer(): void {
    // parse json object from base64 encoded jwt token
    const jwtToken = JSON.parse(atob(this.userValue?.jwtToken.split('.')[1]));

    console.log({ jwtToken });

    // set a timeout to refresh the token a minute before it expires
    const expires = new Date(jwtToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - (60 * 1000);
    this.refreshTokenTimeoutRef = setTimeout(() => this.refreshToken().subscribe(), timeout);
  }

  private stopRefreshTokenTimer(): void {
    clearTimeout(this.refreshTokenTimeoutRef);
  }
}
