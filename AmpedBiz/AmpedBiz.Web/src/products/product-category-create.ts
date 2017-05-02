import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { ProductCategory } from '../common/models/product-category';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';

@autoinject
export class ProductCategoryCreate {
  private _api: ServiceApi;
  private _controller: DialogController;
  private _notification: NotificationService;

  public header: string = 'Create Product Category';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public productCategory: ProductCategory;

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;
  }

  activate(productCategory: ProductCategory) {
    if (productCategory) {
      this.header = "Edit Product Category";
      this.isEdit = true;
      this._api.productCategories.get(productCategory.id)
        .then(data => this.productCategory = <ProductCategory>data)
        .catch(error => this._notification.warning(error));
    }
    else {
      this.header = "Create Product Category";
      this.isEdit = false;
      this.productCategory = <ProductCategory>{};
    }
  }

  cancel() {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {

    if (this.isEdit) {

      this._api.productCategories.update(this.productCategory)
        .then(data => {
          this._notification.success("Product Category has been saved.")
            .whenClosed((data) => this._controller.ok(<ProductCategory>data));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
    else {

      this._api.productCategories.create(this.productCategory)
        .then(data => {
          this._notification.success("Product Category has been saved.")
            .whenClosed((data) => this._controller.ok(<ProductCategory>data));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
  }
}