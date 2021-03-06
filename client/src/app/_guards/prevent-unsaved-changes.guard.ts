import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberDetailComponent } from '../components/members/member-detail/member-detail.component';
import { MemberseditComponent } from '../components/members/membersedit/membersedit.component';
import { ConfirmService } from '../_services/confirm.service';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanActivate, CanDeactivate<unknown> {

  constructor(private confirmService: ConfirmService) { }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return true;
  }
  canDeactivate(component: MemberseditComponent): Observable<boolean> | boolean {
    if(component.edirForm.dirty) {
      // return confirm("Are you sure you want to continue? Any unsaved changes will be lost...");
      return this.confirmService.confirm();
    }
    return true;
  }
  
}
