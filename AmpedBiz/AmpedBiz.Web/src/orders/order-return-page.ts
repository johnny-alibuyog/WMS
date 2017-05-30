import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { BindingEngine, autoinject, bindable, bindingMode, customElement, computedFrom } from 'aurelia-framework'
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { Lookup } from '../common/custom_types/lookup';
import { ServiceApi } from '../services/service-api';
import { Dictionary } from '../common/custom_types/dictionary';
import { OrderReturn, OrderReturnable, OrderReturning, orderEvents } from '../common/models/order';
import { pricing } from '../common/models/pricing';
import { ensureNumeric } from '../common/utils/ensure-numeric';
import { NotificationService } from '../common/controls/notification-service';

@autoinject
@customElement("order-return-page")
export class OrderReturnPage {

  private _api: ServiceApi;
  private _notification: NotificationService;
  private _eventAggregator: EventAggregator;
  private _bindingEngine: BindingEngine;
  private _subscriptions: Subscription[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public orderId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public returns: OrderReturn[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public returnables: OrderReturnable[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public reasons: Lookup<string>[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public isModificationDisallowed: boolean = true;

  public totalReturnedAmount?: number;

  public totalReturningAmount?: number;

  public products: Lookup<string>[] = [];

  public returnPager: Pager<OrderReturn> = new Pager<OrderReturn>();

  public returnablePager: Pager<OrderReturnable> = new Pager<OrderReturnable>();

  public selectedItem: OrderReturnable;

  constructor(api: ServiceApi,  notification: NotificationService, eventAggregator: EventAggregator, bindingEngine: BindingEngine) {
    this._api = api;
    this._notification = notification;
    this._eventAggregator = eventAggregator;
    this._bindingEngine = bindingEngine;

    this.returnPager.onPage = () => this.initializeReturnablePage();
    this.returnablePager.onPage = () => this.initializeReturnablePage();
  }

  public attached(): void {
    this._subscriptions = [
      /* do not allow adding anymore
      this._eventAggregator.subscribe(
        orderEvents.return.add,
        response => this.addItem()
      ),
      */
    ];
  }

  public detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  public reasonsChanged(newValue: Lookup<string>[], oldValue: Lookup<string>[]): void {
    console.log(newValue);
  }

  public returnsChanged(newValue: OrderReturn[], oldValue: OrderReturn[]): void {
    this.initializeReturnPage();
  }

  public returnablesChanged(newValue: OrderReturnable[], oldValue: OrderReturnable[]): void {
    if ((!oldValue || oldValue.length == 0) && (newValue && newValue.length > 0)) {

      /*
      this._referenceReturnables = this.returnables;
      this.returnables = [];

      // Display only those items with returns. If product is to be returned, add it using the add button;
      this.returnables = this._referenceReturnables
        .filter(x => x.returnedQuantity > 0)
        .map(x => Object.assign(<OrderReturnable>{}, x));
      */

      // You can return items that are returnable.
      this.products = newValue
        .filter(x => x.returnableQuantity > 0)
        .map(x => x.product);

    }

    this.initializeReturnablePage();
  }

  private initializeReturnPage(): void {
    if (!this.returns) {
      this.returns = [];
    }

    this.total();

    this.returnPager.count = this.returns.length;
    this.returnPager.items = this.returns.slice(
      this.returnPager.start,
      this.returnPager.end
    );
  }

  private initializeReturnablePage(): void {
    if (!this.returnables) {
      this.returnables = [];
    }

    this.total();

    this.returnablePager.count = this.returnables.length;
    this.returnablePager.items = this.returnables.slice(
      this.returnablePager.start,
      this.returnablePager.end
    );
  }

  public initializeItem(returnable: OrderReturnable): void {
    let reference = this.returnables.find(x =>
      returnable.product &&
      returnable.product.id == x.product.id
    );

    returnable.orderId = this.orderId;
    returnable.product = reference && reference.product || null;
    returnable.discountRate = reference && reference.discountRate || 0;
    returnable.discountAmount = reference && reference.discountAmount || 0;
    returnable.unitPriceAmount = reference && reference.unitPriceAmount || 0;
    returnable.extendedPriceAmount = reference && reference.extendedPriceAmount || 0;
    returnable.totalPriceAmount = reference && reference.totalPriceAmount || 0;
    returnable.orderedQuantity = reference && reference.orderedQuantity || 0;
    returnable.returnedQuantity = reference && reference.returnedQuantity || 0;
    returnable.returnableQuantity = reference && reference.returnableQuantity || 0;
    returnable.returning.reason = reference && reference.returning && reference.returning.reason || null;
    returnable.returning.returnedOn = reference && reference.returning && reference.returning.returnedOn || new Date();
    returnable.returning.returnedBy = reference && reference.returning && reference.returning.returnedBy || this._api.auth.userAsLookup;
    returnable.returning.quantity = reference && reference.returning && reference.returning.quantity || 0;
    returnable.returning.amount = reference && reference.returning && reference.returning.amount || 0;

    /*
    if (!_returnable.product ;
      let reference = this._referenceReturnables.find(x => x.product.id == _returnable.product.id);
      var destination = <OrderReturnable>{};
      _returnable = Object.assign(_returnable, source);
      var source = {

      };

      Object.assign(<OrderReturnable>{}, reference);
      Object.assign(<OrderReturning>{}, reference.returning);
    }
    else {
      Object.assign(_returnable, this._returnableDefault);
    }
    */
  }

  public addItem(): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (!this.returnables) {
      this.returnables = [];
    }

    var _returnable = <OrderReturnable>{
      orderId: this.orderId,
      product: null,
      discountRate: 0,
      discountAmount: 0,
      unitPriceAmount: 0,
      extendedPriceAmount: 0,
      totalPriceAmount: 0,
      orderedQuantity: 0,
      returnedQuantity: 0,
      returnableQuantity: 0,
      returning: <OrderReturning>{
        reason: null,
        returnedOn: new Date(),
        returnedBy: this._api.auth.userAsLookup,
        quantity: 0,
        amount: 0
      }
    };

    this.returnables.unshift(_returnable);
    this.selectedItem = _returnable;
    this.initializeReturnablePage();
  }

  public editItem(_returnable: OrderReturnable): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (_returnable.returnableQuantity == 0) {
      return;
    }

    if (this.selectedItem !== _returnable)
      this.selectedItem = _returnable;
  }

  public deleteItem(_returnable: OrderReturnable): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (_returnable.returnedQuantity > 0) {
      this._notification.error("You cannot delete return that has already been processed.");
      return;
    }

    var index = this.returnables.indexOf(_returnable);
    if (index > -1) {
      this.returnables.splice(index, 1);
    }
    this.initializeReturnablePage();
  }

  public compute(item: OrderReturnable): void {
    var unitPrice = item.totalPriceAmount / item.orderedQuantity;
    item.returning.amount = unitPrice * item.returning.quantity;
    this.total();
  }

  public total() {
    this.totalReturnedAmount = this.returns
      .reduce((value, item) => value + ensureNumeric(item.returnedAmount), 0) || 0;

    this.totalReturningAmount = this.returnables
      .reduce((value, item) => value + ensureNumeric(item.returning.amount), 0) || 0;
  }
}