import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavComponent } from './nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ListComponent } from './list/list.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MessagesComponent } from './messages/messages.component';
import { AppRoutingModule } from '../app-routing.module';
import { ToastrModule } from 'ngx-toastr';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { MemberseditComponent } from './members/membersedit/membersedit.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { FileUploadModule } from 'ng2-file-upload';
import { InputValidationComponent } from './_form/input-validation/input-validation.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { DateInputComponent } from './_form/date-input/date-input.component';


@NgModule({
  declarations: [
    NavComponent,
    HomeComponent,
    RegisterComponent,
    ListComponent,
    MemberListComponent,
    MemberDetailComponent,
    MessagesComponent,
    MemberCardComponent,
    MemberseditComponent,
    PhotoEditorComponent,
    InputValidationComponent,
    DateInputComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    BsDropdownModule.forRoot(),
    AppRoutingModule,
    ToastrModule.forRoot(
      { positionClass: 'toast-bottom-right' }
    ),
    TabsModule.forRoot(),
    NgxGalleryModule,
    NgxSpinnerModule,
    FileUploadModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot()
  ],
  exports: [
    NavComponent,
    HomeComponent,
    ListComponent,
    MemberListComponent,
    MemberDetailComponent,
    MessagesComponent,
    MemberCardComponent,
    TabsModule,
    NgxGalleryModule,
    MemberseditComponent,
    NgxSpinnerModule,
    PhotoEditorComponent,
    FileUploadModule,
    ReactiveFormsModule,
    InputValidationComponent,
    BsDatepickerModule,
    DateInputComponent
  ]
})
export class ComponentsModule { }
