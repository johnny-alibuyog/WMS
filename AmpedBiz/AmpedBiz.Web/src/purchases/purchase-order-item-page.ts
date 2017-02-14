import {DialogService} from 'aurelia-dialog';
import {EventAggregator, Subscription} from 'aurelia-event-aggregator';
import {autoinject, bindable, bindingMode, customElement} from 'aurelia-framework'
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';
import {Lookup} from '../common/custom_types/lookup';
import {ServiceApi} from '../services/service-api';
import {Dictionary} from '../common/custom_types/dictionary';
import {PurchaseOrderItem, purchaseOrderEvents} from '../common/models/purchase-order';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
@customElement("purchase-order-item-page")
export class PurchaseOrderItemPage {

  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;
  private _eventAggregator: EventAggregator;
  private _subscriptions: Subscription[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public purchaseOrderId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public items: PurchaseOrderItem[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public products: Lookup<string>[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public allowedTransitions: Dictionary<string> = {};

  public itemPager: Pager<PurchaseOrderItem> = new Pager<PurchaseOrderItem>();

  public selectedItem: PurchaseOrderItem;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;
    this._eventAggregator = eventAggregator;

    this.itemPager.onPage = () => this.initializePage();
  }

  attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        purchaseOrderEvents.item.add,
        response => this.addItem()
      )
    ];
  }

  detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  itemsChanged(): void {
    this.initializePage();
  }

  initializeItem(item: PurchaseOrderItem): void {
    if (!item.product) {
      item.quantityValue = 0;
      item.unitCostAmount = 0;
      return;
    }

    this._api.products.getInventory(item.product.id).then(data => {
      item.quantityValue = 0;
      item.unitCostAmount = data.basePriceAmount || 0;
    });
  }

  initializePage(): void {
    if (!this.items)
      this.items = [];

    this.itemPager.count = this.items.length;
    this.itemPager.items = this.items.slice(
      this.itemPager.start,
      this.itemPager.end
    );
  }

  addItem(): void {
    if (!this.items)
      this.items = <PurchaseOrderItem[]>[];

    var item = <PurchaseOrderItem>{
      quantityValue: 0,
      unitPriceAmountce: 0,
    };

    this.items.push(item);
    this.selectedItem = item;

    this.initializePage();
  }

  editItem(item: PurchaseOrderItem): void {
    if (this.selectedItem !== item)
      this.selectedItem = item;
  }

  deleteItem(item: PurchaseOrderItem): void {
    var index = this.items.indexOf(item);
    if (index > -1) {
      this.items.splice(index, 1);
    }
    this.initializePage();
  }
}