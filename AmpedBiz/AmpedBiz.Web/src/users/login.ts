import {autoinject} from 'aurelia-framework';
import {User} from './common/models/user'
import {AuthService} from '../services/auth-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class Login {
  private _auth: AuthService;
  private _notificaton: NotificationService;

  public user: User;

  constructor(userService: AuthService) {
    this._auth = userService;
  }

  login() {
    if (this.user.username && this.user.password) {
      this._auth.login(this.user)
        .catch(error => this._notificaton.warning(error));
    }
    else {
      this._notificaton.warning('Please enter a username and password.')
    }
  }

  logout() {
    this._auth.logout()
      .catch(error => this._notificaton.warning(error));
  }

  activate() {
    this.user = {
      username: 'Username1',
      password: 'Password1'
    };
  }
}
