import { User } from '../common/models/user'
import { autoinject } from 'aurelia-framework';
//import {Validation, ValidationGroup} from 'aurelia-validation';
import { AuthService } from '../services/auth-service';
import { NotificationService } from '../common/controls/notification-service';
import { isNullOrWhiteSpace } from '../common/utils/string-helpers';

import 'assets/common/images/logo.png';

@autoinject
export class Login {

  public user: User;

  //constructor(userService: AuthService, validation: Validation) {
  constructor(
    private readonly _auth: AuthService,
    private readonly _notification: NotificationService) {
    //this._validation = validation.on(this.user)
    //  .ensure('username').isNotEmpty().hasMinLength(3).hasMaxLength(10)
    //  .ensure('password').isNotEmpty().hasMinLength(3).hasMaxLength(10);
  }

  public async login(): Promise<void> {
    try {
      if (isNullOrWhiteSpace(this.user.username) || isNullOrWhiteSpace(this.user.password)) {
        this._notification.warning('Please enter a username and password.');
        return;
      }
      await this._auth.login(this.user);
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async logout(): Promise<void> {
    try {
      await this._auth.logout();
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public activate(): void {
    this.user = {
      username: '',
      password: ''
    }
  }
}
