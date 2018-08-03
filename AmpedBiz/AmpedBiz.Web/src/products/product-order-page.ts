import { Filter, Pager, PagerRequest, PagerResponse, SortDirection, Sorter } from '../common/models/paging';
import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'

import { Dictionary } from '../common/custom_types/dictionary';
import { ProductOrderPageItem } from '../common/models/product';
import { Router } from 'aurelia-router';
import { ServiceApi } from '../services/service-api';
import { buildQueryString } from 'aurelia-path';

@autoinject
@customElement("product-order-page")
export class ProductOrderPage {

  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ProductOrderPageItem> = new Pager<ProductOrderPageItem>();

  @bindable()
  public productId: string = '';

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router
  ) {
    this.filter["id"] = this.productId;
    this.filter.onFilter = () => this.getPage();
    this.sorter["orderNumber"] = SortDirection.None;
    this.sorter["createdOn"] = SortDirection.Ascending;
    this.sorter["status"] = SortDirection.None;
    this.sorter["customer"] = SortDirection.None;
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
    let data = await this._api.products.getOrderPage({
      filter: this.filter,
      sorter: this.sorter,
      pager: <PagerRequest>this.pager
    });
    var response = <PagerResponse<ProductOrderPageItem>>data;
    this.pager.count = response.count;
    this.pager.items = response.items;
  }

  view(item: ProductOrderPageItem): void {
    this._router.navigate("#/orders/order-create?" + buildQueryString({ id: item.id }));
  }
}