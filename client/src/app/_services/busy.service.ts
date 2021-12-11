import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount = 0;
  constructor(private spinnerService: NgxSpinnerService) { }

  busy() {
    this.busyRequestCount++;
    this.spinnerService.show(undefined, {
      // type: 'line-scale-party',
      // bdColor: 'rgba(255, 255, 255, 0.3)',
      // color: '#333333',
      // type: 'ball-scale-multiple',
      // type: 'timer',
      // type: 'ball-clip-rotate-multiple',
      // type: 'ball-fussion',
      // type: 'ball-spin-clockwise',
      // type: 'cog',
      type: 'square-jelly-box',
      bdColor: 'rgba(51,51,51,0.8)',
      color: '#fff',
      size: 'medium'
    })
  }

  idle() {
    this.busyRequestCount--;
    if(this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.spinnerService.hide();
    }
  }
}
