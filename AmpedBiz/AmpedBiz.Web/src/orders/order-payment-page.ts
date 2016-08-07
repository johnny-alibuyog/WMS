import {DialogService} from 'aurelia-dialog';
import {EventAggregator, Subscription} from 'aurelia-event-aggregator';
import {autoinject, bindable, bindingMode, customElement, computedFrom} from 'aurelia-framework'
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';
import {Lookup} from '../common/custom_types/lookup';
import {ServiceApi} from '../services/service-api';
import {Dictionary} from '../common/custom_types/dictionary';
import {OrderPayment, orderEvents} from '../common/models/order';
import {NotificationService} from '../common/controls/notification-service';
import {OrderPaymentCreate} from './order-payment-create';

@autoinject
@customElement("order-payment-page")
export class OrderPaymentPage {

  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;
  private _eventAggregator: EventAggregator;
  private _subscriptions: Subscription[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public orderId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public payments: OrderPayment[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public allowedTransitions: Dictionary<string> = {};

  public paymentPager: Pager<OrderPayment> = new Pager<OrderPayment>();

  public selectedItem: OrderPayment;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;
    this._eventAggregator = eventAggregator;

    this.paymentPager.onPage = () => this.initializePage();
  }

  attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        orderEvents.payment.pay, 
        response => this.addPayment()
      )
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

    this.paymentPager.count = this.payments.length;
    this.paymentPager.items = this.payments.slice(
      this.paymentPager.start,
      this.paymentPager.end
    );
  }

  addPayment(): void {
    this._dialog.open({ viewModel: OrderPaymentCreate, model: this.orderId })
      .then(response => {
        if (!response.wasCancelled)
          this._eventAggregator.publish(orderEvents.payment.paid, response.output);
      });
  }
}