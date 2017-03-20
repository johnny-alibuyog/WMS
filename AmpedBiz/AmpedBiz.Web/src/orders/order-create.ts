import { Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Dictionary } from '../common/custom_types/dictionary';
import { Lookup } from '../common/custom_types/lookup';
import { StageDefinition } from '../common/models/stage-definition';
import { Order, OrderStatus, OrderAggregate, OrderPayable, orderEvents } from '../common/models/order';
import { ServiceApi } from '../services/service-api';
import { Override } from '../users/override';
import { ReportViewer } from '../common/controls/report-viewer';
import { DialogService } from 'aurelia-dialog';
import { NotificationService } from '../common/controls/notification-service';
import { OrderInvoiceDetailReport } from './order-invoice-detail-report';
import { pricing } from '../common/models/pricing';

@autoinject
export class OrderCreate {
  private readonly _api: ServiceApi;
  private readonly _router: Router;
  private readonly _dialog: DialogService;
  private readonly _notification: NotificationService;
  private readonly _eventAggregator: EventAggregator;
  private readonly _invoiceReport: OrderInvoiceDetailReport;

  private _subscriptions: Subscription[] = [];

  public header: string = 'Order';
  public canSave: boolean = true;

  public products: Lookup<string>[] = [];
  public branches: Lookup<string>[] = [];
  public customers: Lookup<string>[] = [];
  public paymentTypes: Lookup<string>[] = [];
  public pricings: Lookup<string>[] = [];
  public returnReasons: Lookup<string>[] = [];
  public statuses: Lookup<OrderStatus>[] = [];
  public payable: OrderPayable;
  public order: Order;

  constructor(api: ServiceApi, router: Router, dialog: DialogService, notification: NotificationService, eventAggregator: EventAggregator, invoiceReport: OrderInvoiceDetailReport) {
    this._api = api;
    this._router = router;
    this._dialog = dialog;
    this._notification = notification;
    this._eventAggregator = eventAggregator;
    this._invoiceReport = invoiceReport;
  }

  public getInitializedOrder(): Order {
    return <Order>{
      orderedOn: new Date(),
      pricing: pricing.listPrice,
      stage: <StageDefinition<OrderStatus, OrderAggregate>>{
        allowedTransitions: [],
        allowedModifications: [
          OrderAggregate.items,
          OrderAggregate.payments,
        ]
      }
    };
  }

  public get isOrderInvoiced(): boolean {
    return this.order && this.order.status >= OrderStatus.invoiced;
  }

  public activate(order: Order): void {
    this._subscriptions = [
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
      Promise<Lookup<string>[]>,
      Promise<Lookup<string>[]>,
      Promise<Lookup<OrderStatus>[]>,
      Promise<OrderPayable>,
      Promise<Order>] = [
        this._api.products.getLookups(),
        this._api.branches.getLookups(),
        this._api.customers.getLookups(),
        this._api.paymentTypes.getLookups(),
        this._api.pricings.getLookups(),
        this._api.returnReasons.getLookups(),
        this._api.orders.getStatusLookup(),
        order.id
          ? this._api.orders.getPayables(order.id)
          : Promise.resolve(<OrderPayable>{}),
        order.id
          ? this._api.orders.get(order.id)
          : Promise.resolve(this.getInitializedOrder())
      ];

    Promise.all(requests)
      .then(responses => {
        this.products = responses[0];
        this.branches = responses[1];
        this.customers = responses[2];
        this.paymentTypes = responses[3];
        this.pricings = responses[4];
        this.returnReasons = responses[5];
        this.statuses = responses[6];
        this.payable = responses[7];
        this.setOrder(responses[8]);

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
    order.items = order.items || [];
    order.returns = order.returns || [];
    order.payments = order.payments || [];
    order.returnables = this._api.orders.computeReturnablesFrom(order);

    this.order = order;
  }

  public addItem(): void {
    this._eventAggregator.publish(orderEvents.item.add);
  }

  public addReturn(): void {
    this._eventAggregator.publish(orderEvents.return.add);
  }

  public addPayment(): void {
    this._eventAggregator.publish(orderEvents.payment.add);
  }

  public save(): void {
    // generate new returns from returnables >> returning items
    var newReturns = this._api.orders.generateNewReturnsFrom(this.order);
    newReturns.forEach(newReturn => this.order.returns.push(newReturn));

    let _save = () => this._api.orders.save(this.order)
      .then(data => this.resetAndNoify(data, "Order has been saved."))
      .catch(error => this._notification.warning(error));

    if (newReturns && newReturns.length > 0) {
      // if there are new returns, require override
      this._dialog
        .open({ viewModel: Override, model: {} })
        .then(response => { if (!response.wasCancelled) _save(); });
    }
    else {
      _save();
    }


  }

  public invoice(): void {
    this._api.orders.invoice(this.order)
      .then(data => this.resetAndNoify(data, null))
      .then(_ => this.showInvoice())
      .catch(error => this._notification.warning(error));
  }

  public showInvoice(): void {
    this._api.orders.getInvoiceDetail(this.order.id)
      .then(data => this._invoiceReport.show(data))
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

  public ship(): void {
    this._api.orders.ship(this.order)
      .then(data => this.resetAndNoify(data, "Order has been shiped."))
      .catch(error => this._notification.warning(error));
  }

  public complete(): void {
    this._api.orders.complete(this.order)
      .then(data => this.resetAndNoify(data, "Order has been completed."))
      .catch(error => this._notification.warning(error));
  }

  public cancel(): void {
    this._api.orders.cancel(this.order)
      .then(data => this.resetAndNoify(data, "Order has been cancelled."))
      .catch(error => this._notification.warning(error));
  }

  public back(): void {
    this._router.navigateBack();
  }
}