import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {Supplier} from './common/models/supplier';
import {ProductInventory} from './common/models/product';
import {PurchaseOrder, PurchaseOrderDetail, RecievingDetail, PaymentDetail} from './common/models/purchase-order';
import {ServiceApi} from '../services/service-api';
import {KeyValuePair} from '../common/custom_types/key-value-pair';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class PurchaseOrderCreate {
  private _api: ServiceApi;
  private _controller: DialogController;
  private _notification: NotificationService;

  public header: string = 'Purchase Order';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public purchaseOrder: PurchaseOrder;
  public selectedPurchaseOrderDetail: PurchaseOrderDetail;
  public selectedRecievingDetail: RecievingDetail;
  public selectedPaymentDetail: PaymentDetail;
  public products: ProductInventory[];
  public suppliers: KeyValuePair<string, string>[];

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;

    this._api.suppliers.getLookup<string, string>()
      .then(data => {
        this.suppliers = data;
        console.log(this.suppliers);
      });
  }

  activate(purchaseOrder: PurchaseOrder): void {
    if (purchaseOrder) {
      this.isEdit = true;
      this._api.purchaseOrders.get(purchaseOrder.id)
        .then(data => this.purchaseOrder = <PurchaseOrder>data)
        .catch(error => this._notification.warning(error));
    }
    else {
      this.isEdit = false;
      this.purchaseOrder = <PurchaseOrder>{};
    }
  }

  close() {
    this._controller.cancel({ wasCancelled: true, output: null });
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
      this.products = <ProductInventory[]>{};
      return;
    }

    this._api.suppliers.getProductInventories(this.purchaseOrder.supplierId)
      .then(response => this.products = response);
  }

  getProductName(productId: string): string {
      var product = this.products.find(x => x.id == productId);
      if (product)
        return product.name;

      return null;
  }

  initializePurchaseOrderDetail(item: PurchaseOrderDetail): void {
    if (!item.productId) {
      item.quantityValue = 0;
      item.unitPriceAmount = 0;
      return;
    }
    else {
      var product = this.products.find(x => x.id == item.productId);

      item.quantityValue = product.targetValue || 1;
      item.unitPriceAmount = product.wholeSalePriceAmount || 0;
    }
  }

  addPurchaseOrderDetail(): void {
    if (!this.purchaseOrder.purchaseOrderDetails)
      this.purchaseOrder.purchaseOrderDetails = <PurchaseOrderDetail[]>[];

    var item = <PurchaseOrderDetail>{
      quantityValue: 0,
      unitPriceAmountce: 0,
    };

    this.purchaseOrder.purchaseOrderDetails.push(item);
    this.selectedPurchaseOrderDetail = item;
  }

  editPurchaseOrderDetail(item: PurchaseOrderDetail): void {
    if (this.selectedPurchaseOrderDetail !== item)
      this.selectedPurchaseOrderDetail = item;
  }

  deletePurchaseOrderDetail(item: PurchaseOrderDetail): void {
    var index = this.purchaseOrder.purchaseOrderDetails.indexOf(item);
    if (index > -1) {
      this.purchaseOrder.purchaseOrderDetails.splice(index, 1);
    }
  }

  addRecievingDetail(): void {
    if (!this.purchaseOrder.recievingDetails)
      this.purchaseOrder.recievingDetails = [];

    this.purchaseOrder.recievingDetails.push(<RecievingDetail>{});
  }

  addPaymentDetail(): void {
    if (!this.purchaseOrder.paymentDetail)
      this.purchaseOrder.paymentDetail = [];

    this.purchaseOrder.paymentDetail.push(<PaymentDetail>{});
  }

  createNew(): void {
    this._api.purchaseOrders.createNew(this.purchaseOrder)
      .then(data => {
        this._notification.success("Purchase order has been saved.")
          .then((data) => this._controller.ok({ wasCancelled: true, output: <PurchaseOrder>data }));
      })
      .catch(error => {
        this._notification.warning(error)
      });
  }

  updateNew(): void {
    this._api.purchaseOrders.updateNew(this.purchaseOrder)
      .then(data => {
        this._notification.success("Purchase order has been saved.")
          .then((data) => this._controller.ok({ wasCancelled: true, output: <PurchaseOrder>data }));
      })
      .catch(error => {
        this._notification.warning(error)
      });
  }

  submit(): void {


  }

  approve(): void {

  }

  reject(): void {

  }

  pay(): void {

  }

  recieve(): void {

  }

  complete(): void {

  }

  cancel(): void {

  }
}