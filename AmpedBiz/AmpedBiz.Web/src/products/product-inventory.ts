import {autoinject, bindable, bindingMode, customElement} from 'aurelia-framework'
import {Router} from 'aurelia-router';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';
import {ServiceApi} from '../services/service-api';
import {Dictionary} from '../common/custom_types/dictionary';
import {Lookup} from '../common/custom_types/lookup';
import {Inventory} from '../common/models/inventory';

@autoinject
@customElement("product-inventory")
export class ProductInventory {
  private _api: ServiceApi;
  private _router: Router;

  public filter: Filter;
  public sorter: Sorter;
  //public pager: Pager<ProductOrderPageItem>;

  public unitOfMeasures: Lookup<string>[];

  @bindable()
  public productId: string = '';

  @bindable({defaultBindingMode: bindingMode.twoWay})
  public inventory: Inventory;


  constructor(api: ServiceApi, router: Router) {
    this._api = api;
    this._router = router;

    this.filter = new Filter();
    this.filter["id"] = this.productId;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["orderNumber"] = SortDirection.None;
    this.sorter["createdOn"] = SortDirection.Ascending;
    this.sorter["status"] = SortDirection.None;
    this.sorter["customer"] = SortDirection.None;
    this.sorter["quantity"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    //this.pager = new Pager<ProductOrderPageItem>();
    //this.pager.onPage = () => this.getPage();
  }

  attached() {
    var requests: [Promise<Lookup<string>[]>] = [this._api.unitOfMeasures.getLookups()];

    Promise.all(requests).then(data => {
      this.unitOfMeasures = data[0];
    });
  }

  productIdChanged(): void {
    this.filter["id"] = this.productId;
    this.getPage();
  }

  getPage(): void {
    if (!this.filter["id"]) {
      return;
    }

    /*
    this._api.products
      .getOrderPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<ProductOrderPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      });
    */
  }
}