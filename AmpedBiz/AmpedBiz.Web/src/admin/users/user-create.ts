import { autoinject } from 'aurelia-framework';
import { Role, role } from '../../common/models/role';
import { User } from '../../common/models/user';
import { Branch } from '../../common/models/branch';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';
import { Router } from 'aurelia-router';

@autoinject
export class UserCreate {

  public header: string = 'Create User';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public user: User;
  public branches: Branch[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService,
  ) { }

  public async activate(user: User): Promise<void> {
    try {
      this.isEdit = (user && user.id) ? true : false;
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

  public back(): void {
    this._router.navigateBack();
  }

  public async save(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to save?').whenClosed();
      if (result.output === ActionResult.Yes) {
        this.user = (this.isEdit)
          ? await this._api.users.update(this.user)
          : await this._api.users.create(this.user);
        await this._notification.success("User  has been saved.").whenClosed();
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
