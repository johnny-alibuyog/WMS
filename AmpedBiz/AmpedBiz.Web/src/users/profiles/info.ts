import { Role, role } from '../../common/models/role';

import { AuthService } from "../../services/auth-service";
import { Branch } from '../../common/models/branch';
import { DialogController } from 'aurelia-dialog';
import { NotificationService } from '../../common/controls/notification-service';
import { ServiceApi } from '../../services/service-api';
import { User } from '../../common/models/user';
import { autoinject } from 'aurelia-framework';

@autoinject
export class Info {
  private readonly _api: ServiceApi;
  private readonly _auth: AuthService;
  private readonly _notificaton: NotificationService;

  public header: string = 'Info';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public user: User;
  public branches: Branch[];

  constructor(api: ServiceApi, auth: AuthService, notification: NotificationService) {
    this._api = api;
    this._auth = auth;
    this._notificaton = notification;
  }

  public activate(): void {
    this.isEdit = false;
    this._api.branches.getList()
      .then(data => this.branches = <Branch[]>data)
      .catch(error => this._notificaton.warning(error));

    this._api.users.get(this._auth.user.id)
      .then(data => this.user = <User>data)
      .catch(error => this._notificaton.warning(error));
  }

  public toggleEdit(): void {
    this.isEdit = !this.isEdit;
  }

  public save(): void {
    // TODO: add api functionality specific to operation (change info)
    this._api.users.update(this.user)
      .then(data => {
        this._notificaton.success("Info has been saved succesfully.")
          .whenClosed((data) => this.isEdit = false);
      })
      .catch(error => {
        this._notificaton.warning(error);
      });
  }
}