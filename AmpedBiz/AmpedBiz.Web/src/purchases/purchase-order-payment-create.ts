/*
import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { Lookup } from '../common/custom_types/lookup';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { PurchaseOrder, PurchaseOrderPayment, PurchaseOrderPayable } from '../common/models/purchase-order';

@autoinject
export class PurchaseOrderPaymentCreate {
  private _api: ServiceApi;
  private _controller: DialogController;
  private _notification: NotificationService;
  private _purchaseOrderId: string;

  public header: string = 'Payment';
  public canSave: boolean = true;
  public payable: PurchaseOrderPayable;
  public paymentTypes: Lookup<string>[] = [];

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;
  }

  activate(purchaseOrderId: string) {
    let requests: [Promise<Lookup<string>[]>, Promise<PurchaseOrderPayable>] = [
      this._api.paymentTypes.getLookups(),
      this._api.purchaseOrders.getPayables(purchaseOrderId)
    ];

    Promise.all(requests).then((responses: [Lookup<string>[], PurchaseOrderPayable]) => {
      this._purchaseOrderId = purchaseOrderId;
      this.paymentTypes = responses[0];
      this.payable = responses[1];
      this.payable.paidOn = new Date();
      this.payable.paymentAmount = this.payable.balanceAmount;
    });
  }

  cancel() {
    this._controller.cancel();
  }

  save() {
    this._api.purchaseOrders
      .pay(<PurchaseOrder>{
        id: this._purchaseOrderId,
        payments: [
          <PurchaseOrderPayment>{
            paidBy: this._api.auth.userAsLookup,
            paidOn: new Date,
            paymentAmount: this.payable.paymentAmount,
            paymentType: this.payable.paymentType,
          }
        ]
      })
      .then(data => {
        this._notification.success("Purchase order has been paid.")
          .then(response => this._controller.ok(data));
      })
      .catch(error => {
        this._notification.warning(error);
      });
  }
}
*/