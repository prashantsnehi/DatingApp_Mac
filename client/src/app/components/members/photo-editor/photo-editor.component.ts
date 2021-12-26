import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Photo } from 'src/app/_models/photo.';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { environment } from 'src/environments/environment';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: Member;
  baseUrl: string = environment.apiUrl;
  uploader: FileUploader;
  hasBaseDropzoneOver = false;
  user: User;

  constructor(private accountService: AccountService, private memberService: MembersService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.initilizeUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropzoneOver = e;
  }

  initilizeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, header) => {
      if(response) {
        const photo: Photo = JSON.parse(response);
        this.member.photos.push(photo);
        if(photo.isMain) {
          this.user.photoUrl = photo.url;
          this.member.photoUrl = photo.url;
          this.accountService.setCurrentUser(this.user);
        }
      }
    }
  }

  setMainPhoto(photo: Photo) {
    this.memberService.setMainPhoto(photo.id).subscribe(() => {
      this.user.photoUrl = photo.url;
      this.accountService.setCurrentUser(this.user);
      this.member.photoUrl = photo.url;
      this.member.photos.forEach(p => {
        if(p.isMain) p.isMain = false;
        if(p.id == photo.id) p.isMain = true;
      })
    })
  }

  deletePhoto(photoId: number) {
    Swal.fire({
      title: '<strong><h4>Confirmation.</h4></strong>',
      icon: 'question',
      html: 'Are you sure you want to delte this photo',
      showCloseButton: true,
      showCancelButton: true,
      confirmButtonText: '<i class="fa fa-thumbs-up"></i> Sure!',
      confirmButtonColor: 'green',
      confirmButtonAriaLabel: 'Thumbs up Sure!',
      cancelButtonText: '<i class="fa fa-thumbs-down"></i> Not Sure!',
      cancelButtonColor: 'red',
      allowOutsideClick: false
    }).then((result) => {
      this.memberService.deletePhoto(photoId).subscribe(() => {
        this.member.photos = this.member.photos.filter(x => x.id != photoId);
      })
    })
  }

}
