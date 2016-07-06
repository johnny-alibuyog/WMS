import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {Supplier} from '../common/models/supplier';
import {ProductInventory} from '../common/models/product';
import {PurchaseOrder, PurchaseOrderDetail, RecievingDetail, PaymentDetail} from '../common/models/purchase-order';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';
import {ServiceApi} from '../services/service-api';
import {Lookup} from '../common/custom_types/lookup';
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
  public products: Lookup<string>[] = [];
  public suppliers: Lookup<string>[] = [];

  public purchaseOrderDetailsPage: Pager<PurchaseOrderDetail> = new Pager<PurchaseOrderDetail>();
  public recievingDetailsPage: Pager<RecievingDetail> = new Pager<RecievingDetail>();
  public paymentDetailsPage: Pager<PaymentDetail> = new Pager<PaymentDetail>();

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;

    this.purchaseOrderDetailsPage.onPage = () => this.initializePurchaseOrderDetailsPage();

    this._api.suppliers.getLookups()
      .then(data => this.suppliers = data);
  }

  activate(purchaseOrder: PurchaseOrder): void {
    if (purchaseOrder) {
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

  setPurchaseOrder(purchaseOrder: PurchaseOrder): void {
    this.purchaseOrder = purchaseOrder;
    this.purchaseOrder.purchaseOrderDetails = this.purchaseOrder.purchaseOrderDetails || [];
    this.purchaseOrder.recievingDetails = this.purchaseOrder.recievingDetails || [];
    this.purchaseOrder.paymentDetails = this.purchaseOrder.paymentDetails || []

    this.initializePurchaseOrderDetailsPage();
    this.getSupplierProducts();
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
      this.products = [];
      return;
    }

    this._api.suppliers.getProductLookups(this.purchaseOrder.supplierId)
      .then(data => this.products = data);
  }

  initializePurchaseOrderDetail(item: PurchaseOrderDetail): void {
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

  initializePurchaseOrderDetailsPage(): void {
    this.purchaseOrderDetailsPage.count = this.purchaseOrder.purchaseOrderDetails.length;
    this.purchaseOrderDetailsPage.items = this.purchaseOrder.purchaseOrderDetails.slice(
      this.purchaseOrderDetailsPage.start,
      this.purchaseOrderDetailsPage.end
    );
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
    this.initializePurchaseOrderDetailsPage();
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
    this.initializePurchaseOrderDetailsPage();
  }

  addRecievingDetail(): void {
    if (!this.purchaseOrder.recievingDetails)
      this.purchaseOrder.recievingDetails = [];

    this.purchaseOrder.recievingDetails.push(<RecievingDetail>{});
  }

  addPaymentDetail(): void {
    if (!this.purchaseOrder.paymentDetails)
      this.purchaseOrder.paymentDetails = [];

    this.purchaseOrder.paymentDetails.push(<PaymentDetail>{});
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