import { autoinject } from 'aurelia-framework';
import { DialogService } from 'aurelia-dialog';
import { BranchCreate } from './branch-create';
import { Branch, BranchPageItem } from '../../common/models/branch';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../../common/models/paging';

@autoinject
export class BranchPage {
  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<Branch>;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, filter: Filter, sorter: Sorter, pager: Pager<BranchPageItem>) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;

    this.filter = filter;
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getPage();

    this.sorter = sorter;
    this.sorter["code"] = SortDirection.None;
    this.sorter["name"] = SortDirection.Ascending;
    this.sorter["descirption"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = pager;
    this.pager.onPage = () => this.getPage();
  }

  public activate(): void {
    this.getPage();
  }

  public async getPage(): Promise<void> {
    try {
      let data = await this._api.branches.getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });

      var response = <PagerResponse<BranchPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }

  public create(): void {
    this._dialog.open({ viewModel: BranchCreate, model: null })
      .whenClosed(response => { if (!response.wasCancelled) this.getPage(); });
  }

  public edit(item: BranchPageItem): void {
    this._dialog.open({ viewModel: BranchCreate, model: <Branch>{ id: item.id } })
      .whenClosed(response => { if (!response.wasCancelled) this.getPage(); });
  }

  public delete(item: any): void {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}