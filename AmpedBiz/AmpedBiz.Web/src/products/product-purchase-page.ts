import { Filter, Pager, PagerRequest, PagerResponse, SortDirection, Sorter } from '../common/models/paging';
import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'

import { Dictionary } from '../common/custom_types/dictionary';
import { ProductPurchasePageItem } from '../common/models/product';
import { Router } from 'aurelia-router';
import { ServiceApi } from '../services/service-api';
import { buildQueryString } from 'aurelia-path';

@autoinject
@customElement("product-purchase-page")
export class ProductPurchasePage {
  private _api: ServiceApi;
  private _router: Router;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<ProductPurchasePageItem>;

  @bindable()
  public productId: string = '';

  constructor(api: ServiceApi, router: Router) {
    this._api = api;
    this._router = router;

    this.filter = new Filter();
    this.filter["id"] = this.productId;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["purchaseOrderNumber"] = SortDirection.None;
    this.sorter["createdOn"] = SortDirection.Ascending;
    this.sorter["status"] = SortDirection.None;
    this.sorter["supplier"] = SortDirection.None;
    this.sorter["unitCost"] = SortDirection.None;
    this.sorter["quantityUnit"] = SortDirection.None;
    this.sorter["quantityValue"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<ProductPurchasePageItem>();
    this.pager.onPage = () => this.getPage();
  }

  productIdChanged(): void {
    this.filter["id"] = this.productId;
    this.getPage();
  }

  getPage(): void {
    if (!this.filter["id"]) {
      return;
    }

    this._api.products
      .getPurchasePage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<ProductPurchasePageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      });
  }

  view(item: ProductPurchasePageItem): void {
    //this._router.navigateToRoute("purchase-order-view", { id: item.id });
    this._router.navigate("#/purchases/purchase-order-create?" + buildQueryString({ id: item.id }));
  }
}