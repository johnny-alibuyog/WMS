import {DialogService} from 'aurelia-dialog';
import {EventAggregator, Subscription} from 'aurelia-event-aggregator';
import {autoinject, bindable, bindingMode, customElement, computedFrom} from 'aurelia-framework'
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';
import {Lookup} from '../common/custom_types/lookup';
import {ServiceApi} from '../services/service-api';
import {Dictionary} from '../common/custom_types/dictionary';
import {OrderItem} from '../common/models/order';
import {pricingScheme} from '../common/models/pricing-scheme';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class OrderItemPage {

  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;
  private _eventAggregator: EventAggregator;
  private _subscriptions: Subscription[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public purchaseOrderId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public items: OrderItem[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public pricingScheme: Lookup<string> = pricingScheme.wholesalePrice;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public products: Lookup<string>[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public allowedTransitions: Dictionary<string> = {};

  public itemPage: Pager<OrderItem> = new Pager<OrderItem>();

  public selectedItem: OrderItem;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;
    this._eventAggregator = eventAggregator;

    this.itemPage.onPage = () => this.initializePage();
  }

  attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe('addOrderItem', response => this.addItem())
    ];
  }

  detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  itemsChanged(): void {
    this.initializePage();
  }

  pricingSchemeChanged(): void {
    if (!this.items) {
      this.items = [];
    }

    // TODO: not a good idea. should make batch request. refactor soon
    this.items.forEach(item => {
      this._api.products.getInventory(item.product.id).then(data => {
        if (!this.pricingScheme)
          this.pricingScheme = pricingScheme.wholesalePrice;

        item.quantityValue = 1;
        item.unitPriceAmount = pricingScheme.getPriceAmount(this.pricingScheme, data) || 0;
        this.compute(item);
      });
    });
  }

  initializeItem(item: OrderItem): void {
    if (!item.product) {
      item.quantityValue = 0;
      item.unitPriceAmount = 0;
      return;
    }

    this._api.products.getInventory(item.product.id).then(data => {
      if (!this.pricingScheme)
        this.pricingScheme = pricingScheme.wholesalePrice;

      item.quantityValue = 1;
      item.unitPriceAmount = pricingScheme.getPriceAmount(this.pricingScheme, data) || 0;
    });
  }

  initializePage(): void {
    if (!this.items)
      this.items = [];

    this.items.forEach(item => {
      if (!item.discountRate || !item.discountAmount || !item.total) {
        this.compute(item);
      }
    });

    this.itemPage.count = this.items.length;
    this.itemPage.items = this.items.slice(
      this.itemPage.start,
      this.itemPage.end
    );
  }

  addItem(): void {
    if (!this.items)
      this.items = <OrderItem[]>[];

    var item = <OrderItem>{
      quantityValue: 0,
      unitPriceAmountce: 0,
    };

    this.items.push(item);
    this.selectedItem = item;
    this.initializePage();
  }

  editItem(item: OrderItem): void {
    if (this.selectedItem !== item)
      this.selectedItem = item;
  }

  deleteItem(item: OrderItem): void {
    var index = this.items.indexOf(item);
    if (index > -1) {
      this.items.splice(index, 1);
    }
    this.initializePage();
  }

  compute(item: OrderItem): void {
    if (!item.discountRate) {
      item.discountRate = 0;
    }
    item.extendedPriceAmount = item.unitPriceAmount * item.quantityValue;
    item.discountAmount = item.discountRate * item.extendedPriceAmount;
    item.total = item.extendedPriceAmount - item.discountAmount;
  }
}