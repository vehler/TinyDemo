import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '@app/shared/services/authentication.service';
import { User } from '@app/+users';

@Component({
  selector: 'app-shell',
  templateUrl: './shell.component.html',
  styleUrls: ['./shell.component.scss']
})
export class ShellComponent implements OnInit {

  public title = 'Tiny Demo App';
  public authUser: User;
  public isLoggedIn: boolean;

  constructor(private readonly authService: AuthenticationService) {}

  public ngOnInit(): void {

    this.authService.user
      .subscribe(user => {
        this.authUser = user;
      });
  }

  public logout(): void {
    this.authService.logout();
  }
}
