import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from '@app/shared/guards/auth.guard';

import { HomeComponent } from '@app/home/home.component';
import { LoginComponent } from '@app/login/login.component';
import { ShellComponent, NotFoundComponent } from '@app/shared/components';

const routes: Routes = [
  {
    path: '',
    component: ShellComponent,
    children: [
      {
        path: 'users',
        canActivate: [AuthGuard],
        loadChildren: () => import('./+users/users.module').then(m => m.UsersModule)
      },
      {
        path: '',
        component: HomeComponent
      }
    ]
  },
  {
    path: 'login',
    component: LoginComponent
  },
  // otherwise redirect to home
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
