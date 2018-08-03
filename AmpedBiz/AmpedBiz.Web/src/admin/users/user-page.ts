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

  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<UserPageItem> = new Pager<UserPageItem>();
  public booleanValue: boolean = false;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _dialog: DialogService,
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

  public async create(): Promise<void> {
    let settings = { viewModel: UserCreate, model: null };
    let response = await this._dialog.open(settings).whenClosed();
    if (!response.wasCancelled) this.getPage();
  }

  public async edit(user: User): Promise<void> {
    if (!this.canEdit(user)) {
      var message = `You are not allowed to edit ${user.person.firstName + ' ' + user.person.lastName}.`;
      await this._notification.warning(message);
      return;
    }

    let settings = { viewModel: UserCreate, model: user };
    let response = await this._dialog.open(settings).whenClosed();
    if (!response.wasCancelled) await this.getPage();
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