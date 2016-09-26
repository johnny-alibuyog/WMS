import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {CustomerCreate} from './customer-create';
import {Customer, CustomerPageItem} from '../common/models/customer';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class CustomerPage {
  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;

  public header: string = ' Customers';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<CustomerPageItem>;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, filter: Filter, sorter: Sorter, pager: Pager<CustomerPageItem>) {
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

  private getPage(): void {
    this._api.customers
      .getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<CustomerPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  public create(): void {
    this._dialog.open({ viewModel: CustomerCreate, model: null })
      .then(response => { if (!response.wasCancelled) this.getPage(); });
  }

  public edit(item: CustomerPageItem): void {
    this._dialog.open({ viewModel: CustomerCreate, model: <Customer>{ id: item.id } })
      .then(response => { if (!response.wasCancelled) this.getPage(); });
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