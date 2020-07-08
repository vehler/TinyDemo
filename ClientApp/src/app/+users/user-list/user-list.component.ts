import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { UserService } from '../user.service';
import { User } from '../user.interface';

@Component({
  selector: 'app-users',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit, OnDestroy {

  public users$: Observable<User[]>;

  private _destroyed$ = new Subject();

  constructor(private readonly userService: UserService) { }

  public ngOnInit(): void {

    this.users$ = this.userService.getUsers()
      .pipe(
        takeUntil(this._destroyed$)
      );

  }

  public ngOnDestroy (): void {
    this._destroyed$.next();
    this._destroyed$.complete();
  }

}
