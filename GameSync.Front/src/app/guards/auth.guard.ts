import {CanActivateChildFn, CanActivateFn, Router} from '@angular/router';
import {environment} from "../../environments/environment";
import {inject} from "@angular/core";

export const authGuard: CanActivateFn = (route, state) => {
  const router: Router = inject(Router);
  if (localStorage.getItem(environment.securityStorage) !== null) {
    return true;
  }
  router.navigateByUrl('/login');
  return false;
};

export const authGuardChild: CanActivateChildFn = (route, state) => {
  return authGuard(route,state);
};
