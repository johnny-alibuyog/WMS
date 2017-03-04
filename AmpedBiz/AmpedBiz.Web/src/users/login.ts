import { User } from '../common/models/user'
import { autoinject } from 'aurelia-framework';
//import {Validation, ValidationGroup} from 'aurelia-validation';
import { AuthService } from '../services/auth-service';
import { NotificationService } from '../common/controls/notification-service';
import { Pager } from '../common/controls/pager';

@autoinject
export class Login {
  private _auth: AuthService;
  //private _validation: ValidationGroup;
  private _notification: NotificationService;

  public user: User;

  //constructor(userService: AuthService, validation: Validation) {
  constructor(userService: AuthService) {
    this._auth = userService;
    //this._validation = validation.on(this.user)
    //  .ensure('username').isNotEmpty().hasMinLength(3).hasMaxLength(10)
    //  .ensure('password').isNotEmpty().hasMinLength(3).hasMaxLength(10);
  }

  login() {
    if (this.user.username && this.user.password) {
      this._auth.login(this.user)
        .catch(error => this._notification.warning(error));
    }
    else {
      this._notification.warning('Please enter a username and password.')
    }
  }

  logout() {
    this._auth.logout()
      .catch(error => this._notification.warning(error));
  }

  activate() {
    this.user = {
      username: 'super_user',
      password: '123!@#qweASD'
    };
  }
}
