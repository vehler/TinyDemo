import { Component, Input, Output, EventEmitter } from '@angular/core';
import { User } from '@app/+users';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  @Input()
  public title: string;

  @Input()
  public user: User;

  @Output()
  public onLogout: EventEmitter<boolean> = new EventEmitter<boolean>();

  public isExpanded = false;

  public collapse(): void {
    this.isExpanded = false;
  }

  public toggle(): void {
    this.isExpanded = !this.isExpanded;
  }

  public logout(): void {
    this.onLogout.emit(true);
  }
}
