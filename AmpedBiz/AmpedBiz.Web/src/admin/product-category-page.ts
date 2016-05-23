import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {ProductCategoryCreate} from './product-category-create';
import {ProductCategory, ProductCategoryPageItem} from './common/models/product-category';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class ProductCategoryPage {
  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<ProductCategoryPageItem>;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, filter: Filter, sorter: Sorter, pager: Pager<ProductCategoryPageItem>) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;

    this.filter = filter;
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getPage();

    this.sorter = sorter;
    this.sorter["code"] = SortDirection.None;
    this.sorter["name"] = SortDirection.Ascending;
    this.sorter.onSort = () => this.getPage();

    this.pager = pager;
    this.pager.onPage = () => this.getPage();
  }

  activate() {
    this.getPage();
  }

  getPage() {
    this._api.productCategories
      .getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<ProductCategoryPageItem>>data;
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
        viewModel: ProductCategoryCreate, 
        model: null 
      })
      .then(response => {
        if (!response.wasCancelled) {
          this.getPage();
        }
      });
  }

  edit(item: ProductCategoryPageItem) {
    this._dialog
      .open({ 
        viewModel: ProductCategoryCreate, 
        model: <ProductCategory>{ id: item.id }  
      })
      .then(response => {
        if (!response.wasCancelled) {
          this.getPage();
        }
      });
  }

  delete(item: any) {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}