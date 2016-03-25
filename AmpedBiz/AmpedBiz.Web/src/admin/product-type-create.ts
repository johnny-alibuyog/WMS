import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';

@autoinject
export class ProductCreate {
  private _controller: DialogController;
  header: string = 'Create Type Product';
  productType = {
    id: '',
    name: '',
  };

  constructor(controller: DialogController) {
    this._controller = controller;
  }

  activate(productType) {
    if (productType) {
      this.header = "Edit Product";
      this.productType = JSON.parse(JSON.stringify(productType)); //Object.create(product);
    }
    else {
      this.header = "Create Product";
      this.productType = Object.create(this.productType);
    }

    if (this.productType.id === '')
      this.productType.id = this.guid();
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