import {Router, RouteConfig, NavigationInstruction, activationStrategy} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';
import {PurchaseOrderCreate} from './purchase-order-create';
import {PurchaseOrder, PurchaseOrderPageItem, PurchaseOrderStatus} from '../common/models/purchase-order';
import {Supplier} from '../common/models/supplier';
import {ServiceApi} from '../services/service-api';
import {Lookup} from '../common/custom_types/lookup';
import {NotificationService} from '../common/controls/notification-service';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class PurchaseOrderPage {
  private _api: ServiceApi;
  private _router: Router;
  private _notification: NotificationService;

  public header: string = 'Purchase Orders';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<PurchaseOrderPageItem>;

  public statuses: Lookup<PurchaseOrderStatus>[];
  public suppliers: Lookup<string>[];

  constructor(api: ServiceApi, router: Router, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["status"] = PurchaseOrderStatus.new;
    this.filter["supplier"] = null;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["supplier"] = SortDirection.None;
    this.sorter["status"] = SortDirection.Ascending;
    this.sorter["createdBy"] = SortDirection.None;
    this.sorter["createdOn"] = SortDirection.None;
    this.sorter["submittedBy"] = SortDirection.None;
    this.sorter["total"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<PurchaseOrderPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): any {

    let requests: [Promise<Lookup<string>[]>, Promise<Lookup<PurchaseOrderStatus>[]>] = [
      this._api.suppliers.getLookups(),
      this._api.purchaseOrders.getStatusLookup()
    ];

    Promise.all(requests).then((responses: [Lookup<string>[], Lookup<PurchaseOrderStatus>[]]) => {
      this.suppliers = responses[0];
      this.statuses = responses[1];
      this.filter["status"] = routeConfig.settings.status;
      this.header = routeConfig.title;

      this.getPage();
    });
  }

  determineActivationStrategy(): string {
    return activationStrategy.invokeLifecycle;
  }

  /*
  activate() {
    let requests: [Promise<Lookup<string>[]>, Promise<Lookup<PurchaseOrderStatus>[]>] = [
      this._api.suppliers.getLookups(),
      this._api.purchaseOrders.getStatusLookup()
    ];

    Promise.all(requests).then((responses: [Lookup<string>[], Lookup<PurchaseOrderStatus>[]]) => {
      this.suppliers = responses[0];
      this.statuses = responses[1];

      this.getPage();
    });
  }
  */

  getPage(): void {
    this._api.purchaseOrders
      .getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<PurchaseOrderPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  create() {
    this._router.navigateToRoute('purchase-order-create');
  }

  edit(item: PurchaseOrderPageItem) {
    this._router.navigateToRoute('purchase-order-create', <PurchaseOrder>{ id: item.id });
  }

  delete(item: PurchaseOrderPageItem) {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}