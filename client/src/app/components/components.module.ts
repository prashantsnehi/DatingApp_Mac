import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavComponent } from './nav/nav.component';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';

@NgModule({
  declarations: [
    NavComponent,
    HomeComponent,
    RegisterComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    BsDropdownModule.forRoot()
  ],
  exports: [
    NavComponent,
    HomeComponent
  ]
})
export class ComponentsModule { }
