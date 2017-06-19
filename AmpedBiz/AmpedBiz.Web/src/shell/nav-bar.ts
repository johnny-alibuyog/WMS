import { autoinject, bindable, bindingMode } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { User } from '../common/models/user';
import { AuthService } from '../services/auth-service';

@autoinject
export class NavBar {
  private _auth: AuthService;

  public user: string;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public router: Router;

  constructor(auth: AuthService) {
    this._auth = auth;
    this.user = this._auth.userFullname;
  }

  public logout(): void {
    this._auth.logout();
  }
}