import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {ProductType} from './common/models/product-type';
import {ProductTypeService} from '../services/product-type-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class ProductTypeCreate {
  private _controller: DialogController;
  private _service: ProductTypeService;

  public header: string = 'Create Type Product';
  public productType: ProductType;
  public notificaton: NotificationService;

  constructor(notification: NotificationService, controller: DialogController, service: ProductTypeService) {
    this.notificaton = notification;
    this._controller = controller;
    this._service = service;
  }

  activate(productType: ProductType) {
    if (productType) {
      this.header = "Edit Product";
      this._service.getProductType(productType.id, {
        success: (data) => {
          this.productType = <ProductType>data
        },
        error: (error) => {
          this.notificaton.warning(error)
        }
      });
    }
    else {
      this.header = "Create Product";
      this.productType = <ProductType>{};
    }
  }

  cancel() {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {
    this._service.createProductType(this.productType, {
      success: (data) => {
        this.notificaton.success("Product Type has been saved.")
          .then((data) => this._controller.ok({ wasCancelled: true, output: <ProductType>data }));
      },
      error: (error) => {
        this.notificaton.warning(error)
      }
    })
  }
}