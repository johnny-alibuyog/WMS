import { Role, role } from '../../common/models/role';

import { AuthService } from "../../services/auth-service";
import { Branch } from '../../common/models/branch';
import { DialogController } from 'aurelia-dialog';
import { NotificationService } from '../../common/controls/notification-service';
import { ServiceApi } from '../../services/service-api';
import { User } from '../../common/models/user';
import { autoinject } from 'aurelia-framework';
import { ActionResult } from '../../common/controls/notification';

@autoinject
export class ChangePassword {
  private readonly _api: ServiceApi;
  private readonly _auth: AuthService;
  private readonly _notificaton: NotificationService;

  public header: string = 'Change Password';
  public newPassword: string;
  public confirmPassword: string;
  public user: User;
  public branches: Branch[];

  constructor(api: ServiceApi, auth: AuthService, notification: NotificationService) {
    this._api = api;
    this._auth = auth;
    this._notificaton = notification;
  }

  public activate(): void {

    this.newPassword = '';

    this.confirmPassword = '';

    this._api.branches.getList()
      .then(data => this.branches = <Branch[]>data)
      .catch(error => this._notificaton.warning(error));

    this._api.users.get(this._auth.user.id)
      .then(data => this.user = <User>data)
      .catch(error => this._notificaton.warning(error));
  }

  public save(): void {
    // TODO: add api functionality specific to operation (change info)
    if (!this.newPassword || this.newPassword.trim() === '') {
      this._notificaton.warning('New password not set.');
      return;
    }

    if (!this.confirmPassword || this.confirmPassword.trim() === '') {
      this._notificaton.warning('Confirm password not set.');
      return;
    }

    if (this.newPassword !== this.confirmPassword) {
      this._notificaton.warning('Confirm password does not match.');
      return;
    }

    this.user.password = this.newPassword;

    this._notificaton.confirm('Do you want to change your password?').whenClosed(result => {
      if (result.output === ActionResult.Yes) {
        this._api.users.update(this.user)
          .then(data => this._notificaton.success('Password has been changed succesfully.'))
          .catch(error => this._notificaton.warning(error));
      }
    });
  }
}