import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {Lookup} from '../common/custom_types/lookup';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';
import {PurchaseOrder, PurchaseOrderReceipt, PurchaseOrderReceivable} from '../common/models/purchase-order';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class PurchaseOrderReceiptCreate {
  private _api: ServiceApi;
  private _controller: DialogController;
  private _notification: NotificationService;
  private _purchaseOrderId: string;

  public header: string = 'Receipt';
  public canSave: boolean = true;
  public products: Lookup<string>[];
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

  activate(purchaseOrderId: string) {
    /*
    this._purchaseOrder = purchaseOrder;
    this.receivables = this._purchaseOrder.receivables;
    this.products = this.receivables.map((item) => item.product);
    */

    let requests: [Promise<PurchaseOrderReceivable[]>] = [
      this._api.purchaseOrders.getReceivables(purchaseOrderId)
    ];

    Promise.all(requests).then((results: [PurchaseOrderReceivable[]]) => {
      this._purchaseOrderId = purchaseOrderId;
      this.receivables = results[0];
      this.products = this.receivables.map((item) => item.product);

      this.initializePage();
    });
  }

  cancel() {
    this._controller.cancel();
  }

  editItem(item: PurchaseOrderReceivable): void {
    if (this.selectedItem !== item)
      this.selectedItem = item;
  }

  resetItem(item: PurchaseOrderReceivable): void {
    item.receivingQuantity = item.receivableQuantity;
  }

  initializePage(): void {
    this.receivablePage.count = this.receivables.length;
    this.receivablePage.items = this.receivables.slice(
      this.receivablePage.start,
      this.receivablePage.end
    );
  }

  save() {
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
  }
}