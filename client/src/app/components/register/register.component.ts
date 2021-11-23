import { EmitterVisitorContext } from '@angular/compiler';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();

  model: any = {};
  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
    console.log(this.model);
  }

  register() {
    this.accountService.registerUser(this.model).subscribe(response => {
      console.log(response);
      this.cancel();
    }, error => {
      console.log(error);
      this.toastr.error(JSON.stringify(error.error.errors));
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
