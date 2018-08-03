import { autoinject } from 'aurelia-framework';
import { DiscontinuedPageItem, Product } from '../common/models/product';
import { Filter, Pager, PagerRequest, PagerResponse, SortDirection, Sorter } from '../common/models/paging';
import { Router } from 'aurelia-router';
import { NotificationService } from '../common/controls/notification-service';
import { ServiceApi } from '../services/service-api';

@autoinject
export class ProductPage {

  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<DiscontinuedPageItem> = new Pager<DiscontinuedPageItem>();

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService
  ) {
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getPage();
    this.sorter["code"] = SortDirection.None;
    this.sorter["name"] = SortDirection.Ascending;
    this.sorter["category"] = SortDirection.None;
    this.sorter["supplier"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    await this.getPage();
  }

  public async getPage(): Promise<void> {
    try {
      let data = await this._api.products.getDiscontinuedPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      var response = <PagerResponse<DiscontinuedPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }

  edit(item: DiscontinuedPageItem) {
    this._router.navigateToRoute('product-create', <Product>{ id: item.id });
  }
}