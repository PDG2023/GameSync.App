import {CanActivateChildFn, CanActivateFn} from '@angular/router';
import {environment} from "../../environments/environment";

export const anonymousGuard: CanActivateFn = (route, state) => {
  return localStorage.getItem(environment.securityStorage) === null;
};

export const anonymousGuardChild: CanActivateChildFn = (route, state) => {
  return anonymousGuard(route, state);
};
