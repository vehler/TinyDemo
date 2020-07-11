import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '@app/shared/services/authentication.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

  public visitorName: string = 'Visitor';

  constructor(private readonly authService: AuthenticationService) { }

  public ngOnInit(): void {
    this.authService.user
      .subscribe(user => {
        this.visitorName = user?.firstName;
      });
  }

}
