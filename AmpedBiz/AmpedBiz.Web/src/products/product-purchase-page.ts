import { buildQueryString } from 'aurelia-path';
import { autoinject, bindable, customElement } from 'aurelia-framework'
import { Filter, Pager, PagerRequest, PagerResponse, SortDirection, Sorter } from '../common/models/paging';
import { ProductPurchasePageItem } from '../common/models/product';
import { Router } from 'aurelia-router';
import { ServiceApi } from '../services/service-api';

@autoinject
@customElement("product-purchase-page")
export class ProductPurchasePage {

  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ProductPurchasePageItem> = new Pager<ProductPurchasePageItem>();

  @bindable()
  public productId: string = '';

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router
  ) {
    this.filter["id"] = this.productId;
    this.filter.onFilter = () => this.getPage();
    this.sorter["purchaseOrderNumber"] = SortDirection.None;
    this.sorter["createdOn"] = SortDirection.Ascending;
    this.sorter["status"] = SortDirection.None;
    this.sorter["supplier"] = SortDirection.None;
    this.sorter["unitCost"] = SortDirection.None;
    this.sorter["quantityUnit"] = SortDirection.None;
    this.sorter["quantityValue"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public productIdChanged(): void {
    this.filter["id"] = this.productId;
    this.getPage();
  }

  public async getPage(): Promise<void> {
    if (!this.filter["id"]) {
      return;
    }
    let data = await this._api.products.getPurchasePage({
      filter: this.filter,
      sorter: this.sorter,
      pager: <PagerRequest>this.pager
    });
    var response = <PagerResponse<ProductPurchasePageItem>>data;
    this.pager.count = response.count;
    this.pager.items = response.items;
  }

  public view(item: ProductPurchasePageItem): void {
    //this._router.navigateToRoute("purchase-order-view", { id: item.id });
    this._router.navigate("#/purchases/purchase-order-create?" + buildQueryString({ id: item.id }));
  }
}