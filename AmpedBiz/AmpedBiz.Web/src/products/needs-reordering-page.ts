import {Router, RouteConfig, NavigationInstruction} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';
import {ProductCreate} from './product-create';
import {Product, NeedsReorderingPageItem} from '../common/models/product';
import {ServiceApi} from '../services/service-api';
import {Lookup} from '../common/custom_types/lookup';
import {NotificationService} from '../common/controls/notification-service';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class NeedsReorderingPage {
  private _api: ServiceApi;
  private _router: Router;
  private _notification: NotificationService;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<NeedsReorderingPageItem>;

  public suppliers: Lookup<string>[] = [];
  public categories: Lookup<string>[] = [];

  constructor(api: ServiceApi, router: Router, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["code"] = SortDirection.None;
    this.sorter["productName"] = SortDirection.Ascending;
    this.sorter["categoryName"] = SortDirection.None;
    this.sorter["available"] = SortDirection.None;
    this.sorter["currentLevel"] = SortDirection.None;
    this.sorter["targetLevel"] = SortDirection.None;
    this.sorter["belowTarget"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<NeedsReorderingPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  activate(): void {
    this.getPage();
  }

  attached(): void {
    this.getPage();
  }

  getPage(): void {
    this._api.products
      .getNeedsReorderingPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<NeedsReorderingPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  edit(item: NeedsReorderingPageItem) {
    this._router.navigateToRoute('product-create', <Product>{ id: item.id });
    //this._dialog.open({ viewModel: ProductCreate, model: <Product>{ id: item.id } })
    //  .then(response => { if (!response.wasCancelled) this.getPage(); });
  }
}