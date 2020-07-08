import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserListComponent } from './user-list/user-list.component';
import { UserUpsertComponent } from './user-upsert/user-upsert.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: UserListComponent
  },
  {
    path: 'new',
    component: UserUpsertComponent
  },
  {
    path: ':id/edit',
    component: UserUpsertComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsersRoutingModule { }
