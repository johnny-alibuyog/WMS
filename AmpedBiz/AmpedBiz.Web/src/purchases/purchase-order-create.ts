import { Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Dictionary } from '../common/custom_types/dictionary';
import { Lookup } from '../common/custom_types/lookup';
import { StageDefinition } from '../common/models/stage-definition';
import { PurchaseOrder, PurchaseOrderStatus, PurchaseOrderAggregate, PurchaseOrderReceivable, purchaseOrderEvents, } from '../common/models/purchase-order';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';

@autoinject
export class PurchaseOrderCreate {
  private _api: ServiceApi;
  private _router: Router;
  private _notification: NotificationService;
  private _eventAggegator: EventAggregator;
  private _subscriptions: Subscription[] = [];

  public header: string = 'Purchase Order';
  public isEdit: boolean = false;
  public canSave: boolean = true;

  public paymentTypes: Lookup<string>[] = [];
  public suppliers: Lookup<string>[] = [];
  public products: Lookup<string>[] = [];
  public statuses: Lookup<PurchaseOrderStatus>[] = [];
  public purchaseOrder: PurchaseOrder;

  constructor(api: ServiceApi, router: Router, notification: NotificationService, eventAggegator: EventAggregator) {
    this._api = api;
    this._router = router;
    this._notification = notification;
    this._eventAggegator = eventAggegator;
  }

  getInitializedPurchaseOrder(): PurchaseOrder {
    return <PurchaseOrder>{
      createdOn: new Date(),
      stage: <StageDefinition<PurchaseOrderStatus, PurchaseOrderAggregate>>{
        allowedTransitions: [],
        allowedModifications: [
          PurchaseOrderAggregate.items,
        ]
      }
    };
  }

  activate(purchaseOrder: PurchaseOrder): void {
    this._subscriptions = [
      this._eventAggegator.subscribe(
        purchaseOrderEvents.receivings.added,
        data => this.addedReceivings(data)
      )
    ];

    let requests: [
      Promise<Lookup<string>[]>,
      Promise<Lookup<string>[]>,
      Promise<Lookup<PurchaseOrderStatus>[]>,
      Promise<PurchaseOrder>] = [
        this._api.paymentTypes.getLookups(),
        this._api.suppliers.getLookups(),
        this._api.purchaseOrders.getStatusLookup(),
        purchaseOrder.id
          ? this._api.purchaseOrders.get(purchaseOrder.id)
          : Promise.resolve(this.getInitializedPurchaseOrder())
      ];

    Promise.all(requests)
      .then(data => {
        this.paymentTypes = data[0];
        this.suppliers = data[1];
        this.statuses = data[2];
        this.setPurchaseOrder(data[3]);
      })
      .catch(error =>
        this._notification.warning(error)
      );
  }

  deactivate(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  resetAndNoify(purchaseOrder: PurchaseOrder, notificationMessage?: string) {
    this.setPurchaseOrder(purchaseOrder);

    if (notificationMessage) {
      this._notification.success(notificationMessage);
    }
  }

  setPurchaseOrder(purchaseOrder: PurchaseOrder): void {
    if (purchaseOrder.id) {
      this.isEdit = true;
    }
    else {
      this.isEdit = false;
    }

    this.purchaseOrder = purchaseOrder;
    this.purchaseOrder.items = this.purchaseOrder.items || [];
    this.purchaseOrder.payments = this.purchaseOrder.payments || [];
    this.purchaseOrder.receipts = this.purchaseOrder.receipts || [];
    this.purchaseOrder.receivables = this.purchaseOrder.receivables || [];

    this.hydrateSupplierProducts(this.purchaseOrder.supplier);
  }

  changeSupplier(supplier: Lookup<string>): void {
    this.products = [];
    this.purchaseOrder.items = [];

    if (!supplier && !supplier.id) {
      return;
    }

    this.hydrateSupplierProducts(supplier);
  }

  hydrateSupplierProducts(supplier: Lookup<string>): void {
    if (supplier == null || supplier.id == null) {
      return;
    }

    this._api.suppliers.getProductLookups(this.purchaseOrder.supplier.id)
      .then(data => this.products = data);
  }

  addItem(): void {
    this._eventAggegator.publish(purchaseOrderEvents.item.add);
  }

  addPayment(): void {
    this._eventAggegator.publish(purchaseOrderEvents.payment.add);
  }

  addReceipt(): void {
    this._eventAggegator.publish(purchaseOrderEvents.receipts.receive);
  }

  addReceivings(): void {
    this._eventAggegator.publish(purchaseOrderEvents.receivings.add);
  }

  addedReceivings(receivables: PurchaseOrderReceivable[]): void {
    this.purchaseOrder.receivables.forEach(item => {
      var value = receivables.find(x => x.product.id == item.product.id);
      item.receiving = value.receiving;
    });
  }

  save(): void {
    this._api.purchaseOrders.save(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchas eOrder has been saved."))
      .catch(error => this._notification.warning(error));
  }

  submit(): void {
    this._api.purchaseOrders.submit(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchase order has been submitted."))
      .catch(error => this._notification.warning(error));
  }

  approve(): void {
    this._api.purchaseOrders.approve(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchase order has been approved."))
      .catch(error => this._notification.warning(error));
  }

  reject(): void {
    this._api.purchaseOrders.reject(this)
      .then(data => this.resetAndNoify(data, "Purchase order has been rejected."))
      .catch(error => this._notification.warning(error));
  }

  /*
  pay(): void {
    var paidEvent = <PurchaseOrderPaidEvent>{
      purchaseOrderId: this.purchaseOrder.id,
      paidBy: this._api.auth.userAsLookup,
      paidOn: new Date(),
      paymentAmount: 0,
      paymentType: null
    };

    this._api.purchaseOrders.pay(paidEvent)
      .then(data => this.resetAndNoify(data, "Purchase order has been paid."))
      .catch(error => this._notification.warning(error));
  }

  recieve(): void {
    var recievedEvent = <PurchaseOrderReceivedEvent>{
      purchaseOrderId: this.purchaseOrder.id,
      receipts: []
    };

    this._api.purchaseOrders.receive(recievedEvent)
      .then(data => this.resetAndNoify(data, "Purchase order has been received."))
      .catch(error => this._notification.warning(error));
  }
  */

  complete(): void {
    this._api.purchaseOrders.complete(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchase order has been completed."))
      .catch(error => this._notification.warning(error));
  }

  cancel(): void {
    this._api.purchaseOrders.cancel(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchase order has been cancelled."))
      .catch(error => this._notification.warning(error));
  }

  refresh() {
    this.activate(this.purchaseOrder);
  }

  back(): void {
    this._router.navigateBack();
  }
}