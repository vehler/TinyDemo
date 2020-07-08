import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { User } from './user.interface';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private readonly http: HttpClient) { }

  public getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${environment.apiRoot}v1/users`);
  }

  public createUser(user: User) {
    return this.http.post(`${environment.apiRoot}v1/users`, user, { withCredentials: true });
  }

}
