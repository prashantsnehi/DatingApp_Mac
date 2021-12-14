import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { Login } from 'src/app/_models/login';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: Login;
  currentUser$: Observable<User>;
  userDto: any;
  loginForm: FormGroup;
  formValidator: boolean;

  constructor(private accountService: AccountService,
    private router: Router,
    private toastr: ToastrService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
    this.initializeForm();
    this.model = { username: '', password: '' };
    this.formValidator = this.validateForm();
  }

  login() {
    if(this.model.username.length == 0) return this.toastr.error("Username is required");
    if(this.model.password.length == 0) return this.toastr.error("Password is required");

    this.accountService.login(this.model).subscribe(response => {
      this.router.navigateByUrl('/members');
      this.userDto = JSON.parse(localStorage.getItem('user'));
      this.toastr.success('Welcome ' + this.userDto?.knownAs);
    });
  }

  validateForm =() =>
    this.model.username.length > 0 && this.model.password.length > 0 ? true : false;

  loginUsingFormBuilder() {
    this.accountService.login(this.loginForm.value).subscribe(response => {
      this.router.navigateByUrl('/members');
      this.userDto = JSON.parse(localStorage.getItem('user'));
      this.toastr.success('Welcome ' + this.userDto?.knownAs);
    });
  }

  initializeForm() {
    this.loginForm = this.fb.group({
      username: ['', [ Validators.required, Validators.minLength(3) ]],
      password: ['', [ Validators.required, Validators.minLength(5) ]]
    });
  }

  logout() {
    this.accountService.logout();
    this.model = { username: '', password: '' };
    this.initializeForm();
    this.router.navigateByUrl('/');
    this.toastr.success("Session ennded");
  }
}
