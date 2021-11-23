import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavComponent } from './nav/nav.component';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ListComponent } from './list/list.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MessagesComponent } from './messages/messages.component';
import { AppRoutingModule } from '../app-routing.module';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [
    NavComponent,
    HomeComponent,
    RegisterComponent,
    ListComponent,
    MemberListComponent,
    MemberDetailComponent,
    MessagesComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    BsDropdownModule.forRoot(),
    AppRoutingModule,
    ToastrModule
  ],
  exports: [
    NavComponent,
    HomeComponent,
    ListComponent,
    MemberListComponent,
    MemberDetailComponent,
    MessagesComponent
  ]
})
export class ComponentsModule { }
