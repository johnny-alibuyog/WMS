import {DialogService} from 'aurelia-dialog';
import {EventAggregator, Subscription} from 'aurelia-event-aggregator';
import {autoinject, bindable, bindingMode, customElement, computedFrom} from 'aurelia-framework'
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';
import {Lookup} from '../common/custom_types/lookup';
import {ServiceApi} from '../services/service-api';
import {Dictionary} from '../common/custom_types/dictionary';
import {PurchaseOrderPayment} from '../common/models/purchase-order';
import {NotificationService} from '../common/controls/notification-service';
import {PurchaseOrderPaymentCreate} from './purchase-order-payment-create';

@autoinject
export class PurchaseOrderPaymentPage {

  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;
  private _eventAggregator: EventAggregator;
  private _subscriptions: Subscription[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public purchaseOrderId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public payments: PurchaseOrderPayment[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public allowedTransitions: Dictionary<string> = {};

  public paymentPage: Pager<PurchaseOrderPayment> = new Pager<PurchaseOrderPayment>();

  public selectedItem: PurchaseOrderPayment;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;
    this._eventAggregator = eventAggregator;

    this.paymentPage.onPage = () => this.initializePage();
  }

  attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe('addPurchaseOrderPayment', response => this.addPayment())
    ];
  }

  detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  paymentsChanged(): void {
    this.initializePage();
  }

  initializePage(): void {
    if (!this.payments)
      this.payments = [];

    this.paymentPage.count = this.payments.length;
    this.paymentPage.items = this.payments.slice(
      this.paymentPage.start,
      this.paymentPage.end
    );
  }

  addPayment(): void {
    this._dialog.open({ viewModel: PurchaseOrderPaymentCreate, model: this.purchaseOrderId })
      .then(response => {
        if (!response.wasCancelled)
          this._eventAggregator.publish('purchaseOrderPaid');
      });
  }
}