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

  public header: string = 'Create User';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public user: User;
  public branches: Branch[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _notification: NotificationService,
    private readonly _controller: DialogController,
  ) { }

  public async activate(user: User): Promise<void> {
    try {
      this.isEdit = (user) ? true : false;
      this.header = (this.isEdit) ? "Edit User" : "Create User";
      [this.branches, this.user] = await Promise.all([
        this._api.branches.getList(),
        (this.isEdit)
          ? this._api.users.get(user.id)
          : this._api.users.getInitialUser(null)
      ]);
    }
    catch (error) {
      this._notification.warning(error)
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
    this._controller.cancel();
  }

  public async save(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to save?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = (this.isEdit)
          ? await this._api.users.update(this.user)
          : await this._api.users.create(this.user);
        await this._notification.success("User  has been saved.").whenClosed();
        this._controller.ok(<User>data);
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public toggle(role: Role): void {
    role.assigned = !role.assigned;
  }
}
