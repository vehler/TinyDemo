import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';
import { UsersRoutingModule } from './users-routing.module';
import { UserListComponent } from './user-list/user-list.component';
import { UserUpsertComponent } from './user-upsert/user-upsert.component';

@NgModule({
  declarations: [UserListComponent, UserUpsertComponent],
  imports: [
    SharedModule,
    UsersRoutingModule
  ]
})
export class UsersModule { }
