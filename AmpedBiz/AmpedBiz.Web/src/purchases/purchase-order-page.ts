import { autoinject } from 'aurelia-framework';
import { Router, RouteConfig, NavigationInstruction, activationStrategy } from 'aurelia-router';
import { PurchaseOrder, PurchaseOrderPageItem, PurchaseOrderStatus } from '../common/models/purchase-order';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class PurchaseOrderPage {

  public header: string = 'Purchase Orders';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<PurchaseOrderPageItem> = new Pager<PurchaseOrderPageItem>();
  public statuses: Lookup<PurchaseOrderStatus>[];
  public suppliers: Lookup<string>[];
  public users: Lookup<string>[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService
  ) {
    this.filter["status"] = PurchaseOrderStatus.created;
    this.filter["createdBy"] = null;
    this.filter["supplier"] = null;
    this.filter.onFilter = () => this.getPage();
    this.sorter["supplier"] = SortDirection.None;
    this.sorter["status"] = SortDirection.Ascending;
    this.sorter["createdBy"] = SortDirection.None;
    this.sorter["createdOn"] = SortDirection.None;
    this.sorter["submittedBy"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): Promise<void> {
    [this.users, this.suppliers, this.statuses] = await Promise.all([
      this._api.users.getLookups(),
      this._api.suppliers.getLookups(),
      this._api.purchaseOrders.getStatusLookup()
    ]);
    this.filter["status"] = routeConfig.settings.status;
    this.header = routeConfig.title;
    await this.getPage();
  }

  public determineActivationStrategy(): string {
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

  public async getPage(): Promise<void> {
    try {
      let data = await this._api.purchaseOrders.getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      var response = <PagerResponse<PurchaseOrderPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }

  public create(): void {
    this._router.navigateToRoute('purchase-order-create');
  }

  public edit(item: PurchaseOrderPageItem): void {
    this._router.navigateToRoute('purchase-order-create', <PurchaseOrder>{ id: item.id });
  }

  public delete(item: PurchaseOrderPageItem): void {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}