import { User } from '../common/models/user'
import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { AuthService } from '../services/auth-service';
import { NotificationService } from '../common/controls/notification-service';
import { Pager } from '../common/controls/pager';

@autoinject
export class Override {
  private _auth: AuthService;
  private _controller: DialogController;
  private _notification: NotificationService;

  public user: User;

  constructor(userService: AuthService, controller: DialogController, notification: NotificationService) {
    this._auth = userService;
    this._controller = controller;
    this._notification = notification;
  }

  override() {
    if (this.user.username && this.user.password) {
      this._auth.override(this.user)
        .then(data => this._controller.ok({ output: <User>data }))
        .catch(error => this._notification.warning(error));
    }
    else {
      this._notification.warning('Please enter a username and password.')
    }
  }

  cancel() {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  activate() {
    this.user = {
      username: 'super_user',
      password: '123!@#qweASD'
    };
  }
}
