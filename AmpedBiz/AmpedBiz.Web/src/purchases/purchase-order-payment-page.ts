import { DialogService } from 'aurelia-dialog';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { autoinject, bindable, bindingMode, customElement, computedFrom } from 'aurelia-framework'
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { Lookup } from '../common/custom_types/lookup';
import { ServiceApi } from '../services/service-api';
import { Dictionary } from '../common/custom_types/dictionary';
import { PurchaseOrderPayment, PurchaseOrderPayable, purchaseOrderEvents } from '../common/models/purchase-order';
import { NotificationService } from '../common/controls/notification-service';

@autoinject
@customElement("purchase-order-payment-page")
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
  public paymentTypes: Lookup<string>[] = [];

  public payable: PurchaseOrderPayable;

  public paymentPager: Pager<PurchaseOrderPayment> = new Pager<PurchaseOrderPayment>();

  public selectedItem: PurchaseOrderPayment;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;
    this._eventAggregator = eventAggregator;

    this.paymentPager.onPage = () => this.initializePage();
  }

  public purchaseOrderIdChanged(): void {

    let requests: [Promise<PurchaseOrderPayable>] = [
      this.purchaseOrderId
        ? this._api.purchaseOrders.getPayables(this.purchaseOrderId)
        : Promise.resolve(<PurchaseOrderPayable>{})
    ];

    Promise.all(requests).then((responses: [PurchaseOrderPayable]) => {
      this.payable = responses[1];
    });
  }

  public attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        purchaseOrderEvents.payment.add,
        response => this.addItem()
      )
    ];
  }

  public detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  public paymentsChanged(): void {
    this.initializePage();
  }

  private initializePage(): void {
    if (!this.payments)
      this.payments = [];

    this.paymentPager.count = this.payments.length;
    this.paymentPager.items = this.payments.slice(
      this.paymentPager.start,
      this.paymentPager.end
    );
  }

  private addItem(): void {
    if (!this.payments)
      this.payments = [];

    var _payment = <PurchaseOrderPayment>{
      paidOn: new Date(),
      paidBy: this._api.auth.userAsLookup,
      paymentType: this.paymentTypes && this.paymentTypes.length > 0 ? this.paymentTypes[0] : null,
      paymentAmount: this.payable && this.payable.balanceAmount || 0,
    };

    this.payments.push(_payment);
    this.selectedItem = _payment;
    this.initializePage();
  }

  public editItem(_payment: PurchaseOrderPayment): void {
    if (this.selectedItem !== _payment)
      this.selectedItem = _payment;
  }

  public deleteItem(_payment: PurchaseOrderPayment): void {
    var index = this.payments.indexOf(_payment);
    if (index > -1) {
      this.payments.splice(index, 1);
    }
    this.initializePage();
  }
}