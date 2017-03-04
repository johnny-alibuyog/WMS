import { Router, RouteConfig, NavigationInstruction } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { buildQueryString } from 'aurelia-path';
import { Order, OrderPageItem, OrderStatus } from '../common/models/order';
import { Supplier } from '../common/models/supplier';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class ActiveOrderPage {
  private _api: ServiceApi;
  private _router: Router;
  private _notification: NotificationService;

  public header: string = 'Active Orders';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<OrderPageItem>;

  constructor(api: ServiceApi, router: Router, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._notification = notification;

    this.filter = new Filter();
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["orderdOn"] = SortDirection.None;
    this.sorter["createdBy"] = SortDirection.Ascending;
    this.sorter["customer"] = SortDirection.None;
    this.sorter["status"] = SortDirection.None;
    this.sorter["paidOn"] = SortDirection.None;
    this.sorter["taxAmount"] = SortDirection.None;
    this.sorter["shippingFeeAmount"] = SortDirection.None;
    this.sorter["subTotalAmount"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<OrderPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  public activate(): void {
    this.getPage();
  }

  public attached(): void {
    this.getPage();
  }

  private getPage(): void {
    this._api.orders
      .getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<OrderPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  public edit(item: OrderPageItem): void {
    this._router.navigate('#/orders/order-create?' + buildQueryString({ id: item.id }));
  }

  public delete(item: OrderPageItem): void {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}