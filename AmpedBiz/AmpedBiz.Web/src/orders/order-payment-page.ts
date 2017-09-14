import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Filter, Pager, PagerRequest, PagerResponse, SortDirection, Sorter } from '../common/models/paging';
import { OrderPayable, OrderPayment, orderEvents } from '../common/models/order';
import { autoinject, bindable, bindingMode, computedFrom, customElement } from 'aurelia-framework'

import { Dictionary } from '../common/custom_types/dictionary';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { ServiceApi } from '../services/service-api';
import { ensureNumeric } from '../common/utils/ensure-numeric';

@autoinject
@customElement("order-payment-page")
export class OrderPaymentPage {

  private _api: ServiceApi;
  private _notification: NotificationService;
  private _eventAggregator: EventAggregator;
  private _subscriptions: Subscription[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public orderId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public payments: OrderPayment[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public paymentTypes: Lookup<string>[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public isModificationDisallowed: boolean = true;

  public totalPaymentAmount: number;

  public payable: OrderPayable;

  public paymentPager: Pager<OrderPayment> = new Pager<OrderPayment>();

  public selectedItem: OrderPayment;

  constructor(api: ServiceApi, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._notification = notification;
    this._eventAggregator = eventAggregator;

    this.paymentPager.onPage = () => this.initializePage();
  }

  public orderIdChanged(): void {
    this.initializePayables();
  }

  public attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        orderEvents.payment.add,
        response => this.addItem()
      )
    ];
  }

  public detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  public paymentsChanged(): void {
    this.initializePayables();
    this.initializePage();
  }

  private initializePage(): void {
    if (!this.payments) {
      this.payments = [];
    }

    this.total();

    this.paymentPager.count = this.payments.length;
    this.paymentPager.items = this.payments.slice(
      this.paymentPager.start,
      this.paymentPager.end
    );
  }

  public initializePayables(): void {
    let requests: [Promise<OrderPayable>] = [
      this.orderId
        ? this._api.orders.getPayables(this.orderId)
        : Promise.resolve(<OrderPayable>{})
    ];

    Promise.all(requests).then((responses: [OrderPayable]) => {
      this.payable = responses[0];
    });
  }

  private addItem(): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (!this.payments) {
      this.payments = [];
    }

    var current = this.payments.find(x => !x.paymentAmount || x.paymentAmount == 0);
    if (current) {
      this.selectedItem = current;
      return;
    }

    var _payment = <OrderPayment>{
      paidOn: new Date(),
      paidTo: this._api.auth.userAsLookup,
      paymentType: this.paymentTypes && this.paymentTypes.length > 0 ? this.paymentTypes[0] : null,
      paymentAmount: this.payable && this.payable.balanceAmount - this.totalPaymentAmount || 0,
    };

    this.payments.unshift(_payment);
    this.selectedItem = _payment;
    this.initializePage();
  }

  public editItem(_payment: OrderPayment): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (_payment.id) {
      return; // do not allow edit of payment items that has already been published
    }

    if (this.selectedItem !== _payment) {
      this.selectedItem = _payment;
    }
  }

  public deleteItem(_payment: OrderPayment): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (_payment.id) {
      return; // do not allow edit of payment items that has already been published
    }

    var index = this.payments.indexOf(_payment);
    if (index > -1) {
      this.payments.splice(index, 1);
    }
    this.initializePage();
  }

  public compute(item: OrderPayment): void {
    //item.paymentAmount = 25;
    this.total();
  }

  public total(): void {
    this.totalPaymentAmount = this.payments
      .filter(item => !item.id)
      .reduce((value, current) =>
        value + ensureNumeric(current.paymentAmount), 0
      ) || 0;
  }
}