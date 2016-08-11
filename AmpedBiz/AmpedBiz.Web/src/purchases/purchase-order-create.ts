import {Router} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';
import {EventAggregator, Subscription} from 'aurelia-event-aggregator';
import {Dictionary} from '../common/custom_types/dictionary';
import {PurchaseOrder, PurchaseOrderStatus, purchaseOrderEvents} from '../common/models/purchase-order';
import {PurchaseOrderNewlyCreatedEvent, PurchaseOrderSubmittedEvent, PurchaseOrderApprovedEvent, PurchaseOrderPaidEvent, PurchaseOrderReceivedEvent, PurchaseOrderCompletedEvent, PurchaseOrderCancelledEvent} from '../common/models/purchase-order-event';
import {ServiceApi} from '../services/service-api';
import {Lookup} from '../common/custom_types/lookup';
import {NotificationService} from '../common/controls/notification-service';

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

  getInitializedOrder(): PurchaseOrder {
    let purchaseOrder: PurchaseOrder = {
      createdOn: new Date(),
      allowedTransitions: <Dictionary<string>>{}
    };
    purchaseOrder.allowedTransitions[PurchaseOrderStatus[PurchaseOrderStatus.new]] = "Save";
    return purchaseOrder;
  }


  activate(purchaseOrder: PurchaseOrder): void {
    this._subscriptions = [
      this._eventAggegator.subscribe(
        purchaseOrderEvents.payment.paid,
        data => this.resetAndNoify(data, null)
      ),
      this._eventAggegator.subscribe(
        purchaseOrderEvents.receipts.received,
        data => this.resetAndNoify(data, null)
      )
    ];

    if (purchaseOrder.id) {
      this.isEdit = true;
    }
    else {
      this.isEdit = false;
    }

    let requests: [
      Promise<Lookup<string>[]>,
      Promise<Lookup<PurchaseOrderStatus>[]>,
      Promise<PurchaseOrder>] = [
        this._api.suppliers.getLookups(),
        this._api.purchaseOrders.getStatusLookup(),
        this.isEdit
          ? this._api.purchaseOrders.get(purchaseOrder.id)
          : Promise.resolve(this.getInitializedOrder())
      ];

    Promise.all(requests)
      .then(data => {
        this.suppliers = data[0];
        this.statuses = data[1];
        this.purchaseOrder = data[2];
      })
      .catch(error =>
        this._notification.warning(error)
      );

    this._api.suppliers.getLookups()
      .then(data => this.suppliers = data);
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
    this._api.suppliers.getProductLookups(this.purchaseOrder.supplier.id)
      .then(data => this.products = data);
  }

  addItem(): void {
    this._eventAggegator.publish(purchaseOrderEvents.item.add);
  }

  addPayment(): void {
    this._eventAggegator.publish(purchaseOrderEvents.payment.pay);
  }

  addReceipt(): void {
    this._eventAggegator.publish(purchaseOrderEvents.receipts.receive);
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
    var newlyCreatedEvent = <PurchaseOrderNewlyCreatedEvent>{
      createdBy: this._api.auth.userAsLookup,
      createdOn: this.purchaseOrder.createdOn,
      expectedOn: this.purchaseOrder.expectedOn,
      supplier: this.purchaseOrder.supplier,
      items: this.purchaseOrder.items
    };

    this._api.purchaseOrders.createNew(newlyCreatedEvent)
      .then(data => this.resetAndNoify(data, "Purchase order has been created."))
      .catch(error => this._notification.warning(error));
  }

  updateNew(): void {
    var newlyCreatedEvent = <PurchaseOrderNewlyCreatedEvent>{
      purchaseOrderId: this.purchaseOrder.id,
      createdBy: this._api.auth.userAsLookup,
      createdOn: this.purchaseOrder.createdOn,
      expectedOn: this.purchaseOrder.expectedOn,
      supplier: this.purchaseOrder.supplier,
      items: this.purchaseOrder.items
    };

    this._api.purchaseOrders.updateNew(newlyCreatedEvent)
      .then(data => this.resetAndNoify(data, "Purchase order has been updated."))
      .catch(error => this._notification.warning(error));
  }

  submit(): void {
    var submittedEvent = <PurchaseOrderSubmittedEvent>{
      purchaseOrderId: this.purchaseOrder.id,
      submittedBy: this._api.auth.userAsLookup,
      submittedOn: new Date(),
    };

    this._api.purchaseOrders.submit(submittedEvent)
      .then(data => this.resetAndNoify(data, "Purchase order has been submitted."))
      .catch(error => this._notification.warning(error));
  }

  approve(): void {
    var approvedEvent = <PurchaseOrderApprovedEvent>{
      purchaseOrderId: this.purchaseOrder.id,
      approvedBy: this._api.auth.userAsLookup,
      approvedOn: new Date()
    };

    this._api.purchaseOrders.approve(approvedEvent)
      .then(data => this.resetAndNoify(data, "Purchase order has been approved."))
      .catch(error => this._notification.warning(error));
  }

  reject(): void {
    var newlyCreatedEvent = <PurchaseOrderNewlyCreatedEvent>{
      purchaseOrderId: this.purchaseOrder.id,
      createdBy: this._api.auth.userAsLookup,
      createdOn: this.purchaseOrder.createdOn,
    };

    this._api.purchaseOrders.reject(newlyCreatedEvent)
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
    var completedEvent = <PurchaseOrderCompletedEvent>{
      purchaseOrderId: this.purchaseOrder.id,
      completedBy: this._api.auth.userAsLookup,
      completedOn: new Date()
    };

    this._api.purchaseOrders.complete(completedEvent)
      .then(data => this.resetAndNoify(data, "Purchase order has been completed."))
      .catch(error => this._notification.warning(error));
  }

  cancel(): void {
    var cancelledEvent = <PurchaseOrderCancelledEvent>{
      purchaseOrderId: this.purchaseOrder.id,
      cancelledBy: this._api.auth.userAsLookup,
      cancelledOn: new Date(),
      cancellationReason: 'Cancellation Reason'
    };

    this._api.purchaseOrders.cancel(cancelledEvent)
      .then(data => this.resetAndNoify(data, "Purchase order has been cancelled."))
      .catch(error => this._notification.warning(error));
  }

  refresh(){
    this.activate(this.purchaseOrder);
  }

  back(): void {
    this._router.navigateBack();
  }
}