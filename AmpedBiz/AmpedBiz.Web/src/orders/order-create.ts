import {Router} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';
import {EventAggregator, Subscription} from 'aurelia-event-aggregator';
import {Dictionary} from '../common/custom_types/dictionary';
import {Lookup} from '../common/custom_types/lookup';
import {Order, OrderStatus, orderEvents} from '../common/models/order';
import {ServiceApi} from '../services/service-api';
import {ReportViewer} from '../common/controls/report-viewer';
import {NotificationService} from '../common/controls/notification-service';
import {OrderInvoiceDetailReport} from './order-invoice-detail-report';

@autoinject
export class OrderCreate {
  private readonly _api: ServiceApi;
  private readonly _router: Router;
  private readonly _notification: NotificationService;
  private readonly _eventAggregator: EventAggregator;
  private readonly _invoiceReport: OrderInvoiceDetailReport;

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

  constructor(api: ServiceApi, router: Router, notification: NotificationService, eventAggregator: EventAggregator, invoiceReport: OrderInvoiceDetailReport) {
    this._api = api;
    this._router = router;
    this._notification = notification;
    this._eventAggregator = eventAggregator;
    this._invoiceReport = invoiceReport;
  }

  public getInitializedOrder(): Order {
    let order: Order = {
      orderedOn: new Date(),
      allowedTransitions: <Dictionary<string>>{}
    };
    order.allowedTransitions[OrderStatus[OrderStatus.new]] = "Save";
    return order;
  }

  public get isOrderInvoiced(): boolean {
    return this.order && this.order.status >= OrderStatus.invoiced;
  }

  public activate(order: Order): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        orderEvents.payment.paid,
        data => this.resetAndNoify(data, null)
      ),
      this._eventAggregator.subscribe(
        orderEvents.invoiceDetail.show,
        data => window.open(data, "_blank")
      )
    ];

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
        order.id
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
        this.setOrder(responses[5]);

        if (!this.order.branch) {
          this.order.branch = this.branches
            .find(x => x.id == this._api.auth.user.branchId);
        }

      })
      .catch(error => {
        this._notification.error(error);
      });
  }

  public deactivate(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  public resetAndNoify(order: Order, notificationMessage: string): void {
    this.setOrder(order);

    if (notificationMessage) {
      this._notification.success(notificationMessage);
    }
  }

  public setOrder(order: Order): void {
    if (order.id) {
      this.isEdit = true;
    }
    else {
      this.isEdit = false;
    }

    this.order = order;
    this.order.items = this.order.items || [];
    this.order.returns = this.order.returns || [];
    this.order.payments = this.order.payments || [];
  }

  public addItem(): void {
    this._eventAggregator.publish(orderEvents.item.add);
  }

  public addReturn(): void {
    this._eventAggregator.publish(orderEvents.return.add);
  }

  public addPayment(): void {
    this._eventAggregator.publish(orderEvents.payment.pay);
  }

  public signalPricingSchemChanged(): void {
    this._eventAggregator.publish(orderEvents.pricingScheme.changed);
  }

  public save(): void {
    if (this.isEdit) {
      this.updateNew();
    }
    else {
      this.createNew();
    }
  }

  public createNew(): void {
    this._api.orders.createNew(this.order)
      .then(data => {
        this._notification.success("Order has been created.");
        this._router.navigateBack();
      })
      .catch(error => this._notification.warning(error));
  }

  public updateNew(): void {
    this._api.orders.updateNew(this.order)
      .then(data => this.resetAndNoify(data, "Order has been updated."))
      .catch(error => this._notification.warning(error));
  }

  public stage(): void {
    this._api.orders.stage(this.order)
      .then(data => this.resetAndNoify(data, "Order has been staged."))
      .catch(error => this._notification.warning(error));
  }

  public route(): void {
    this._api.orders.route(this.order)
      .then(data => this.resetAndNoify(data, "Order has been routed."))
      .catch(error => this._notification.warning(error));
  }

  public invoice(): void {
    this._api.orders.invoice(this.order)
      .then(data => this.resetAndNoify(data, "Order has been invoiced."))
      .then(_ => this.viewInvoice())
      .catch(error => this._notification.warning(error));
  }

  public viewInvoice(): void {
    this._api.orders.getInvoiceDetail(this.order.id)
      .then(data => this._invoiceReport.show(data))
  }

  /*
    pay(): void {
      this._api.orders.pay(this.order)
        .then(data => this.resetAndNoify(data, "Order has been paid."))
        .catch(error => this._notification.warning(error));
    }
  */

  public ship(): void {
    this._api.orders.ship(this.order)
      .then(data => this.resetAndNoify(data, "Order has been shiped."))
      .catch(error => this._notification.warning(error));
  }

  public returns(): void {
    this._api.orders.returns(this.order)
      .then(data => this.resetAndNoify(data, "Order items has been retuned."))
      .catch(error => this._notification.warning(error));
  }

  public complete(): void {
    this._api.orders.complete(this.order)
      .then(data => this.resetAndNoify(data, "Order has been completed."))
      .catch(error => this._notification.warning(error));
  }

  public cancel(): void {
    this._api.orders.cancel(this.order)
      .then(data => {
        this._notification.success("Order has been cancelled.");
        this._router.navigateBack();
      })
      .catch(error => this._notification.warning(error));
  }

  public back(): void {
    this._router.navigateBack();
  }
}