import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from '@app/shared/guards/auth.guard';

import { HomeComponent } from '@app/home/home.component';
import { LoginComponent } from '@app/login/login.component';
import { ShellComponent } from '@app/shared/components';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: ShellComponent,
    children: [
      {
        path: '',
        pathMatch: 'full',
        component: HomeComponent
      },
      {
        path: 'users',
        canActivate: [AuthGuard],
        loadChildren: () => import('./+users/users.module').then(m => m.UsersModule)
      }
    ]
  },
  {
    path: 'login',
    component: LoginComponent
  },
  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
