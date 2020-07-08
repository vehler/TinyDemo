import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthenticationService } from './shared';
import { User } from './+users';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

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
