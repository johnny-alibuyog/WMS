import {DialogService} from 'aurelia-dialog';
import {EventAggregator} from 'aurelia-event-aggregator';
import {autoinject, bindable, bindingMode, customElement, computedFrom} from 'aurelia-framework'
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';
import {Lookup} from '../common/custom_types/lookup';
import {ServiceApi} from '../services/service-api';
import {Dictionary} from '../common/custom_types/dictionary';
import {PurchaseOrderItem} from '../common/models/purchase-order';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class PurchaseOrderItemPage {

  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;
  private _eventAggregator: EventAggregator;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public purchaseOrderId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public items: PurchaseOrderItem[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public products: Lookup<string>[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public allowedTransitions: Dictionary<string> = {};

  public itemPage: Pager<PurchaseOrderItem> = new Pager<PurchaseOrderItem>();

  public selectedItem: PurchaseOrderItem;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;
    this._eventAggregator = eventAggregator;

    this.itemPage.onPage = () => this.initializePage();
    this._eventAggregator.subscribe('addPurchaseOrderItem', response => this.addItem());
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
      item.quantityValue = data.targetValue || 1;
      item.unitCostAmount = data.basePriceAmount || 0;
    });
  }

  initializePage(): void {
    if (!this.items)
      this.items = [];

    this.itemPage.count = this.items.length;
    this.itemPage.items = this.items.slice(
      this.itemPage.start,
      this.itemPage.end
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

    //this.itemPage.offset = this.itemPage.end;
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