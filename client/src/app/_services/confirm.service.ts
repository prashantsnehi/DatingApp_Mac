import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { observable, Observable } from 'rxjs';
import { ConfirmDialogComponent } from '../components/modals/confirm-dialog/confirm-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  bsModelRef: BsModalRef;

  constructor(private modelService: BsModalService) { }

  confirm(title = 'Confirmation',
          message = 'Are you sure?',
          btnOkText = 'Confirm',
          btnCancelText = 'Cancel'): Observable<boolean> {
      const config = {
        initialState: {
          title, message, btnOkText, btnCancelText
        }
      }
      this.bsModelRef = this.modelService.show(ConfirmDialogComponent, config);
      return new Observable<boolean>(this.getResult());
  }

  private getResult() {
    return (obserer) => {
      const subscription = this.bsModelRef.onHidden.subscribe(() => {
        obserer.next(this.bsModelRef.content.result);
        obserer.complete();
      });

      return {
        unsubscribe() {
          subscription.unsubscribe();
        }
      }
    }
  }
}
