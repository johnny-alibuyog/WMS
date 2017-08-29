import { DiscontinuedPageItem, Product } from '../common/models/product';
import { Filter, Pager, PagerRequest, PagerResponse, SortDirection, Sorter } from '../common/models/paging';
import { NavigationInstruction, RouteConfig, Router } from 'aurelia-router';

import { NotificationService } from '../common/controls/notification-service';
import { ProductCreate } from './product-create';
import { ServiceApi } from '../services/service-api';
import { autoinject } from 'aurelia-framework';

@autoinject
export class ProductPage {
  private _api: ServiceApi;
  private _router: Router;
  private _notification: NotificationService;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<DiscontinuedPageItem>;

  constructor(api: ServiceApi, router: Router, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["code"] = SortDirection.None;
    this.sorter["name"] = SortDirection.Ascending;
    this.sorter["category"] = SortDirection.None;
    this.sorter["supplier"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<DiscontinuedPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  activate(): void {
    this.getPage();
  }

  getPage(): void {
    this._api.products
      .getDiscontinuedPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<DiscontinuedPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  edit(item: DiscontinuedPageItem) {
    this._router.navigateToRoute('product-create', <Product>{ id: item.id });
  }
}