import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'
import { buildQueryString } from 'aurelia-path';
import { Router } from 'aurelia-router';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { ServiceApi } from '../services/service-api';
import { Dictionary } from '../common/custom_types/dictionary';
import { InventoryLevelPageItem } from '../common/models/product';

@autoinject
export class ProuctInventoryLevelPage {
  private _api: ServiceApi;
  private _router: Router;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<InventoryLevelPageItem>;

  constructor(api: ServiceApi, router: Router) {
    this._api = api;
    this._router = router;

    this.filter = new Filter();
    this.filter["code"] = '';
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["name"] = SortDirection.Ascending;
    this.sorter["supplier"] = SortDirection.None;
    this.sorter["unitOfMeasure"] = SortDirection.None;
    // this.sorter["onHand"] = SortDirection.None;
    // this.sorter["allocated"] = SortDirection.None;
    // this.sorter["available"] = SortDirection.None;
    // this.sorter["onOrder"] = SortDirection.None;
    // this.sorter["currentLevel"] = SortDirection.None;
    // this.sorter["targetLevel"] = SortDirection.None;
    // this.sorter["belowTargetLevel"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<InventoryLevelPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  activate(): void {
    this.getPage();
  }

  getPage(): void {
    this._api.products
      .getInventoryLevelPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<InventoryLevelPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      });
  }

  view(item: InventoryLevelPageItem): void {
    //console.log("#/orders/order-create?" + buildQueryString({ id: item.id }));
    //this._router.navigate("#/orders/order-create?" + buildQueryString({ id: item.id }));
    this._router.navigateToRoute("product-create", { id: item.id });
  }
}