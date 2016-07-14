import {Router} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {Supplier} from '../common/models/supplier';
import {ProductInventory} from '../common/models/product';
import {PurchaseOrderPaymentCreate} from './purchase-order-payment-create';
import {PurchaseOrder, PurchaseOrderStatus, PurchaseOrderItem, PurchaseOrderReciept, PurchaseOrderPayment} from '../common/models/purchase-order';
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
  public selectedReceipt: PurchaseOrderReciept;
  public selectedPayment: PurchaseOrderPayment;
  public products: Lookup<string>[] = [];
  public suppliers: Lookup<string>[] = [];

  public itemPage: Pager<PurchaseOrderItem> = new Pager<PurchaseOrderItem>();
  public recieptPage: Pager<PurchaseOrderReciept> = new Pager<PurchaseOrderReciept>();
  public paymentPage: Pager<PurchaseOrderPayment> = new Pager<PurchaseOrderPayment>();

  constructor(api: ServiceApi, router: Router, dialog: DialogService, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._dialog = dialog;
    this._notification = notification;

    this.itemPage.onPage = () => this.initializeItemPage();
    this.paymentPage.onPage = () => this.initializePaymentPage();

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
    this.purchaseOrder.reciepts = this.purchaseOrder.reciepts || [];
    this.purchaseOrder.payments = this.purchaseOrder.payments || []

    this.initializeItemPage();
    this.initializePaymentPage();

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
    if (!this.purchaseOrder.supplierId) {
      this.products = [];
      return;
    }

    this._api.suppliers.getProductLookups(this.purchaseOrder.supplierId)
      .then(data => this.products = data);
  }

  initializePaymentPage(): void {
    this.paymentPage.count = this.purchaseOrder.payments.length;
    this.paymentPage.items = this.purchaseOrder.payments.slice(
      this.paymentPage.start,
      this.paymentPage.end
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
      item.unitPriceAmount = 0;
      return;
    }
    else {
      this._api.products.getInventory(item.product.id).then(data => {
        var product = <ProductInventory>data;
        item.quantityValue = product.targetValue || 1;
        item.unitPriceAmount = product.wholeSalePriceAmount || 0;
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
    if (!this.purchaseOrder.reciepts)
      this.purchaseOrder.reciepts = [];

    this.purchaseOrder.reciepts.push(<PurchaseOrderReciept>{});
  }

  addPayment(): void {
    this._dialog.open({ viewModel: PurchaseOrderPaymentCreate, model: this.purchaseOrder })
      .then(response => {
        if (!response.wasCancelled)
          this.setPurchaseOrder(<PurchaseOrder>response.output);
      });
  }

  createNew(): void {
    this._api.purchaseOrders.createNew(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchase order has been created."))
      .catch(error => this._notification.warning(error));
  }

  updateNew(): void {
    this._api.purchaseOrders.updateNew(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchase order has been updated."))
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
    this._api.purchaseOrders.reject(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchase order has been rejected."))
      .catch(error => this._notification.warning(error));
  }

  pay(): void {
    this._api.purchaseOrders.pay(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchase order has been paid."))
      .catch(error => this._notification.warning(error));
  }

  recieve(): void {
    this._api.purchaseOrders.receive(this.purchaseOrder)
      .then(data => this.resetAndNoify(data, "Purchase order has been received."))
      .catch(error => this._notification.warning(error));
  }

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

  back(): void {
    this._router.navigateBack();
  }
}