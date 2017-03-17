import { Router, RouteConfig, NavigationInstruction } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { CustomerCreate } from './customer-create';
import { Customer, CustomerPageItem } from '../common/models/customer';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class CustomerPage {
  private _api: ServiceApi;
  private _router: Router;
  private _notification: NotificationService;

  public header: string = ' Customers';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<CustomerPageItem>;

  constructor(api: ServiceApi, router: Router, notification: NotificationService, filter: Filter, sorter: Sorter, pager: Pager<CustomerPageItem>) {
    this._api = api;
    this._router = router;
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
    this._router.navigateToRoute('customer-create');
  }

  public edit(item: CustomerPageItem): void {
    this._router.navigateToRoute('customer-create', <Customer>{ id: item.id });
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