import { DialogService } from 'aurelia-dialog';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { autoinject, bindable, bindingMode, customElement, computedFrom } from 'aurelia-framework'
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { Lookup } from '../common/custom_types/lookup';
import { ServiceApi } from '../services/service-api';
import { Dictionary } from '../common/custom_types/dictionary';
import { OrderReturn, OrderReturnable, orderEvents } from '../common/models/order';
import { pricingScheme } from '../common/models/pricing-scheme';
import { NotificationService } from '../common/controls/notification-service';

@autoinject
@customElement("order-return-page")
export class OrderReturnPage {

  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;
  private _eventAggregator: EventAggregator;
  private _subscriptions: Subscription[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public orderId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public returns: OrderReturn[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public allowedTransitions: Dictionary<string> = {};

  public returnables: OrderReturnable[] = [];

  public products: Lookup<string>[] = [];

  public returnPager: Pager<OrderReturn> = new Pager<OrderReturn>();

  public selectedItem: OrderReturn;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;
    this._eventAggregator = eventAggregator;

    this.returnPager.onPage = () => this.initializePage();
  }

  public orderIdChanged(): void {
    let requests: [Promise<OrderReturnable[]>] = [
      this.orderId ? this._api.orders.getReturnables(this.orderId) : Promise.resolve([])
    ];

    Promise.all(requests).then(responses => {
      this.returnables = responses[0];
      this.products = this.returnables.map(x => x.product);
    });
  }

  public attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        orderEvents.return.add,
        response => this.addItem()
      ),
    ];
  }

  public detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  private returnsChanged(): void {
    this.initializePage();
  }

  private initializePage(): void {
    if (!this.returns)
      this.returns = [];

    this.returns.forEach(_return => {
      if (!_return.discountRate || !_return.discountAmount || !_return.totalPriceAmount) {
        this.compute(_return);
      }
    });

    this.returnPager.count = this.returns.length;
    this.returnPager.items = this.returns.slice(
      this.returnPager.start,
      this.returnPager.end
    );
  }

  public initializeItem(_return: OrderReturn): void {
    if (!_return.product) {
      _return.returnedOn = new Date();
      _return.returnedBy = this._api.auth.userAsLookup;
      _return.quantityValue = 0;
      _return.discountRate = 0;
      _return.discountAmount = 0;
      _return.unitPriceAmount = 0;
      _return.extendedPriceAmount = 0;
      _return.totalPriceAmount = 0;
      return;
    }

    let returnable = this.returnables.find(x => x.product.id == _return.product.id);
    _return.product = returnable.product;
    _return.returnedOn = new Date();
    _return.returnedBy = this._api.auth.userAsLookup;
    _return.quantityValue = returnable.quantityValue;
    _return.discountRate = returnable.discountRate;
    _return.discountAmount = returnable.discountAmount;
    _return.unitPriceAmount = returnable.unitPriceAmount;
    _return.extendedPriceAmount = returnable.extendedPriceAmount;
    _return.totalPriceAmount = returnable.totalPriceAmount;
  }

  public addItem(): void {
    if (!this.returns)
      this.returns = <OrderReturn[]>[];

    var _return = <OrderReturn>{
      quantityValue: 0,
      unitPriceAmountce: 0,
    };

    this.returns.push(_return);
    this.selectedItem = _return;
    this.initializePage();
  }

  public editItem(_return: OrderReturn): void {
    if (this.selectedItem !== _return)
      this.selectedItem = _return;
  }

  public deleteItem(_return: OrderReturn): void {
    var index = this.returns.indexOf(_return);
    if (index > -1) {
      this.returns.splice(index, 1);
    }
    this.initializePage();
  }

  public compute(_return: OrderReturn): void {
    if (!_return.discountRate) {
      _return.discountRate = 0;
    }
    _return.extendedPriceAmount = _return.unitPriceAmount * _return.quantityValue;
    _return.discountAmount = _return.discountRate * _return.extendedPriceAmount;
    _return.totalPriceAmount = _return.extendedPriceAmount - _return.discountAmount;
  }
}