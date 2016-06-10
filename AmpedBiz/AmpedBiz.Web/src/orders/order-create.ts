import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';

@autoinject
export class OrderCreate {
  private _controller: DialogController;
  header: string = 'Create Order';
  purchaseOrder = {
    id: ''
  };

  constructor(controller: DialogController) {
    this._controller = controller;
  }

activate(purchaseOrder) {
    if (purchaseOrder) {
      this.header = "Edit Order";
    }
    else {
      this.header = "Create Order";
    }
  }

cancel() {
    return this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {
    // do some service side call
    return this._controller.ok({ wasCancelled: true, output: this.purchaseOrder });
  }
}