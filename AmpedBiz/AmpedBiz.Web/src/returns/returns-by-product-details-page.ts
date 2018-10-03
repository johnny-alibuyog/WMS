import { Product } from './../common/models/product';
import { RouteConfig, NavigationInstruction } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { ReturnsByProductPageItem } from '../common/models/return';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { Lookup } from '../common/custom_types/lookup';

@autoinject
export class ReturnsByProductDetailsPage {

  public header: string = '';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ReturnsByProductPageItem> = new Pager<ReturnsByProductPageItem>();
  public product: Product;
  public branches: Lookup<string>[] = [];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _notification: NotificationService
  ) {
    this.filter["branch"] = null;
    this.filter["product"] = null;
    this.filter["includeOrderReturns"] = false;
    this.filter.onFilter = () => this.getPage();
    this.sorter["branchName"] = SortDirection.Ascending;
    this.sorter["productName"] = SortDirection.Ascending;
    this.sorter["quantityValue"] = SortDirection.None;
    this.sorter["returnedAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): Promise<void> {
    [this.product, this.branches] = await Promise.all([
      this._api.products.get(params.productId),
      this._api.branches.getLookups()
    ]);
    this.header = ` Returns of ${this.product.name}`
    this.filter["branch"] = params.branchId;
    this.filter["product"] = params.productId;
    this.filter["includeOrderReturns"] = params.includeOrderReturns == "true";

    await this.getPage();
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.returns.getReturnsByProductDetailsPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<ReturnsByProductPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    };
  }
}
