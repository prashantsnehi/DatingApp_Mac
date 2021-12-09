import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, observable } from 'rxjs';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  currentUser$: Observable<User>;
  userDto: any;
  constructor(private accountService: AccountService, 
              private router: Router,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
  }

  login() {
    this.accountService.login(this.model).subscribe(response => {
      this.router.navigateByUrl('/members');
      this.userDto = JSON.parse(localStorage.getItem('user'));
      this.toastr.success('Welcome ' + this.userDto?.userName);
    });   
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  } 
}
