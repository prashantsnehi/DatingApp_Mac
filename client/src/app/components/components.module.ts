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
import { MemberCardComponent } from './members/member-card/member-card.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { MemberseditComponent } from './members/membersedit/membersedit.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { FileUploadModule } from 'ng2-file-upload';

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
    PhotoEditorComponent
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
    FileUploadModule
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
    FileUploadModule
  ]
})
export class ComponentsModule { }
