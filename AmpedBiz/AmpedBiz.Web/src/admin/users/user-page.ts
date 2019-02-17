import { autoinject } from 'aurelia-framework';
import { Role, role } from '../../common/models/role';
import { User, UserPageItem } from '../../common/models/user';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../../common/models/paging';
import { ActionResult } from '../../common/controls/notification';
import { Router } from 'aurelia-router';

@autoinject
export class UserPage {

  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<UserPageItem> = new Pager<UserPageItem>();
  public booleanValue: boolean = false;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService
  ) {
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getPage();
    this.sorter["name"] = SortDirection.Ascending;
    this.sorter["username"] = SortDirection.None;
    this.sorter["branchName"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    await this.getPage();
  }

  public async getPage(): Promise<void> {
    try {
      let data = await this._api.users.getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      var response = <PagerResponse<UserPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }


  public create(): void {
    this._router.navigateToRoute('user-create');
  }

  public async edit(item: User): Promise<void> {
    if (!this.canEdit(item)) {
      var message = `You are not allowed to edit ${item.person.firstName + ' ' + item.person.lastName}.`;
      await this._notification.warning(message);
      return;
    }
    this._router.navigateToRoute('user-create', { id: item.id });
  }

  public async resetPassword(user: User): Promise<void> {
    try {
      let message = `Do you want to reset password of ${user.person.firstName} ${user.person.lastName}?`;
      let result = await this._notification.confirm(message).whenClosed();
      if (result.output === ActionResult.Yes) {
        await this._api.users.resetPassword({ id: user.id });
        await this._notification.info(`Password of  ${user.person.firstName} ${user.person.lastName} has been reset`);
      }
    }
    catch (error) {
      this._notification.error("Error encountered during reset!");
    }
  }

  public delete(user: any): void {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }

  public canEdit(user: User) {
    if (this._api.auth.isAuthorized(role.admin)) {
      return true;
    }

    var adminOrManager = (x: Role) => x.id === role.admin.id || x.id === role.manager.id;

    if (this._api.auth.isAuthorized(role.manager)) {
      // manager cannot modify users with manager or admin role
      if (user.roles.some(adminOrManager)) {
        return false;
      }
      else {
        return true;
      }
    }

    return false;
  }
}
