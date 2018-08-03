import { Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { buildQueryString } from 'aurelia-path';
import { OrderPageItem } from '../common/models/order';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, SortDirection, PagerResponse } from '../common/models/paging';

@autoinject
export class ActiveOrderPage {

  public header: string = 'Active Customer Orders';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<OrderPageItem> = new Pager<OrderPageItem>();

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService
  ) {
    this.filter.onFilter = () => this.getPage();
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
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    await this.getPage();
  }

  public async attached(): Promise<void> {
    await this.getPage();
  }

  private async getPage(): Promise<void> {
    try {
      let response = await this._api.orders.getActiveOrderPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      this.pager.count = response.count;
      this.pager.items = response.items;
    } 
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }

  public edit(item: OrderPageItem): void {
    this._router.navigate('#/orders/order-create?' + buildQueryString({ id: item.id }));
  }

  public delete(): void {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}