import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {ProductCreate} from './product-create';
import {Product} from './common/models/product';
import {ProductService} from '../services/product-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class ProductList {
  private _notification: NotificationService;
  private _service: ProductService;
  private _dialog: DialogService;

  public products: Product[];
  public filterText: string = '';

  constructor(dialog: DialogService, service: ProductService, notification: NotificationService) {
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
    this._service.getProducts(this.filterText)
      .then(data => {
        this.products = <Product[]>data
        if (!this.products || this.products.length == 0) {
          this._notification.warning("No items found!");
        }
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  create() {
    this._dialog
      .open({ viewModel: ProductCreate, model: null })
      .then(response => {
        if (!response.wasCancelled) {
          this.refreshList();
        }
      });
  }

  edit(item: Product) {
    this._dialog
      .open({ viewModel: ProductCreate, model: item })
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