import { Router, RouteConfig, NavigationInstruction, activationStrategy } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { Order, OrderPageItem, OrderStatus } from '../common/models/order';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class OrderPage {

  public header: string = ' Orders';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<OrderPageItem> = new Pager<OrderPageItem>();
  public statuses: Lookup<OrderStatus>[];
  public customers: Lookup<string>[];
  public users: Lookup<string>[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService
  ) {
    this.filter["status"] = OrderStatus.created;
    this.filter["createdBy"] = null;
    this.filter["customer"] = null;
    this.filter.onFilter = () => this.getPage();
    this.sorter["orderdOn"] = SortDirection.None;
    this.sorter["createdBy"] = SortDirection.Ascending;
    this.sorter["customer"] = SortDirection.None;
    this.sorter["status"] = SortDirection.None;
    this.sorter["paymentOn"] = SortDirection.None;
    this.sorter["taxAmount"] = SortDirection.None;
    this.sorter["shippingFeeAmount"] = SortDirection.None;
    this.sorter["subTotalAmount"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): Promise<void> {
    [this.users, this.customers, this.statuses] = await Promise.all([
      this._api.users.getLookups(),
      this._api.customers.getLookups(),
      this._api.orders.getStatusLookup()
    ]);

    this.filter["status"] = routeConfig.settings.status;
    this.header = routeConfig.title;

    await this.getPage();
  }

  public determineActivationStrategy(): string {
    return activationStrategy.invokeLifecycle;
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.orders.getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      var response = <PagerResponse<OrderPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }

  public create(): void {
    this._router.navigateToRoute('order-create');
  }

  public edit(item: OrderPageItem): void {
    this._router.navigateToRoute('order-create', <Order>{ id: item.id });
  }

  public delete(item: OrderPageItem) {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}
