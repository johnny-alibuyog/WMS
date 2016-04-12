import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {ProductCategoryCreate} from './product-category-create';
import {ProductCategory} from './common/models/product-category';
import {ProductCategoryService} from '../services/product-category-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class ProductCategoryList {
  private _notification: NotificationService;
  private _service: ProductCategoryService;
  private _dialog: DialogService;

  public productCategories: ProductCategory[];
  public filterText: string = '';

  constructor(dialog: DialogService, service: ProductCategoryService, notification: NotificationService) {
    this._dialog = dialog;
    this._service = service;
    this._notification = notification;
  }

  activate() {
    this.refreshList();
  }

  refreshList() {
    this.filterText = '';
    this.filter();
  }

  filter() {
    this._service.getProductCategories(this.filterText)
      .then(data => {
        this.productCategories = <ProductCategory[]>data
        if (!this.productCategories || this.productCategories.length == 0) {
          this._notification.warning("No items found!");
        }
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  create() {
    this._dialog
      .open({ viewModel: ProductCategoryCreate, model: null })
      .then(response => {
        if (!response.wasCancelled) {
          this.refreshList();
        }
      });
  }

  edit(item: ProductCategory) {
    this._dialog
      .open({ viewModel: ProductCategoryCreate, model: item })
      .then(response => {
        if (!response.wasCancelled) {
          this.refreshList();
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