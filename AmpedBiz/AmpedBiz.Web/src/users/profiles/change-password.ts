import { Branch } from '../../common/models/branch';
import { NotificationService } from '../../common/controls/notification-service';
import { ServiceApi } from '../../services/service-api';
import { UserPassword } from '../../common/models/user';
import { autoinject } from 'aurelia-framework';
import { ActionResult } from '../../common/controls/notification';

@autoinject
export class ChangePassword {

  public header: string = 'Change Password';
  public user: UserPassword;
  public branches: Branch[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _notificaton: NotificationService
  ) { }

  public activate(): void {
    this.user = <UserPassword>{
      id: this._api.auth.user.id
    }
  }

  public async save(): Promise<void> {
    try {
      if (!this.user.oldPassword || this.user.oldPassword.trim() === '') {
        this._notificaton.warning('Old password not set.');
        return;
      }

      if (!this.user.newPassword || this.user.newPassword.trim() === '') {
        this._notificaton.warning('New password not set.');
        return;
      }

      if (!this.user.confirmPassword || this.user.confirmPassword.trim() === '') {
        this._notificaton.warning('Confirm password not set.');
        return;
      }

      if (this.user.newPassword !== this.user.confirmPassword) {
        this._notificaton.warning('Confirm password does not match.');
        return;
      }

      let result = await this._notificaton.confirm('Do you want to change your password?').whenClosed();
      if (result.output === ActionResult.Yes) {
        await this._api.users.updatePassword(this.user);
        await this._notificaton.success('Password has been changed succesfully.').whenClosed();
      }

      this.user.oldPassword = '';
      this.user.newPassword = '';
      this.user.confirmPassword = '';
    }
    catch (error) {
      await this._notificaton.warning(error).whenClosed();
    }
  }
}