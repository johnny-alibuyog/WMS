import { autoinject } from 'aurelia-framework'
import { Router } from 'aurelia-router';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { ServiceApi } from '../services/service-api';
import { InventoryLevelPageItem } from '../common/models/product';

@autoinject
export class ProuctInventoryLevelPage {
  
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<InventoryLevelPageItem> = new Pager<InventoryLevelPageItem>();

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router

  ) {
    this.filter["code"] = '';
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getPage();
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
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    await this.getPage();
  }

  public async getPage(): Promise<void> {
    let data = await this._api.products.getInventoryLevelPage({
      filter: this.filter,
      sorter: this.sorter,
      pager: <PagerRequest>this.pager
    });
    var response = <PagerResponse<InventoryLevelPageItem>>data;
    this.pager.count = response.count;
    this.pager.items = response.items;
  }

  public view(item: InventoryLevelPageItem): void {
    //console.log("#/orders/order-create?" + buildQueryString({ id: item.id }));
    //this._router.navigate("#/orders/order-create?" + buildQueryString({ id: item.id }));
    this._router.navigateToRoute("product-create", { id: item.id });
  }
}