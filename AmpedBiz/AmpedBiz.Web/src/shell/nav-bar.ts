import { autoinject, bindable, bindingMode } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { AuthService } from '../services/auth-service';
import { role } from 'common/models/role';

@autoinject
export class NavBar {
  private _auth: AuthService;

  public canAccessSettings: boolean = false;

  public user: string;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public router: Router;

  constructor(auth: AuthService) {
    this._auth = auth;
    this.canAccessSettings = this._auth.isAuthorized([role.admin, role.manager]);
    this.user = this._auth.userFullname;
  }

  public logout(): void {
    this._auth.logout();
  }
}
