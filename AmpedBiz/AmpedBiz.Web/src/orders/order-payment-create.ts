/*
import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {Lookup} from '../common/custom_types/lookup';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';
import {Order, OrderPayable, OrderPayment} from '../common/models/order';

@autoinject
export class OrderPaymentCreate {
  private _api: ServiceApi;
  private _controller: DialogController;
  private _notification: NotificationService;
  private _orderId: string;

  public header: string = 'Payment';
  public canSave: boolean = true;
  public payable: OrderPayable;
  public paymentTypes: Lookup<string>[] = [];

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;
  }

  public activate(orderId: string): void {
    let requests: [Promise<Lookup<string>[]>, Promise<OrderPayable>] = [
      this._api.paymentTypes.getLookups(),
      this._api.orders.getPayables(orderId)
    ];

    Promise.all(requests).then((responses: [Lookup<string>[], OrderPayable]) => {
      this._orderId = orderId;
      this.paymentTypes = responses[0];
      this.payable = responses[1];
      this.payable.paidOn = new Date();
      this.payable.paymentAmount = this.payable.balanceAmount;
    });
  }

  public cancel(): void {
    this._controller.cancel();
  }

  public save(): void {
    this._api.orders
      .pay(<Order>{
        id: this._orderId,
        payments: [
          <OrderPayment>{
            paidOn: new Date(),
            paidBy: this._api.auth.userAsLookup,
            paymentType: this.payable.paymentType,
            paymentAmount: this.payable.paymentAmount
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