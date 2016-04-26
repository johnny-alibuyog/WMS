import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {ProductCreate} from './product-create';
import {Product, ProductPageItem} from './common/models/product';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class ProductList {
  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<ProductPageItem>;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, filter: Filter, sorter: Sorter, pager: Pager<ProductPageItem>) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;

    this.filter = filter;
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getList();

    this.sorter = sorter;
    this.sorter["code"] = SortDirection.None;
    this.sorter["name"] = SortDirection.Ascending;
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
      .getPages({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<ProductPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;

        if (this.pager.count === 0) {
          this._notification.warning("No items found!");
        }
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  create() {
    this._dialog
      .open({ 
        viewModel: ProductCreate, 
        model: null 
      })
      .then(response => {
        if (!response.wasCancelled) {
          this.getList();
        }
      });
  }

  edit(item: ProductPageItem) {
    this._dialog
      .open({ 
        viewModel: ProductCreate, 
        model: <Product>{ id: item.id }  
      })
      .then(response => {
        if (!response.wasCancelled) {
          this.getList();
        }
      });
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