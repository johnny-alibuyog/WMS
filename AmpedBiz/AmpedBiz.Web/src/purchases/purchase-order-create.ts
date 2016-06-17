import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {PurchaseOrder, PurchaseOrderDetail, RecievingDetail, PaymentDetail} from './common/models/purchase-order';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class PurchaseOrderCreate {
  private _api: ServiceApi;
  private _controller: DialogController;
  private _notification: NotificationService;

  public header: string = 'Purchase Order';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public purchaseOrder: PurchaseOrder;

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;
  }

  activate(purchaseOrder: PurchaseOrder) {
    if (purchaseOrder) {
      this.isEdit = true;
      this._api.purchaseOrders.get(purchaseOrder.id)
        .then(data => this.purchaseOrder = <PurchaseOrder>data)
        .catch(error => this._notification.warning(error));
    }
    else {
      this.isEdit = false;
      this.purchaseOrder = <PurchaseOrder>{};
    }
  }

  close() {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {
    if (this.isEdit) {

      this._api.purchaseOrders.update(this.purchaseOrder)
        .then(data => {
          this._notification.success("Purchase order has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <PurchaseOrder>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
    else {

      this._api.purchaseOrders.create(this.purchaseOrder)
        .then(data => {
          this._notification.success("Purchase order has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <PurchaseOrder>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
  }

  submit() : void {

  }

  approve() : void {

  }

  reject() : void {

  }

  pay() : void {

  }

  recieve() : void {

  }

  complete() : void {

  }

  cancel() : void {

  }
}