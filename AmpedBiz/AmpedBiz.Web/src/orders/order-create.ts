import { Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Lookup } from '../common/custom_types/lookup';
import { StageDefinition } from '../common/models/stage-definition';
import { Order, OrderStatus, OrderAggregate, OrderPayable, orderEvents } from '../common/models/order';
import { ServiceApi } from '../services/service-api';
import { Override } from '../users/override';
import { DialogService } from 'aurelia-dialog';
import { NotificationService } from '../common/controls/notification-service';
import { InvoiceReport } from './invoice-report';
import { AuthService } from '../services/auth-service';
import { pricing } from '../common/models/pricing';
import { role } from '../common/models/role';
import { ActionResult } from '../common/controls/notification';

@autoinject
export class OrderCreate {

  private _subscriptions: Subscription[] = [];

  public header: string = 'Order';

  public readonly canSave: boolean = true;
  public readonly canAddItem: boolean = true;
  public readonly canAddReturn: boolean = true;
  public readonly canAddPayment: boolean = true;
  public readonly canInvoice: boolean = true;
  public readonly canStage: boolean = true;
  public readonly canRoute: boolean = true;
  public readonly canShip: boolean = true;
  public readonly canComplete: boolean = true;
  public readonly canCancel: boolean = true;

  public products: Lookup<string>[] = [];
  public branches: Lookup<string>[] = [];
  public customers: Lookup<string>[] = [];
  public paymentTypes: Lookup<string>[] = [];
  public pricings: Lookup<string>[] = [];
  public returnReasons: Lookup<string>[] = [];
  public statuses: Lookup<OrderStatus>[] = [];
  public payable: OrderPayable;
  public order: Order;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _auth: AuthService,
    private readonly _router: Router,
    private readonly _dialog: DialogService,
    private readonly _notification: NotificationService,
    private readonly _eventAggregator: EventAggregator,
    private readonly _invoiceReport: InvoiceReport
  ) {
    this.canSave = this._auth.isAuthorized([role.admin, role.manager, role.salesclerk]);
    this.canAddItem = this._auth.isAuthorized([role.admin, role.manager, role.salesclerk]);
    this.canAddReturn = this._auth.isAuthorized([role.admin, role.manager, role.salesclerk]);
    this.canAddPayment = this._auth.isAuthorized([role.admin, role.manager, role.salesclerk]);
    this.canInvoice = this._auth.isAuthorized([role.admin, role.manager, role.salesclerk]);
    this.canStage = this._auth.isAuthorized([role.admin, role.manager, role.salesclerk]);
    this.canRoute = this._auth.isAuthorized([role.admin, role.manager, role.salesclerk]);
    this.canShip = this._auth.isAuthorized([role.admin, role.manager, role.salesclerk]);
    this.canComplete = this._auth.isAuthorized([role.admin, role.manager]);
    this.canCancel = this._auth.isAuthorized([role.admin, role.manager]);
  }

  public getInitializedOrder(): Order {
    return <Order>{
      orderedOn: new Date(),
      orderedBy: this._auth.userAsLookup,
      createdOn: new Date(),
      createdBy: this._auth.userAsLookup,
      pricing: pricing.retailPrice,
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

  public async activate(order: Order): Promise<void> {
    try {
      this._subscriptions = [
        this._eventAggregator.subscribe(
          orderEvents.invoiceDetail.show,
          data => window.open(data, "_blank")
        )
      ];

      let newOrder: Order;

      [
        this.products,
        this.branches,
        this.customers,
        this.paymentTypes,
        this.pricings,
        this.returnReasons,
        this.statuses,
        this.payable,
        newOrder
      ] = await Promise.all([
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
      ]);

      this.setOrder(newOrder);
    }
    catch (error) {
      this._notification.error(error);
    }
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

    if (!order.branch && this.branches && this.branches.length > 0) {
      order.branch = this.branches.find(x => x.id == this._api.auth.user.branchId);
    }

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

  public async save(): Promise<void> {
    try {
      // generate new returns from returnables >> returning items
      let withReturns = this._api.orders.generateNewReturns(this.order);

      let confirmation = (withReturns)
        ? await this._dialog.open({ viewModel: Override, model: {} }).whenClosed() // if there are new returns, require override
        : await this._notification.confirm('Do you want to save the order?').whenClosed();

      if (!confirmation.wasCancelled) {
        let data = await this._api.orders.save(this.order)
        this.resetAndNoify(data, "Order has been saved.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async invoice(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to invoice the order?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = await this._api.orders.invoice(this.order);
        this.resetAndNoify(data, null);
        await this.showInvoice();
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async showInvoice(): Promise<void> {
    let data = await this._api.orders.getInvoiceDetail(this.order.id);
    await this._invoiceReport.show(data);
  }

  public signalPricingChanged(): void {
    this._eventAggregator.publish(orderEvents.pricingChanged);
  }

  public async stage(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to stage the order?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = await this._api.orders.stage(this.order);
        this.resetAndNoify(data, "Order has been staged.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async route(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to route the order?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = await this._api.orders.route(this.order);
        this.resetAndNoify(data, "Order has been routed.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async ship(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to ship the order?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = await this._api.orders.ship(this.order);
        this.resetAndNoify(data, "Order has been shipped.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async complete(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to complete the order?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = await this._api.orders.complete(this.order);
        this.resetAndNoify(data, "Order has been completed.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async cancel(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to cancel the order?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = await this._api.orders.cancel(this.order);
        this.resetAndNoify(data, "Order has been cancelled.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public back(): void {
    this._router.navigateBack();
  }
}