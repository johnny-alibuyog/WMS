import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';

@autoinject
export class ProductCreate {
  private _controller: DialogController;
  
  product = { 
    id: '', 
    productCode: '', 
    productName: '', 
    category: '', 
    supplier: '', 
    standardCost: "₱0.00", 
    listPrice: "₱0.00" 
  };

  constructor(controller: DialogController) {
    this._controller = controller;
  }

  activate(product) {
    this.product = product;
  }

  cancel() {
    return this._controller.cancel({ wasCancelled: true, output: null });
  }
  create() {
    return this._controller.ok({ wasCancelled: true, output: this.product });
  }
}