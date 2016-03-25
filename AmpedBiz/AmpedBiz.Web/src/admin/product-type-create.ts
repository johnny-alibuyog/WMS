import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {ProductType} from './common/models/product-type';
import {ProductTypeService} from '../services/product-type-service';

@autoinject
export class ProductCreate {
  private _controller: DialogController;
  private _service: ProductTypeService;

  public header: string = 'Create Type Product';
  public productType: ProductType;

  constructor(controller: DialogController, service: ProductTypeService) {
    this._controller = controller;
    this._service = service;
  }

  activate(productType: ProductType) {
    if (productType) {
      this.header = "Edit Product";
      this._service.getProductType(productType.id, {
        success: (data) => this.productType = <ProductType>data,
        error: (error) => console.warn(error)
      });
    }
    else {
      this.header = "Create Product";
      this.productType = <ProductType>{};
    }
  }

  cancel() {
    return this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {
    // do some service side call
    return this._controller.ok({ wasCancelled: true, output: this.productType });
  }

  private guid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

}