import { DialogService } from 'aurelia-dialog';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { Lookup } from '../common/custom_types/lookup';
import { ServiceApi } from '../services/service-api';
import { Dictionary } from '../common/custom_types/dictionary';
import { PurchaseOrderReceipt, PurchaseOrderReceivable, PurchaseOrderReceiving, purchaseOrderEvents } from '../common/models/purchase-order';
import { NotificationService } from '../common/controls/notification-service';

@autoinject
@customElement("purchase-order-receipt-page")
export class PurchaseOrderReceiptPage {
  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;
  private _eventAggregator: EventAggregator;
  private _subscriptions: Subscription[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public purchaseOrderId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public receipts: PurchaseOrderReceipt[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public receivables: PurchaseOrderReceivable[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public products: Lookup<string>[] = [];

  public receiptPager: Pager<PurchaseOrderReceipt> = new Pager<PurchaseOrderReceipt>();

  public receivablePager: Pager<PurchaseOrderReceivable> = new Pager<PurchaseOrderReceivable>();

  public selectedItem: PurchaseOrderReceivable;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;
    this._eventAggregator = eventAggregator;

    this.receiptPager.onPage = () => this.initializeReceiptPage();
    this.receivablePager.onPage = () => this.initializeReceivablePage();
  }

  attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        purchaseOrderEvents.receipts.add,
        response => this.addItem()
      )
    ];
  }

  detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  receiptsChanged() {
    this.initializeReceiptPage();
  }

  receivablesChanged() {
    this.initializeReceivablePage();
  }

  initializeReceiptPage(): void {
    if (!this.receipts)
      this.receipts = [];

    this.receiptPager.count = this.receipts.length;
    this.receiptPager.items = this.receipts.slice(
      this.receiptPager.start,
      this.receiptPager.end
    );
  }

  initializeReceivablePage(): void {
    if (!this.receivables)
      this.receivables = [];

    this.receivablePager.count = this.receivables.length;
    this.receivablePager.items = this.receivables.slice(
      this.receivablePager.start,
      this.receivablePager.end
    );
  }

  private addItem(): void {
    if (!this.receivables)
      this.receivables = [];

    var _receivable = <PurchaseOrderReceivable>{
      purchaseOrderId: this.purchaseOrderId,
      product: null,
      orderedQuantity: 0,
      receivedQuantity: 0,
      receivableQuantity: 0,
      receiving: <PurchaseOrderReceiving>{
        batchNumber: null,
        receivedBy: this._api.auth.userAsLookup,
        receivedOn: new Date(),
        expiresOn: null,
        quantity: 0
      }
    };

    this.receivables.push(_receivable);
    this.selectedItem = _receivable;
    this.initializeReceiptPage();
    this.initializeReceivablePage();
  }

  public editItem(_receivable: PurchaseOrderReceivable): void {
    if (this.selectedItem !== _receivable)
      this.selectedItem = _receivable;
  }

  public deleteItem(_receivable: PurchaseOrderReceivable): void {
    if (_receivable.orderedQuantity > 0) {
      this._notification.error("You cannot delete receipt item that has already been ordered.");
      return;
    }

    if (_receivable.receivedQuantity > 0) {
      this._notification.error("You cannot delete receipt item that has already been received.");
      return;
    }

    var index = this.receivables.indexOf(_receivable);
    if (index > -1) {
      this.receivables.splice(index, 1);
    }
    this.initializeReceiptPage();
    this.initializeReceivablePage();
  }
}