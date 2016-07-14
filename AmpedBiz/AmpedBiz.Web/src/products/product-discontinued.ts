import {autoinject} from 'aurelia-framework';
import {Product, ProductPageItem} from '../common/models/product';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class ProductDiscontinued {
  private _api: ServiceApi;
  private _notification: NotificationService;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<ProductPageItem>;

  constructor(api: ServiceApi, notification: NotificationService, filter: Filter, sorter: Sorter, pager: Pager<ProductPageItem>) {
    this._api = api;
    this._notification = notification;

    this.filter = filter;
    this.filter["name"] = '';
    this.filter["discontinued"] = true;
    this.filter.onFilter = () => this.getList();

    this.sorter = sorter;
    this.sorter["code"] = SortDirection.None;
    this.sorter["name"] = SortDirection.None;
    this.sorter["descirption"] = SortDirection.None;
    this.sorter.onSort = () => this.getList();

    this.pager = pager;
    this.pager.onPage = () => this.getList();
  }

  activate() {
    this.getList();
  }

  getList(): void {
    this._api.products
      .getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<ProductPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

}