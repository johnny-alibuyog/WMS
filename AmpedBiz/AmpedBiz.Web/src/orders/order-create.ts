import {Router} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';
import {EventAggregator, Subscription} from 'aurelia-event-aggregator';
import {Dictionary} from '../common/custom_types/dictionary';
import {Order, OrderStatus, orderEvents} from '../common/models/order';
import {ServiceApi} from '../services/service-api';
import {Lookup} from '../common/custom_types/lookup';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class OrderCreate {
  private _api: ServiceApi;
  private _router: Router;
  private _notification: NotificationService;
  private _eventAggregator: EventAggregator;
  private _subscriptions: Subscription[] = [];

  public header: string = 'Order';
  public isEdit: boolean = false;
  public canSave: boolean = true;

  public products: Lookup<string>[] = [];
  public branches: Lookup<string>[] = [];
  public customers: Lookup<string>[] = [];
  public pricingSchemes: Lookup<string>[] = [];
  public statuses: Lookup<OrderStatus>[] = [];
  public order: Order;

  constructor(api: ServiceApi, router: Router, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._router = router;
    this._notification = notification;
    this._eventAggregator = eventAggregator;
  }

  getInitializedOrder(): Order {
    let order: Order = {
      orderedOn: new Date(),
      allowedTransitions: <Dictionary<string>>{}
    };
    order.allowedTransitions[OrderStatus[OrderStatus.new]] = "Save";
    return order;
  }

  activate(order: Order) {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        orderEvents.payment.paid,
        data => this.resetAndNoify(data, null)
      ),
    ];

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
        this.isEdit
          ? this._api.orders.get(order.id)
          : Promise.resolve(this.getInitializedOrder())
      ];

    Promise.all(requests)
      .then(responses => {
        this.products = responses[0];
        this.branches = responses[1];
        this.customers = responses[2];
        this.pricingSchemes = responses[3];
        this.statuses = responses[4];
        this.order = responses[5];

        if (!this.order.branch) {
          this.order.branch = this.branches
            .find(x => x.id == this._api.auth.user.branchId);
        }

      })
      .catch(error => {
        this._notification.error(error);
      });
  }

  deactivate() {
    this._subscriptions.forEach(subscription => subscription.dispose());
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
    this._eventAggregator.publish(orderEvents.item.add);
  }

  addPayment(): void {
    this._eventAggregator.publish(orderEvents.payment.pay);
  }

  signalPricingSchemChanged(): void {
    this._eventAggregator.publish(orderEvents.pricingScheme.changed);
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
      .then(data => this.resetAndNoify(data, "Order has been invoiced."))
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