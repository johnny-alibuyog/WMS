import { DialogService } from 'aurelia-dialog';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { Lookup } from '../common/custom_types/lookup';
import { ServiceApi } from '../services/service-api';
import { Dictionary } from '../common/custom_types/dictionary';
import { PurchaseOrderItem, purchaseOrderEvents } from '../common/models/purchase-order';
import { ensureNumeric } from '../common/utils/ensure-numeric';
import { NotificationService } from '../common/controls/notification-service';

@autoinject
@customElement("purchase-order-item-page")
export class PurchaseOrderItemPage {

  private readonly _api: ServiceApi;
  private readonly _dialog: DialogService;
  private readonly _notification: NotificationService;
  private readonly _eventAggregator: EventAggregator;

  private _subscriptions: Subscription[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public purchaseOrderId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public items: PurchaseOrderItem[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public products: Lookup<string>[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public isModificationDisallowed: boolean = true;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public taxAmount: number;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public shippingFeeAmount: number;

  public discountAmount: number;

  public subTotalAmount: number;

  public grandTotalAmount: number;

  public itemPager: Pager<PurchaseOrderItem> = new Pager<PurchaseOrderItem>();

  public selectedItem: PurchaseOrderItem;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;
    this._eventAggregator = eventAggregator;

    this.itemPager.onPage = () => this.initializePage();
  }

  public attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        purchaseOrderEvents.item.add,
        response => this.addItem()
      )
    ];
  }

  public detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  public itemsChanged(): void {
    this.initializePage();
  }

  public taxAmountChanged(): void {
    this.total();
  }

  public shippingFeeAmountChanged(): void {
    this.total();
  }

  public initializeItem(item: PurchaseOrderItem): void {
    if (this.isModificationDisallowed) {
      return;
    }

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

  public initializePage(): void {
    if (!this.items) {
      this.items = [];
    }

    this.total();

    this.itemPager.count = this.items.length;
    this.itemPager.items = this.items.slice(
      this.itemPager.start,
      this.itemPager.end
    );
  }

  public addItem(): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (!this.items) {
      this.items = [];
    }

    var current = this.items.find(x => !x.totalCostAmount || x.totalCostAmount == 0);
    if (current) {
      this.selectedItem = current;
      return;
    }

    var item = <PurchaseOrderItem>{
      quantityValue: 0,
      unitPriceAmountce: 0,
    };

    this.items.unshift(item);
    this.selectedItem = item;

    this.initializePage();
  }

  public editItem(item: PurchaseOrderItem): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (this.selectedItem !== item)
      this.selectedItem = item;
  }

  public deleteItem(item: PurchaseOrderItem): void {
    if (this.isModificationDisallowed) {
      return;
    }

    var index = this.items.indexOf(item);
    if (index > -1) {
      this.items.splice(index, 1);
    }
    this.initializePage();
  }

  public compute(item: PurchaseOrderItem): void {
    item.totalCostAmount = item.unitCostAmount * item.quantityValue;

    this.total();
  }

  public total(): void {
    //this.taxAmount = ensureNumeric(this.taxAmount);

    //this.shippingFeeAmount = ensureNumeric(this.shippingFeeAmount);

    //this.discountAmount = ensureNumeric(this.discountAmount);

    this.subTotalAmount = this.items
      .reduce((value, current) => value + ensureNumeric(current.totalCostAmount), 0) || 0;
      
    this.grandTotalAmount = 
      ensureNumeric(this.taxAmount) + 
      ensureNumeric(this.shippingFeeAmount) + 
      ensureNumeric(this.subTotalAmount) - 
      ensureNumeric(this.discountAmount);
  }
}