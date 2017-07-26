import { Router, RouteConfig, NavigationInstruction, activationStrategy } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { Order, OrderPageItem, OrderStatus } from '../common/models/order';
import { Supplier } from '../common/models/supplier';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class OrderPage {
  private _api: ServiceApi;
  private _router: Router;
  private _notification: NotificationService;

  public header: string = ' Orders';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<OrderPageItem>;

  public statuses: Lookup<OrderStatus>[];
  public customers: Lookup<string>[];
  public users: Lookup<string>[];

  constructor(api: ServiceApi, router: Router, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["status"] = OrderStatus.new;
    this.filter["createdBy"] = null;
    this.filter["customer"] = null;
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

  public activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): any {

    let requests: [Promise<Lookup<string>[]>, Promise<Lookup<string>[]>, Promise<Lookup<OrderStatus>[]>] = [
      this._api.users.getLookups(),
      this._api.customers.getLookups(),
      this._api.orders.getStatusLookup()
    ];

    Promise.all(requests).then((responses: [Lookup<string>[], Lookup<string>[], Lookup<OrderStatus>[]]) => {
      this.users = responses[0];
      this.customers = responses[1];
      this.statuses = responses[2];
      this.filter["status"] = routeConfig.settings.status;
      this.header = routeConfig.title;

      this.getPage();
    });
  }

  public determineActivationStrategy(): string {
    return activationStrategy.invokeLifecycle;
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