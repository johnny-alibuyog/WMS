import {Router} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {Supplier} from '../common/models/supplier';
import {ProductInventory} from '../common/models/product';
import {PurchaseOrderPaymentCreate} from './purchase-order-payment-create';
import {PurchaseOrderReceiptCreate} from './purchase-order-receipt-create';
import {PurchaseOrder, PurchaseOrderStatus, PurchaseOrderItem, PurchaseOrderPayment, PurchaseOrderReceipt, PurchaseOrderReceivable} from '../common/models/purchase-order';
import {PurchaseOrderNewlyCreatedEvent, PurchaseOrderSubmittedEvent, PurchaseOrderApprovedEvent, PurchaseOrderPaidEvent, PurchaseOrderReceivedEvent, PurchaseOrderCompletedEvent, PurchaseOrderCancelledEvent} from '../common/models/purchase-order-event';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';
import {ServiceApi} from '../services/service-api';
import {Lookup} from '../common/custom_types/lookup';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class PurchaseOrderCreate {
  private _api: ServiceApi;
  private _router: Router;
  private _dialog: DialogService;
  private _notification: NotificationService;

  public header: string = 'Purchase Order';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public purchaseOrder: PurchaseOrder;
  public selectedItem: PurchaseOrderItem;
  public selectedPayment: PurchaseOrderPayment;
  public selectedReceipt: PurchaseOrderReceipt;
  public products: Lookup<string>[] = [];
  public suppliers: Lookup<string>[] = [];

  public itemPage: Pager<PurchaseOrderItem> = new Pager<PurchaseOrderItem>();
  public paymentPage: Pager<PurchaseOrderPayment> = new Pager<PurchaseOrderPayment>();
  public receiptPage: Pager<PurchaseOrderReceipt> = new Pager<PurchaseOrderReceipt>();
  public receivablePage: Pager<PurchaseOrderReceivable> = new Pager<PurchaseOrderReceivable>();

  constructor(api: ServiceApi, router: Router, dialog: DialogService, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._dialog = dialog;
    this._notification = notification;

    this.itemPage.onPage = () => this.initializeItemPage();
    this.paymentPage.onPage = () => this.initializePaymentPage();
    this.receiptPage.onPage = () => this.initializeReceiptPage();
    this.receivablePage.onPage = () => this.initializeReceivablePage();

    this._api.suppliers.getLookups()
      .then(data => this.suppliers = data);
  }

  activate(purchaseOrder: PurchaseOrder): void {
    if (purchaseOrder.id) {
      this.isEdit = true;
      this._api.purchaseOrders.get(purchaseOrder.id)
        .then(data => this.setPurchaseOrder(<PurchaseOrder>data))
        .catch(error => this._notification.warning(error));
    }
    else {
      this.isEdit = false;
      this.setPurchaseOrder(<PurchaseOrder>{});
    }
  }

  resetAndNoify(purchaseOrder: PurchaseOrder, notificationMessage: string) {
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

    this.initializeItemPage();
    this.initializePaymentPage();
    this.initializeReceiptPage();
    this.initializeReceivablePage();

    this.getSupplierProducts();
  }

  close() {
    this._router.navigateBack();
  }

  save() {
    if (this.isEdit) {
      this.updateNew();
    }
    else {
      this.createNew();
    }
  }

  getSupplierProducts(): void {
    if (!this.purchaseOrder.supplier && !this.purchaseOrder.supplier.id) {
      this.products = [];
      return;
    }

    this._api.suppliers.getProductLookups(this.purchaseOrder.supplier.id)
      .then(data => this.products = data);
  }

  initializePaymentPage(): void {
    this.paymentPage.count = this.purchaseOrder.payments.length;
    this.paymentPage.items = this.purchaseOrder.payments.slice(
      this.paymentPage.start,
      this.paymentPage.end
    );
  }

  initializeReceiptPage(): void {
    this.receiptPage.count = this.purchaseOrder.receipts.length;
    this.receiptPage.items = this.purchaseOrder.receipts.slice(
      this.receiptPage.start,
      this.receiptPage.end
    );
  }

  initializeReceivablePage(): void {
    this.receivablePage.count = this.purchaseOrder.receivables.length;
    this.receivablePage.items = this.purchaseOrder.receivables.slice(
      this.receivablePage.start,
      this.receivablePage.end
    );
  }

  /*
  addItem(): void {
    if (!this.purchaseOrder.items)
      this.purchaseOrder.items = <PurchaseOrderItem[]>[];

    var item = <PurchaseOrderItem>{
      quantityValue: 0,
      unitPriceAmountce: 0,
    };

    this.purchaseOrder.items.push(item);
    this.selectedItem = item;
    this.initializeItemPage();
  }

  editItem(item: PurchaseOrderItem): void {
    if (this.selectedItem !== item)
      this.selectedItem = item;
  }

  deleteItem(item: PurchaseOrderItem): void {
    var index = this.purchaseOrder.items.indexOf(item);
    if (index > -1) {
      this.purchaseOrder.items.splice(index, 1);
    }
    this.initializeItemPage();
  }
  */

  initializeItem(item: PurchaseOrderItem): void {
    if (!item.product) {
      item.quantityValue = 0;
      item.unitCostAmount = 0;
      return;
    }
    else {
      this._api.products.getInventory(item.product.id).then(data => {
        var product = <ProductInventory>data;
        item.quantityValue = product.targetValue || 1;
        item.unitCostAmount = product.wholeSalePriceAmount || 0;
      });
    }
  }

  initializeItemPage(): void {
    this.itemPage.count = this.purchaseOrder.items.length;
    this.itemPage.items = this.purchaseOrder.items.slice(
      this.itemPage.start,
      this.itemPage.end
    );
  }

  addItem(): void {
    if (!this.purchaseOrder.items)
      this.purchaseOrder.items = <PurchaseOrderItem[]>[];

    var item = <PurchaseOrderItem>{
      quantityValue: 0,
      unitPriceAmountce: 0,
    };

    this.purchaseOrder.items.push(item);
    this.selectedItem = item;
    this.initializeItemPage();
  }

  editItem(item: PurchaseOrderItem): void {
    if (this.selectedItem !== item)
      this.selectedItem = item;
  }

  deleteItem(item: PurchaseOrderItem): void {
    var index = this.purchaseOrder.items.indexOf(item);
    if (index > -1) {
      this.purchaseOrder.items.splice(index, 1);
    }
    this.initializeItemPage();
  }

  addReceipt(): void {
    this._dialog.open({ viewModel: PurchaseOrderReceiptCreate, model: this.purchaseOrder })
      .then(response => {
        if (!response.wasCancelled)
          this.setPurchaseOrder(<PurchaseOrder>response.output);
      });
  }

  addPayment(): void {
    this._dialog.open({ viewModel: PurchaseOrderPaymentCreate, model: this.purchaseOrder })
      .then(response => {
        if (!response.wasCancelled)
          this.setPurchaseOrder(<PurchaseOrder>response.output);
      });
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

  back(): void {
    this._router.navigateBack();
  }
}