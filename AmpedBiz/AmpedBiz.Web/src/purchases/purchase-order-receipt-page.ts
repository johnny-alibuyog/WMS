import {DialogService} from 'aurelia-dialog';
import {EventAggregator, Subscription} from 'aurelia-event-aggregator';
import {autoinject, bindable, bindingMode, customElement, computedFrom} from 'aurelia-framework'
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';
import {Lookup} from '../common/custom_types/lookup';
import {ServiceApi} from '../services/service-api';
import {Dictionary} from '../common/custom_types/dictionary';
import {PurchaseOrderReceipt, PurchaseOrderReceivable} from '../common/models/purchase-order';
import {NotificationService} from '../common/controls/notification-service';
import {PurchaseOrderReceiptCreate} from './purchase-order-receipt-create';

@autoinject
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
  public allowedTransitions: Dictionary<string> = {};

  public receiptPage: Pager<PurchaseOrderReceipt> = new Pager<PurchaseOrderReceipt>();

  public receivablePage: Pager<PurchaseOrderReceivable> = new Pager<PurchaseOrderReceivable>();

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;
    this._eventAggregator = eventAggregator;

    this.receiptPage.onPage = () => this.initializeReceiptPage();
    this.receivablePage.onPage = () => this.initializeReceivablePage();
    
  }

  attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe('addPurchaseOrderReceipt', response => this.addReceipt())
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

    this.receiptPage.count = this.receipts.length;
    this.receiptPage.items = this.receipts.slice(
      this.receiptPage.start,
      this.receiptPage.end
    );
  }

  initializeReceivablePage(): void {
    if (!this.receivables)
      this.receivables = [];

    this.receivablePage.count = this.receivables.length;
    this.receivablePage.items = this.receivables.slice(
      this.receivablePage.start,
      this.receivablePage.end
    );
  }

  addReceipt(): void {
    this._dialog.open({ viewModel: PurchaseOrderReceiptCreate, model: this.purchaseOrderId })
      .then(response => { if (!response.wasCancelled) this._eventAggregator.publish('purchaseOrderReceived'); });
  }

}