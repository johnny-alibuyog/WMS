import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Pager } from '../common/models/paging';
import { PointOfSalePayable, PointOfSalePayment, pointOfSaleEvents, initializePointOfSalePayable } from '../common/models/point-of-sale';
import { Lookup } from '../common/custom_types/lookup';
import { ServiceApi } from '../services/service-api';
import { paymentType } from '../common/models/payment-type';
import { role } from '../common/models/role';
import { Override, OverrideParams } from 'users/override';
import { DialogService } from 'aurelia-dialog';
import Enumerable from 'linq';

@autoinject
@customElement("point-of-sale-payment-page")
export class PointOfSalePaymentPage {

  private _subscriptions: Subscription[] = [];
  private readonly _canEditItem: boolean = false;
  private readonly _canDeleteItem: boolean = false;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public pointOfSaleId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public payments: PointOfSalePayment[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public paymentTypes: Lookup<string>[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public isModificationDisallowed: boolean = true;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public payable: PointOfSalePayable;

  public receivedAmount: number;

  public changeAmount: number;

  public paidAmount: number;

  public balanceAmount: number;

  public paymentPager: Pager<PointOfSalePayment> = new Pager<PointOfSalePayment>();

  public selectedItem: PointOfSalePayment;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _dialog: DialogService,
    private readonly _eventAggregator: EventAggregator
  ) {
    this.paymentPager.onPage = () => this.initializePage();
    this._canEditItem = this._api.auth.isAuthorized([role.admin, role.manager]);
    this._canDeleteItem = this._api.auth.isAuthorized([role.admin, role.manager]);
  }

  public attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        pointOfSaleEvents.payment.add,
        response => this.addItem()
      ),
      this._eventAggregator.subscribe(
        pointOfSaleEvents.saved,
        () => this.selectedItem = null
      ),
    ];
  }

  public payableChanged(newValue: PointOfSalePayable): void {
    this.total();
  }

  public paymentsChanged(): void {
    this.payable = initializePointOfSalePayable(this.payable);
    this.initializePage();
  }

  public detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
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
      this.selectedItem.focus = true;
      return;
    }

    var _payment = <PointOfSalePayment>{
      pointOfSaleId: this.pointOfSaleId,
      paymentOn: new Date(),
      paymentBy: this._api.auth.userAsLookup,
      paymentType: paymentType.cash,
      paymentAmount: 0,
    };

    this.payments.unshift(_payment);
    this.selectedItem = _payment;
    this.selectedItem.focus = true;
    this.initializePage();
  }

  public async editItem(_payment: PointOfSalePayment): Promise<void> {
    if (this.isModificationDisallowed) {
      return;
    }

    if (_payment.id) {
      return; // do not allow edit of payment items that has already been published
    }

    if (!this._canEditItem) {
      let params = <OverrideParams>{ title: "Edit Item Override" };
      let confirmation = await this._dialog.open({ viewModel: Override, model: params }).whenClosed();
      if (confirmation.wasCancelled) {
        return;
      }
    }

    if (this.selectedItem !== _payment)
      this.selectedItem = _payment;
  }

  public async deleteItem(_payment: PointOfSalePayment): Promise<void> {
    if (this.isModificationDisallowed) {
      return;
    }

    if (_payment.id) {
      return; // do not allow edit of payment items that has already been published
    }

    if (!this._canDeleteItem) {
      let params = <OverrideParams>{ title: "Delete Item Override" };
      let confirmation = await this._dialog.open({ viewModel: Override, model: params }).whenClosed();
      if (confirmation.wasCancelled) {
        return;
      }
    }

    var index = this.payments.indexOf(_payment);
    if (index > -1) {
      this.payments.splice(index, 1);
    }
    this.initializePage();
  }

  public compute(item: PointOfSalePayment): void {
    this.total();
  }

  public total(): void {
    this.receivedAmount = Enumerable
      .from(this.payments)
      .sum(x => x.paymentAmount);

    this.changeAmount = this.receivedAmount > this.payable.totalAmount
      ? this.receivedAmount - this.payable.totalAmount : 0;

    this.paidAmount = this.receivedAmount - this.changeAmount;

    this.balanceAmount = this.paidAmount < this.payable.totalAmount
      ? this.payable.totalAmount - this.paidAmount : 0;
  }
}
