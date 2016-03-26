import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {ProductCategory} from './common/models/product-category';
import {ProductCategoryService} from '../services/product-category-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class ProductCategoryCreate {
  private _controller: DialogController;
  private _service: ProductCategoryService;

  public header: string = 'Create Type Product';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public productCategory: ProductCategory;
  public notificaton: NotificationService;

  constructor(notification: NotificationService, controller: DialogController, service: ProductCategoryService) {
    this.notificaton = notification;
    this._controller = controller;
    this._service = service;
  }

  activate(productCategory: ProductCategory) {
    if (productCategory) {
      this.header = "Edit Product";
      this.isEdit = true;
      this._service.getProductCategory(productCategory.id, {
        success: (data) => {
          this.productCategory = <ProductCategory>data;
        },
        error: (error) => {
          this.notificaton.warning(error);
        }
      });
    }
    else {
      this.header = "Create Product";
      this.isEdit = false;
      this.productCategory = <ProductCategory>{};
    }
  }

  cancel() {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {

    if (this.isEdit) {

      this._service.updateProductCategory(this.productCategory, {
        success: (data) => {
          this.notificaton.success("Product Type has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <ProductCategory>data }));
        },
        error: (error) => {
          this.notificaton.warning(error)
        }
      })
    }
    else {

      this._service.createProductCategory(this.productCategory, {
        success: (data) => {
          this.notificaton.success("Product Type has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <ProductCategory>data }));
        },
        error: (error) => {
          this.notificaton.warning(error)
        }
      })

    }
  }
}