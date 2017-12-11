import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { Role, role } from '../../common/models/role';
import { User } from '../../common/models/user';
import { Branch } from '../../common/models/branch';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';

@autoinject
export class UserCreate {
  private readonly _api: ServiceApi;
  private readonly _notification: NotificationService;
  private readonly _controller: DialogController;

  public header: string = 'Create User';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public user: User;
  public branches: Branch[];

  constructor(notification: NotificationService, controller: DialogController, api: ServiceApi) {
    this._notification = notification;
    this._controller = controller;
    this._api = api;
  }

  public activate(user: User): void {
    this._api.branches.getList()
      .then(data => this.branches = <Branch[]>data)
      .catch(error => this._notification.warning(error));

    if (user) {
      this.header = "Edit User";
      this.isEdit = true;
      this._api.users.get(user.id)
        .then(data => this.user = <User>data)
        .catch(error => this._notification.warning(error));
    }
    else {
      this.header = "Create User";
      this.isEdit = false;
      this._api.users.getInitialUser(null)
        .then(data => this.user = <User>data)
        .catch(error => this._notification.warning(error));
    }
  }

  public canAssign(_role: Role): boolean {
    if (this._api.auth.isAuthorized(role.admin)) {
      return true;
    }

    if (this._api.auth.isAuthorized(role.manager)) {
      if (_role.id === role.salesclerk.id) {
        return true;
      }
      if (_role.id === role.warehouseman.id) {
        return true;
      }
    }

    return false;
  }

  public changeValue(): void {
    this.isEdit = !this.isEdit;
  }

  public cancel(): void {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  public save(): void {
    this._notification.confirm('Do you want to save?').whenClosed(result => {
      if (result.output === ActionResult.Yes) {

        if (this.isEdit) {

          this._api.users.update(this.user)
            .then(data => {
              this._notification.success("User  has been saved.")
                .whenClosed((data) => this._controller.ok({ wasCancelled: true, output: <User>data }));
            })
            .catch(error => {
              this._notification.warning(error);
            });
        }
        else {

          this._api.users.create(this.user)
            .then(data => {
              this._notification.success("User has been saved.")
                .whenClosed((data) => this._controller.ok({ wasCancelled: true, output: <User>data }));
            })
            .catch(error => {
              this._notification.warning(error);
            });
        }
      }
    });
  }
}