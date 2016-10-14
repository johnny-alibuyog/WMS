import {autoinject, bindable, bindingMode} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {AuthService} from '../services/auth-service';

@autoinject
export class NavBar {
  private _auth: AuthService;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public router: Router;

  constructor(auth: AuthService) {
    this._auth = auth;
  }

  public logout() : void {
    this._auth.logout();
  }
}