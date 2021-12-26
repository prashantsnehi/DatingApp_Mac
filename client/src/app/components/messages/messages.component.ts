import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Message } from 'src/app/_models/message';
import { Pagination } from 'src/app/_models/pagination';
import { ConfirmService } from 'src/app/_services/confirm.service';
import { MessageService } from 'src/app/_services/message.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[] = [];
  pagination: Pagination;
  container = 'Unread';
  pageNumber = 1;
  pageSize = 5;
  loading = false;

  constructor(private messageService: MessageService,
    private toastr: ToastrService,
    private confirmService: ConfirmService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.loading = true;
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe(response => {
      this.messages = response.result;
      this.pagination = response.pagination;
      this.loading = false;
    })
  }

  deleteMessageWithSwalConfirmation(id: number) {
    Swal.fire({
      title: '<strong><h4>Confirmation.</h4></strong>',
      icon: 'question',
      html: 'Please confirm to delete message',
      showCloseButton: true,
      showCancelButton: true,
      confirmButtonText: '<i class="fa fa-thumbs-up"></i> Sure!',
      confirmButtonColor: 'green',
      confirmButtonAriaLabel: 'Thumbs up Sure!',
      cancelButtonText: '<i class="fa fa-thumbs-down"></i> Not Sure!',
      cancelButtonColor: 'red',
      allowOutsideClick: false
    }).then((result) => {
      if (result.isConfirmed) {
        this.messageService.deleteMessage(id).subscribe(() => {
          this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
          Swal.fire({
            icon: 'success',
            title: 'Message deleted successfully',
            showConfirmButton: false,
            timer: 1500
          })
          //   this.swalWithBootstrapButtons.fire(
          //     'Deleted!',
          //     'Your message deleted.',
          //     'success'
          //   )
        })
      } else {
        this.toastr.error('You have denied to delete message.')
      }
    })
    // if(confirm('Confirm to delete message...')){
    // this.messageService.deleteMessage(id).subscribe(() => {
    //   this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
    // })
    // }
  }

  deleteMessage(id: number) {
    this.confirmService.confirm('Confirm Delete message', 'This cannot be undone').subscribe(result => {
      if (result) {
        this.messageService.deleteMessage(id).subscribe(() => {
          this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
          Swal.fire({
            icon: 'success',
            title: 'Message deleted successfully',
            showConfirmButton: false,
            timer: 1500
          })
        })
      } else {
        this.toastr.error('You have denied to delete message.')
      }
    })
  }

  swalWithBootstrapButtons = Swal.mixin({
    customClass: {
      confirmButton: 'btn btn-success',
      cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
  })

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
    }
    this.loadMessages();
  }
}
