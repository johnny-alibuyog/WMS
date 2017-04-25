import { Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Dictionary } from '../common/custom_types/dictionary';
import { Lookup } from '../common/custom_types/lookup';
import { StageDefinition } from '../common/models/stage-definition';
import { PurchaseOrder, PurchaseOrderStatus, PurchaseOrderAggregate, PurchaseOrderReceivable, purchaseOrderEvents, } from '../common/models/purchase-order';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { pricing } from '../common/models/pricing';
import { VoucherReport } from './voucher-report';

@autoinject
export class PurchaseOrderCreate {
  private _api: ServiceApi;
  private _router: Router;
  private _notification: NotificationService;
  private _eventAggegator: EventAggregator;
  private _subscriptions: Subscription[] = [];
  private _voucherReport: VoucherReport;

  public header: string = 'Purchase Order';
  public isEdit: boolean = false;
  public canSave: boolean = true;

  public paymentTypes: Lookup<string>[] = [];
  public suppliers: Lookup<string>[] = [];
  public products: Lookup<string>[] = [];
  public statuses: Lookup<PurchaseOrderStatus>[] = [];
  public purchaseOrder: PurchaseOrder;

  constructor(api: ServiceApi, router: Router, notification: NotificationService, eventAggegator: EventAggregator, voucherReport: VoucherReport) {
    this._api = api;
    this._router = router;
    this._notification = notification;
    this._eventAggegator = eventAggegator;
    this._voucherReport = voucherReport;
  }

  public getInitializedPurchaseOrder(): PurchaseOrder {
    return <PurchaseOrder>{
      createdOn: new Date(),
      pricing: pricing.distributorPrice,
      stage: <StageDefinition<PurchaseOrderStatus, PurchaseOrderAggregate>>{
        allowedTransitions: [],
        allowedModifications: [
          PurchaseOrderAggregate.items,
        ]
      }
    };
  }

  public get isPurchaseOrderApproved(): boolean {
    return this.purchaseOrder && this.purchaseOrder.status >= PurchaseOrderStatus.approved;
  }

  public activate(purchaseOrder: PurchaseOrder): void {
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

  public deactivate(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  public resetAndNoify(purchaseOrder: PurchaseOrder, notificationMessage?: string) {
    this.setPurchaseOrder(purchaseOrder);

    if (notificationMessage) {
      this._notification.success(notificationMessage);
    }
  }

  public setPurchaseOrder(purchaseOrder: PurchaseOrder): void {
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
    this.purchaseOrder.receivables = this._api.purchaseOrders.computeReceivablesFrom(this.purchaseOrder);

    this.hydrateSupplierProducts(this.purchaseOrder.supplier);
  }

  public changeSupplier(supplier: Lookup<string>): void {
    this.products = [];
    this.purchaseOrder.items = [];

    if (!supplier && !supplier.id) {
      return;
    }

    this.hydrateSupplierProducts(supplier);
  }

  public hydrateSupplierProducts(supplier: Lookup<string>): void {
    if (supplier == null || supplier.id == null) {
      return;
    }

    this._api.suppliers.getProductLookups(this.purchaseOrder.supplier.id)
      .then(data => this.products = data);
  }

  public addItem(): void {
    this._eventAggegator.publish(purchaseOrderEvents.item.add);
  }

  public addPayment(): void {
    this._eventAggegator.publish(purchaseOrderEvents.payment.add);
  }

  public addReceipt(): void {
    this._eventAggegator.publish(purchaseOrderEvents.receipts.add);
  }

  public save(): void {
    // generate new receipts from receivables >> receiving items
    var newReceipts = this._api.purchaseOrders.generateNewReceiptsFrom(this.purchaseOrder);
    newReceipts.forEach(newReceipt => this.purchaseOrder.receipts.push(newReceipt));

    this._api.purchaseOrders.save(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchas eOrder has been saved."))
      .catch(error => this._notification.warning(error));
  }

  public submit(): void {
    this._api.purchaseOrders.submit(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchase order has been submitted."))
      .catch(error => this._notification.warning(error));
  }

  public approve(): void {
    this._api.purchaseOrders.approve(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchase order has been approved."))
      .then(_ => this.showVoucher())
      .catch(error => this._notification.warning(error));
  }

  public showVoucher(): void {
    this._api.purchaseOrders.getVoucher(this.purchaseOrder.id)
      .then(data => this._voucherReport.show(data))
  }

  public reject(): void {
    this._api.purchaseOrders.reject(this)
      .then(data => this.resetAndNoify(data, "Purchase order has been rejected."))
      .catch(error => this._notification.warning(error));
  }

  public complete(): void {
    this._api.purchaseOrders.complete(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchase order has been completed."))
      .catch(error => this._notification.warning(error));
  }

  public cancel(): void {
    this._api.purchaseOrders.cancel(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchase order has been cancelled."))
      .catch(error => this._notification.warning(error));
  }

  public refresh() {
    this.activate(this.purchaseOrder);
  }

  public back(): void {
    this._router.navigateBack();
  }
}