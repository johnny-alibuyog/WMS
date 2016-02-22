import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';

@autoinject
export class ProductCreate {
  private _controller: DialogController;
  header: string = 'Create Product';
  product = {
    id: '',
    code: '',
    name: '',
    category: '',
    supplier: '',
    standardCost: '₱0.00',
    listPrice: '₱0.00'
  };

  constructor(controller: DialogController) {
    this._controller = controller;
  }

  activate(product) {
    if (product) {
      this.header = "Edit Product";
      this.product = JSON.parse(JSON.stringify(product)); //Object.create(product);
    }
    else {
      this.header = "Create Product";
      this.product = Object.create(this.product);
    }

    if (this.product.id === '')
      this.product.id = this.guid();
  }

  cancel() {
    return this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {
    // do some service side call
    return this._controller.ok({ wasCancelled: true, output: this.product });
  }

  private guid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

}