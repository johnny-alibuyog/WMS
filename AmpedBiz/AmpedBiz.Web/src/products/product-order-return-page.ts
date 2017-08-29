import { Filter, Pager, PagerRequest, PagerResponse, SortDirection, Sorter } from '../common/models/paging';
import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'

import { Dictionary } from '../common/custom_types/dictionary';
import { ProductOrderReturnPageItem } from '../common/models/product';
import { Router } from 'aurelia-router';
import { ServiceApi } from '../services/service-api';
import { buildQueryString } from 'aurelia-path';

@autoinject
@customElement("product-order-return-page")
export class ProductOrderReturnPage {
  private _api: ServiceApi;
  private _router: Router;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<ProductOrderReturnPageItem>;

  @bindable()
  public productId: string = '';

  constructor(api: ServiceApi, router: Router) {
    this._api = api;
    this._router = router;

    this.filter = new Filter();
    this.filter["id"] = this.productId;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["reason"] = SortDirection.None;
    this.sorter["returnedOn"] = SortDirection.Ascending;
    this.sorter["returnedBy"] = SortDirection.None;
    this.sorter["returned"] = SortDirection.None;
    this.sorter["quantityUnit"] = SortDirection.None;
    this.sorter["quantityValue"] = SortDirection.None;
    this.sorter["unit"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<ProductOrderReturnPageItem>();
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
      .getOrderReturnPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<ProductOrderReturnPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      });
  }

  view(item: ProductOrderReturnPageItem): void {
    //this._router.navigateToRoute("purchase-order-view", { id: item.id });
    this._router.navigate("#/orders/order-create?" + buildQueryString({ id: item.id }));
  }
}