import { Role, role } from '../../common/models/role';

import { AuthService } from "../../services/auth-service";
import { Branch } from '../../common/models/branch';
import { DialogController } from 'aurelia-dialog';
import { NotificationService } from '../../common/controls/notification-service';
import { ServiceApi } from '../../services/service-api';
import { UserInfo } from '../../common/models/user';
import { autoinject } from 'aurelia-framework';

@autoinject
export class Info {
  private readonly _api: ServiceApi;
  private readonly _auth: AuthService;
  private readonly _notificaton: NotificationService;

  public header: string = 'Info';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public user: UserInfo;

  constructor(api: ServiceApi, auth: AuthService, notification: NotificationService) {
    this._api = api;
    this._auth = auth;
    this._notificaton = notification;
  }

  public async activate(): Promise<void> {
    try {
      this.isEdit = false;
      this.user = await this._api.users.getInfo(this._api.auth.user.id);
    }
    catch (error) {
      this._notificaton.warning(error);
    }
  }

  public toggleEdit(): void {
    this.isEdit = !this.isEdit;
  }

  public async save(): Promise<void> {
    try {
      await this._api.users.updateInfo(this.user)
      await this._notificaton.success("Info has been saved succesfully.").whenClosed();
      this.isEdit = false;
    }
    catch (error) {
      this._notificaton.warning(error);
    }
  }
}