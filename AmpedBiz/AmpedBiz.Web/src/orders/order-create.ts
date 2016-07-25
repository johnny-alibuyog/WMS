import {Router} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';
import {EventAggregator} from 'aurelia-event-aggregator';
import {DialogService} from 'aurelia-dialog';
import {Order, OrderStatus} from '../common/models/order';
import {ServiceApi} from '../services/service-api';
import {Lookup} from '../common/custom_types/lookup';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class OrderCreate {
  private _api: ServiceApi;
  private _router: Router;
  private _dialog: DialogService;
  private _notification: NotificationService;
  private _eventAggegator: EventAggregator;

  public header: string = 'Order';
  public isEdit: boolean = false;
  public canSave: boolean = true;

  public products: Lookup<string>[] = [];
  public branches: Lookup<string>[] = [];
  public customers: Lookup<string>[] = [];
  public pricingSchemes: Lookup<string>[] = [];
  public statuses: Lookup<OrderStatus>[] = [];
  public order: Order;

  constructor(api: ServiceApi, router: Router, dialog: DialogService, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._router = router;
    this._dialog = dialog;
    this._notification = notification;
    this._eventAggegator = eventAggregator;
  }

  activate(order: Order) {
    if (order.id) {
      this.isEdit = true;
    }
    else {
      this.isEdit = false;
    }

    let requests: [
      Promise<Lookup<string>[]>,
      Promise<Lookup<string>[]>,
      Promise<Lookup<string>[]>,
      Promise<Lookup<string>[]>,
      Promise<Lookup<OrderStatus>[]>,
      Promise<Order>] = [
        this._api.products.getLookups(),
        this._api.branches.getLookups(),
        this._api.customers.getLookups(),
        this._api.pricingSchemes.getLookups(),
        this._api.orders.getStatusLookup(),
        (order.id) ? this._api.orders.get(order.id) : Promise.resolve(<Order>{})
      ];

    Promise.all(requests).then((responses: [
      Lookup<string>[],
      Lookup<string>[],
      Lookup<string>[],
      Lookup<string>[],
      Lookup<OrderStatus>[],
      Order
    ]) => {
      this.products = responses[0];
      this.branches = responses[1];
      this.customers = responses[2];
      this.pricingSchemes = responses[3];
      this.statuses = responses[4];
      this.order = responses[5];
    });
  }

  resetAndNoify(order: Order, notificationMessage: string) {
    this.setOrder(order);

    if (notificationMessage) {
      this._notification.success(notificationMessage);
    }
  }

  setOrder(order: Order): void {
    this.order = order;
    this.order.items = this.order.items || [];
    this.order.payments = this.order.payments || [];
  }

  addItem(): void {
    this._eventAggegator.publish('addOrderItem');
  }

  addPayment(): void {
    this._eventAggegator.publish('addOrderPayment');
  }

  save(): void {
    if (this.isEdit) {
      this.updateNew();
    }
    else {
      this.createNew();
    }
  }

  createNew(): void {
    this._api.orders.createNew(this.order)
      .then(data => this.resetAndNoify(data, "Order has been created."))
      .catch(error => this._notification.warning(error));
  }

  updateNew(): void {
    this._api.orders.updateNew(this.order)
      .then(data => this.resetAndNoify(data, "Order has been updated."))
      .catch(error => this._notification.warning(error));
  }

  stage(): void {
    this._api.orders.stage(this.order)
      .then(data => this.resetAndNoify(data, "Order has been staged."))
      .catch(error => this._notification.warning(error));
  }

  route(): void {
    this._api.orders.route(this.order)
      .then(data => this.resetAndNoify(data, "Order has been routed."))
      .catch(error => this._notification.warning(error));
  }

  invoice(): void {
    this._api.orders.invoice(this.order)
      .then(data => this.resetAndNoify(data, "Order has been updated."))
      .catch(error => this._notification.warning(error));
  }

  /*
    pay(): void {
      this._api.orders.pay(this.order)
        .then(data => this.resetAndNoify(data, "Order has been paid."))
        .catch(error => this._notification.warning(error));
    }
  */

  ship(): void {
    this._api.orders.ship(this.order)
      .then(data => this.resetAndNoify(data, "Order has been shiped."))
      .catch(error => this._notification.warning(error));
  }

  complete(): void {
    this._api.orders.complete(this.order)
      .then(data => this.resetAndNoify(data, "Order has been completed."))
      .catch(error => this._notification.warning(error));
  }

  cancel(): void {
    this._api.orders.cancel(this.order)
      .then(data => this.resetAndNoify(data, "Order has been cancelled."))
      .catch(error => this._notification.warning(error));
  }

  back(): void {
    this._router.navigateBack();
  }
}