import { autoinject } from 'aurelia-framework';
import { DialogService } from 'aurelia-dialog';
import { UserCreate } from './user-create';
import { Role, role } from '../../common/models/role';
import { User, UserPageItem } from '../../common/models/user';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../../common/models/paging';

@autoinject
export class UserPage {
  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<UserPageItem>;
  public booleanValue: boolean = false;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, filter: Filter, sorter: Sorter, pager: Pager<UserPageItem>) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;

    this.filter = filter;
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getPage();

    this.sorter = sorter;
    this.sorter["name"] = SortDirection.Ascending;
    this.sorter["username"] = SortDirection.None;
    this.sorter["branchName"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = pager;
    this.pager.onPage = () => this.getPage();
  }

  public activate(): void {
    this.getPage();
  }

  public getPage(): void {
    this._api.users
      .getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<UserPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  public create(): void {
    this._dialog.open({ viewModel: UserCreate, model: null })
      .whenClosed(response => { if (!response.wasCancelled) this.getPage(); });
  }

  public edit(user: User): void {
    if (!this.canEdit(user)){
        this._notification.warning(`You are not allowed to edit ${user.person.firstName + ' ' + user.person.lastName}.`);
        return;
    }

    this._dialog.open({ viewModel: UserCreate, model: user })
      .whenClosed(response => { if (!response.wasCancelled) this.getPage(); });
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