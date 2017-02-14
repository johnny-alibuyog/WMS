import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { Lookup } from '../common/custom_types/lookup';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { PurchaseOrder, PurchaseOrderReceipt, PurchaseOrderReceivable, ReceivingDetails } from '../common/models/purchase-order';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class PurchaseOrderReceiving {
  private _api: ServiceApi;
  private _controller: DialogController;
  private _notification: NotificationService;
  private _purchaseOrderId: string;

  public header: string = 'Receipt';
  public canSave: boolean = true;
  public batchNumber: string;
  public selectedItem: PurchaseOrderReceivable;
  public receivables: PurchaseOrderReceivable[];
  public receivablePage: Pager<PurchaseOrderReceivable> = new Pager<PurchaseOrderReceivable>();

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;

    this.receivablePage.onPage = () => this.initializePage();
  }

  activate(receivables: PurchaseOrderReceivable[]) {
    if (!receivables || receivables.length == 0) {
      return;
    }

    this.receivables = <PurchaseOrderReceivable[]>JSON.parse(JSON.stringify(receivables)); // deep copy
    this.receivables.forEach(item => this.resetItem(item));
    this.selectedItem = this.receivables[0];
    this.initializePage();
  }

  cancel() {
    this._controller.cancel();
  }

  editItem(item: PurchaseOrderReceivable): void {
    if (this.selectedItem !== item) {
      this.selectedItem = item;
    }
  }

  resetItem(item: PurchaseOrderReceivable): void {
    item.receiving = item.receiving || <ReceivingDetails>{};
    item.receiving.quantity = item.receivableQuantity;
  }

  initializePage(): void {
    this.receivablePage.count = this.receivables.length;
    this.receivablePage.items = this.receivables.slice(
      this.receivablePage.start,
      this.receivablePage.end
    );
  }

  receive() {
    this.receivables.forEach(item => {
      item.receiving = item.receiving || <ReceivingDetails>{};
      item.receiving.batchNumber = this.batchNumber;
      item.receiving.receivedBy = this._api.auth.userAsLookup;
      item.receiving.receivedOn = new Date();
    });
    this._controller.close(true, this.receivables);

    /*
    this._api.purchaseOrders
      .receive(<PurchaseOrder>{
        id: this._purchaseOrderId,
        receipts: this.receivables
          .filter(x => x.receivingQuantity > 0)
          .map(x => <PurchaseOrderReceipt>{
            purchaseOrderId: this._purchaseOrderId,
            batchNumber: this.batchNumber,
            receivedBy: this._api.auth.userAsLookup,
            receivedOn: new Date(),
            product: x.product,
            expiresOn: x.expiresOn,
            quantityValue: x.receivingQuantity
          })
      })
      .then(data => {
        this._notification.success("Purchase order has been received.")
          .then(response => this._controller.close(true, data));
      })
      .catch(error => {
        this._notification.warning(error);
      });
    */
  }
}