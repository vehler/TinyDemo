import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { first } from 'rxjs/operators';

import { UserService } from '../user.service';
import { User } from '..';

@Component({
  selector: 'app-user-upsert',
  templateUrl: './user-upsert.component.html',
  styleUrls: ['./user-upsert.component.scss']
})
export class UserUpsertComponent implements OnInit {

  public editId: number;
  public upsertUserForm: FormGroup;
  public loading = false;
  public submitted = false;
  public error = '';

  constructor(private readonly formBuilder: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly userService: UserService
  ) { }

  public ngOnInit(): void {
    this.upsertUserForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required],
      phone: ['']
    });

    this.editId = this.route.snapshot.params['id'];

  }

   // convenience getter for easier access to form fields
   public get f() { return this.upsertUserForm.controls; }

  public onSubmit(): void {
    this.submitted = true;
    this.toggleForm();

    if (this.upsertUserForm.invalid) {
      this.toggleForm();
      return;
    }

    this.loading = true;

    var user = {
      firstName: this.f.firstName.value,
      lastName: this.f.lastName.value,
      email: this.f.email.value,
      phone: this.f.phone.value
    } as User;

    this.userService.createUser(user)
      .pipe(first())
      .subscribe({
        next: () => {
          this.router.navigate(['/users']);
        },
        error: error => {
          this.error = error;
          this.loading = false;
        }
      });
  }

  private toggleForm(): void {
    for(let control in this.upsertUserForm.controls) {
      if(this.upsertUserForm.controls[control].disabled) {
        this.upsertUserForm.controls[control].enable();
      } else {
        this.upsertUserForm.controls[control].disable();
      }
    }
  }
}
