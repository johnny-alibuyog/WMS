import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {Lookup} from '../common/custom_types/lookup';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';
import {PurchaseOrder, PurchaseOrderPayment} from '../common/models/purchase-order';
import {PurchaseOrderPaidEvent} from '../common/models/purchase-order-event';

@autoinject
export class PurchaseOrderPaymentCreate {
  private _api: ServiceApi;
  private _controller: DialogController;
  private _notification: NotificationService;
  private _purchaseOrderId: string;

  public header: string = 'Payment';
  public canSave: boolean = true;
  public payment: PurchaseOrderPayment;
  public paymentTypes: Lookup<string>[] = [];

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;
  }

  activate(purchaseOrderId: string) {
    let requests: [Promise<Lookup<string>[]>] = [this._api.paymentTypes.getLookups()];

    Promise.all(requests).then((responses: [Lookup<string>[]]) => {
      this.paymentTypes = responses[0];
      this._purchaseOrderId = purchaseOrderId;
      this.payment = <PurchaseOrderPayment>{
        purchaseOrderId: purchaseOrderId,
        paidBy: this._api.auth.userAsLookup,
        paidOn: new Date(),
        paymentType: this.paymentTypes[0] || null,
      };
    });
  }

  cancel() {
    this._controller.cancel();
  }

  save() {
    this.payment.paidOn = new Date();
    var paidEvent = <PurchaseOrderPaidEvent>{
      purchaseOrderId: this._purchaseOrderId,
      payments: [this.payment]
    };

    this._api.purchaseOrders.pay(paidEvent)
      .then(data => {
        this._notification.success("Purchase order has been paid.")
          .then(response => this._controller.ok(data));
      })
      .catch(error => {
        this._notification.warning(error);
      });
  }
}