import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Pager } from '../common/models/paging';
import { PurchaseOrderPayable, PurchaseOrderPayment, purchaseOrderEvents } from '../common/models/purchase-order';
import { Lookup } from '../common/custom_types/lookup';
import { ServiceApi } from '../services/service-api';
import * as Enumerable from 'linq';

@autoinject
@customElement("purchase-order-payment-page")
export class PurchaseOrderPaymentPage {

  private _subscriptions: Subscription[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public purchaseOrderId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public payments: PurchaseOrderPayment[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public paymentTypes: Lookup<string>[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public isModificationDisallowed: boolean = true;

  public totalPaymentAmount: number;

  public payable: PurchaseOrderPayable;

  public paymentPager: Pager<PurchaseOrderPayment> = new Pager<PurchaseOrderPayment>();

  public selectedItem: PurchaseOrderPayment;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _eventAggregator: EventAggregator
  ) {
    this.paymentPager.onPage = () => this.initializePage();
  }

  public async purchaseOrderIdChanged(): Promise<void> {
    await this.loadPayables();
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

  public async paymentsChanged(): Promise<void> {
    await this.loadPayables();
    this.initializePage();
  }

  private async loadPayables(): Promise<void> {
    this.payable = this.purchaseOrderId
      ? await this._api.purchaseOrders.getPayables(this.purchaseOrderId)
      : <PurchaseOrderPayable>{};
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

    var _payment = <PurchaseOrderPayment>{
      paidOn: new Date(),
      paidBy: this._api.auth.userAsLookup,
      paymentType: this.paymentTypes && this.paymentTypes.length > 0 ? this.paymentTypes[0] : null,
      paymentAmount: this.payable && this.payable.balanceAmount - this.totalPaymentAmount || 0,
    };

    this.payments.unshift(_payment);
    this.selectedItem = _payment;
    this.initializePage();
  }

  public editItem(_payment: PurchaseOrderPayment): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (_payment.id) {
      return; // do not allow edit of payment items that has already been published
    }

    if (this.selectedItem !== _payment)
      this.selectedItem = _payment;
  }

  public deleteItem(_payment: PurchaseOrderPayment): void {
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

  public compute(item: PurchaseOrderPayment): void {
    this.total();
  }

  public total(): void {
    this.totalPaymentAmount = Enumerable
      .from(this.payments)
      .where(x => !x.id)
      .sum(x => x.paymentAmount);
  }
}
