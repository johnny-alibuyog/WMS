import {Router, RouteConfig, NavigationInstruction} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';
import {ProductCreate} from './product-create';
import {Product, ProductPageItem} from '../common/models/product';
import {ServiceApi} from '../services/service-api';
import {Lookup} from '../common/custom_types/lookup';
import {NotificationService} from '../common/controls/notification-service';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class ProductPage {
  private _api: ServiceApi;
  private _router: Router;
  private _notification: NotificationService;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<ProductPageItem>;

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
    this.sorter["name"] = SortDirection.Ascending;
    this.sorter["descirption"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<ProductPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): any {

    let requests: [Promise<Lookup<string>[]>, Promise<Lookup<string>[]>] = [
      this._api.suppliers.getLookups(),
      this._api.purchaseOrders.getStatusLookup()
    ];

    Promise.all(requests).then((responses: [Lookup<string>[], Lookup<string>[]]) => {
      this.suppliers = responses[0];
      this.categories = responses[1];

      this.getPage();
    });
  }

  getPage(): void {
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

  create() {
    this._router.navigateToRoute('product-create');
    //this._dialog.open({ viewModel: ProductCreate, model: null })
    //  .then(response => { if (!response.wasCancelled) this.getPage(); });
  }

  edit(item: ProductPageItem) {
    this._router.navigateToRoute('product-create', <Product>{ id: item.id });
    //this._dialog.open({ viewModel: ProductCreate, model: <Product>{ id: item.id } })
    //  .then(response => { if (!response.wasCancelled) this.getPage(); });
  }

  delete(item: ProductPageItem) {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}