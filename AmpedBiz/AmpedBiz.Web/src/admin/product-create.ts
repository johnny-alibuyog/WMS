import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {Product} from './common/models/product';
import {Supplier} from './common/models/supplier';
import {ProductCategory} from './common/models/product-category';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class ProductCreate {
  private _api: ServiceApi;
  private _controller: DialogController;
  private _notification: NotificationService;

  public header: string = 'Create Product';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public product: Product;
  public suppliers: Supplier[];
  public categories: ProductCategory[];

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;
  }

  activate(product: Product) {
    
    Promise.all([
      this._api.suppliers.getList()
        .then(data => this.suppliers = <Supplier[]>data)
        .catch(error => this._notification.warning(error)),
        
      this._api.productCategories.getList()
        .then(data => this.categories = <ProductCategory[]>data)
        .catch(error => this._notification.warning(error)),
    ]);
    
    if (product) {
      this.header = "Edit Product";
      this.isEdit = true;
      this._api.products.get(product.id)
        .then(data => this.product = <Product>data)
        .catch(error => this._notification.warning(error));
    }
    else {
      this.header = "Create Product";
      this.isEdit = false;
      this.product = <Product>{};
    }
  }

  cancel() {
    return this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {
    if (this.isEdit) {

      this._api.products.update(this.product)
        .then(data => {
          this._notification.success("Product has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <Product>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
    else {

      this._api.products.create(this.product)
        .then(data => {
          this._notification.success("Product has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <Product>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
  }
}