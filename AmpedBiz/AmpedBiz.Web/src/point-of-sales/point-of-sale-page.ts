import { autoinject } from 'aurelia-framework';
import { Router, RouteConfig, NavigationInstruction, activationStrategy } from 'aurelia-router';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { PointOfSalePageItem, PointOfSaleStatus, PointOfSale } from '../common/models/point-of-sale';

@autoinject
export class PointOfSalePage {

  public header: string = 'Point of Sales';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<PointOfSalePageItem> = new Pager<PointOfSalePageItem>();
  public statuses: Lookup<PointOfSaleStatus>[];
  public customers: Lookup<string>[];
  public users: Lookup<string>[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService
  ) {
    this.filter["status"] = null;
    this.filter["invoiceNumber"] = null;
    this.filter["customer"] = null;
    this.filter.onFilter = () => this.getPage();
    this.sorter["invoiceNumber"] = SortDirection.None;
    this.sorter["status"] = SortDirection.Descending;
    this.sorter["tenderedOn"] = SortDirection.None;
    this.sorter["tenderedBy"] = SortDirection.None;
    this.sorter["customer"] = SortDirection.None;
    this.sorter["discountAmount"] = SortDirection.None;
    this.sorter["subTotalAmount"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter["balanceAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): Promise<void> {
    [this.users, this.customers, this.statuses] = await Promise.all([
      this._api.users.getLookups(),
      this._api.customers.getLookups(),
      this._api.pointOfSales.getStatusLookup()
    ]);
    await this.getPage();
  }

  public async getPage(): Promise<void> {
    try {
      let data = await this._api.pointOfSales.getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      var response = <PagerResponse<PointOfSalePageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }

  public create(): void {
    this._router.navigateToRoute('point-of-sale-create');
  }

  public edit(item: PointOfSalePageItem): void {
    this._router.navigateToRoute('point-of-sale-create', <PointOfSale>{ id: item.id });
  }

  public delete(item: PointOfSalePageItem): void {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}
