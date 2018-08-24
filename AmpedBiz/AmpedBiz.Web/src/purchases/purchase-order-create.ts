import { autoinject } from 'aurelia-framework';
import { role } from "../common/models/role";
import { pricing } from '../common/models/pricing';
import { Filter, Sorter, PagerResponse, PageRequest } from './../common/models/paging';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { PurchaseOrder, PurchaseOrderAggregate, PurchaseOrderReceipt, PurchaseOrderStatus, purchaseOrderEvents, PurchaseOrderItem, } from '../common/models/purchase-order';
import { AuthService } from "../services/auth-service";
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Router } from 'aurelia-router';
import { ServiceApi } from '../services/service-api';
import { StageDefinition } from '../common/models/stage-definition';
import { VoucherReport } from './voucher-report';
import { formatDate } from '../services/formaters';
import { ActionResult } from '../common/controls/notification';
import { SessionData } from '../services/session-data';
import { NeedsReorderingPageItem, ProductInventoryFacade } from '../common/models/product';

@autoinject
export class PurchaseOrderCreate {

  private readonly _subscriptions: Subscription[] = [];

  public header: string = 'Purchase Order';

  public readonly canSave: boolean = true;
  public readonly canRecreate: boolean = true;
  public readonly canAddItem: boolean = true;
  public readonly canAddPayment: boolean = true;
  public readonly canAddReceipt: boolean = true;
  public readonly canSubmit: boolean = true;
  public readonly canApprove: boolean = true;
  public readonly canComplete: boolean = true;
  public readonly canCancel: boolean = true;

  public paymentTypes: Lookup<string>[] = [];
  public suppliers: Lookup<string>[] = [];
  public products: Lookup<string>[] = [];
  public statuses: Lookup<PurchaseOrderStatus>[] = [];
  public purchaseOrder: PurchaseOrder;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _auth: AuthService,
    private readonly _router: Router,
    private readonly _notification: NotificationService,
    private readonly _eventAggegator: EventAggregator,
    private readonly _voucherReport: VoucherReport,
    private readonly _sessionData: SessionData,
  ) {
    this.canSave = this._auth.isAuthorized([role.admin, role.manager, role.salesclerk]);
    this.canRecreate = this._auth.isAuthorized([role.admin, role.manager]);
    this.canAddItem = this._auth.isAuthorized([role.admin, role.manager, role.salesclerk]);
    this.canAddPayment = this._auth.isAuthorized([role.admin, role.manager, role.salesclerk]);
    this.canAddReceipt = this._auth.isAuthorized([role.admin, role.manager, role.salesclerk]);
    this.canSubmit = this._auth.isAuthorized([role.admin, role.manager, role.salesclerk]);
    this.canApprove = this._auth.isAuthorized([role.admin, role.manager]);
    this.canComplete = this._auth.isAuthorized([role.admin, role.manager]);
    this.canCancel = this._auth.isAuthorized([role.admin, role.manager]);
  }

  private async newPurchaseOrder(): Promise<PurchaseOrder> {
    let instance = <PurchaseOrder>{
      createdOn: new Date(),
      createdBy: this._auth.userAsLookup,
      items: [],
      stage: <StageDefinition<PurchaseOrderStatus, PurchaseOrderAggregate>>{
        allowedTransitions: [],
        allowedModifications: [
          PurchaseOrderAggregate.items,
        ]
      }
    };
    return Promise.resolve(instance);
  }

  private async initializeForPurchasing(): Promise<PurchaseOrder> {
    let [newPurchaseOrder, forPurchasing] = await Promise.all([
      this.newPurchaseOrder(),
      this.getProductsForPurchasing()
    ]);
    let inventories = await this._api.products.getInventoryList(forPurchasing.items.map(x => x.id));

    newPurchaseOrder.supplier = this.suppliers.find(x => x.id === this._sessionData.forPurchasing.supplierId);

    if (forPurchasing != null && forPurchasing.items != null) {
      forPurchasing.items.forEach(item => {
        let inventory = inventories.find(x => x.id == item.id);
        let facade = new ProductInventoryFacade(inventory);
        let current = facade.default;
        let purchaseOrderItem = <PurchaseOrderItem>{
          product: facade.getProduct(),
          unitOfMeasures: facade.getUnitOfMeasures(),
          quantity: {
            unit: current.unitOfMeasure,
            value: item.reorderQuantity
          },
          standard: {
            unit: current.standard.unit,
            value: current.standard.value,
          },
          unitCostAmount: facade.getPriceAmount(inventory, current.unitOfMeasure, pricing.basePrice),
        };

        newPurchaseOrder.items.push(purchaseOrderItem);
      });
    }

    return newPurchaseOrder;
  }

  private async getProductsForPurchasing(): Promise<PagerResponse<NeedsReorderingPageItem>> {
    var request = <PageRequest>{
      sorter: new Sorter(),
      filter: new Filter(),
      pager: { offset: 0, size: 0 }  // fetch all
    };

    request.filter["supplierId"] = this._sessionData.forPurchasing.supplierId;
    request.filter["selectedProductIds"] = this._sessionData.forPurchasing.selectedProductIds;
    request.filter["purchaseAllBelowTarget"] = this._sessionData.forPurchasing.purchaseAllBelowTarget;

    return await this._api.products.getNeedsReorderingPage(request);
  }

  private async getInitializedPurchaseOrder(purchaseOrderId: string): Promise<PurchaseOrder> {
    if (purchaseOrderId) {
      return await this._api.purchaseOrders.get(purchaseOrderId);
    }
    else if (this._sessionData.forPurchasing && this._sessionData.forPurchasing.supplierId || null) {
      return await this.initializeForPurchasing();
    }
    else {
      return await this.newPurchaseOrder();
    }
  }

  public get isPurchaseOrderApproved(): boolean {
    return this.purchaseOrder && this.purchaseOrder.status >= PurchaseOrderStatus.approved;
  }

  public async activate(purchaseOrder: PurchaseOrder): Promise<void> {
    try {
      [this.paymentTypes, this.suppliers, this.statuses] = await Promise.all([
        this._api.paymentTypes.getLookups(),
        this._api.suppliers.getLookups(),
        this._api.purchaseOrders.getStatusLookup(),
      ]);
      let intializedPurchaseOrder = await this.getInitializedPurchaseOrder(purchaseOrder.id);
      this.setPurchaseOrder(intializedPurchaseOrder);
    } catch (error) {
      this._notification.warning(error);
    }
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

  public async hydrateSupplierProducts(supplier: Lookup<string>): Promise<void> {
    if (supplier == null || supplier.id == null) {
      return;
    }

    let data = await this._api.suppliers.getProductLookups(supplier.id);
    this.products = data;
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

  public async save(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to save the purchase order?').whenClosed();
      if (result.output === ActionResult.Yes) {
        // generate new receipts from receivables >> receiving items
        let newReceipts = this._api.purchaseOrders.generateNewReceiptsFrom(this.purchaseOrder);
        newReceipts.forEach(newReceipt => this.purchaseOrder.receipts.push(newReceipt));
        let data = await this._api.purchaseOrders.save(this.purchaseOrder);
        this.resetAndNoify(data, "Purchase order has been saved.");
        this._sessionData.forPurchasing = {};
      }
    }
    catch (error) {
      this._notification.warning(error)
    }
  }

  public async submit(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to submit the purchase order?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = await this._api.purchaseOrders.submit(this.purchaseOrder)
        this.resetAndNoify(data, "Purchase order has been submitted.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async approve(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to approve the purchase order?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = await this._api.purchaseOrders.approve(this.purchaseOrder);
        this.resetAndNoify(data, "Purchase order has been approved.");
        this.showVoucher();
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async showVoucher(): Promise<void> {
    let data = await this._api.purchaseOrders.getVoucher(this.purchaseOrder.id);
    this._voucherReport.show(data);
  }

  // NOTE: I HATE THIS METHOD!!! This looks like a disaster. Needed to be refactored when demo is done.
  // REFERENCE: https://github.com/kinlane/csv-converter/blob/gh-pages/json-to-csv/index.html 
  public downloadVoucherCsv(): void {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = this.purchaseOrder.items.map(x => <any>{
      Product: x.product.name,
      Unit: x.quantity.unit.name,
      Quantity: x.quantity.value,
      Cost: x.unitCostAmount,
      Total: x.totalCostAmount
    });

    var $csv = 'sep=,' + '\r\n\n';
    var showLabel = true;
    var reportTitle = "Voucher";

    //This condition will generate the Label/Header
    if (showLabel) {
      var row = "";

      //This loop will extract the label from 1st index of on array
      for (var index in arrData[0]) {

        //Now convert each value to string and comma-seprated
        row += index + ',';
      }

      row = row.slice(0, -1);

      //append Label row with line break
      $csv += row + '\r\n';
    }

    //1st loop is to extract each row
    for (var i = 0; i < arrData.length; i++) {
      var row = "";

      //2nd loop will extract each column and convert it in string comma-seprated
      for (var index in arrData[i]) {
        row += '"' + arrData[i][index] + '",';
      }

      row.slice(0, row.length - 1);

      //add a line break after each row
      $csv += row + '\r\n';
    }

    if ($csv == '') {
      alert("Invalid data");
      return;
    }

    //Generate a file name
    var fileName = formatDate(new Date);

    //this will remove the blank-spaces from the title and replace it with an underscore
    fileName += reportTitle.replace(/ /g, "_");

    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + encodeURI($csv);

    // Now the little tricky part.
    // you can use either>> window.open(uri);
    // but this will not work in some browsers
    // or you will not get the correct file extension    

    //this trick will generate a temp <a /> tag
    var link = document.createElement("a");
    link.href = uri;

    //set the visibility hidden so it will not effect on your web-layout
    //link.style = "visibility:hidden";
    link.download = fileName + ".csv";

    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  public async recreate(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to recreate the purchase order? This will go back to "Created" stage.').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = await this._api.purchaseOrders.recreate(this.purchaseOrder);
        this.resetAndNoify(data, 'You can now recreate the purchase order. It is under "Created" stage.');
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async reject(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to reject the purchase order?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = await this._api.purchaseOrders.reject(this.purchaseOrder);
        this.resetAndNoify(data, "Purchase order has been rejected.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async complete(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to complete the purchase order?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = await this._api.purchaseOrders.complete(this.purchaseOrder);
        this.resetAndNoify(data, "Purchase order has been completed.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async cancel(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to cancel the purchase order?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = await this._api.purchaseOrders.cancel(this.purchaseOrder);
        this.resetAndNoify(data, "Purchase order has been cancelled.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public async refresh(): Promise<void> {
    await this.activate(this.purchaseOrder);
  }

  public back(): void {
    this._router.navigateBack();
  }
}