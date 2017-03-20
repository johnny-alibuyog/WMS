import { DialogService } from 'aurelia-dialog';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { autoinject, bindable, bindingMode, customElement, computedFrom } from 'aurelia-framework'
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { Lookup } from '../common/custom_types/lookup';
import { ServiceApi } from '../services/service-api';
import { Dictionary } from '../common/custom_types/dictionary';
import { OrderItem, orderEvents } from '../common/models/order';
import { pricing } from '../common/models/pricing';
import { NotificationService } from '../common/controls/notification-service';
import { ProductInventory } from '../common/models/product';

@autoinject
@customElement("order-item-page")
export class OrderItemPage {

  private readonly _api: ServiceApi;
  private readonly _dialog: DialogService;
  private readonly _notification: NotificationService;
  private readonly _eventAggregator: EventAggregator;

  private _subscriptions: Subscription[] = [];
  private _productInventories: ProductInventory[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public orderId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public items: OrderItem[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public pricing: Lookup<string> = pricing.listPrice;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public products: Lookup<string>[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public isModificationDisallowed: boolean = true;

  public itemPager: Pager<OrderItem> = new Pager<OrderItem>();

  public selectedItem: OrderItem;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;
    this._eventAggregator = eventAggregator;

    this.itemPager.onPage = () => this.initializePage();
  }

  private getProductInventory(product: Lookup<string>): Promise<ProductInventory> {
    let productInventory = this._productInventories.find(x => x.id === product.id);

    if (productInventory) {
      return Promise.resolve(productInventory);
    }

    return this._api.products.getInventory(product.id).then(data => {
      this._productInventories.push(data);
      return data;
    });
  }

  public attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        orderEvents.item.add,
        response => this.addItem()
      ),
    ];
  }

  public detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  public itemsChanged(): void {
    this.initializePage();

    let productIds = this.items.map(x => x.product.id);
    this._api.products.getInventoryList(productIds)
      .then(result => this._productInventories = result);
  }

  public pricingChanged(): void {
    // TODO: find a better way to signal pricing change

    if (!this.items) {
      this.items = [];
    }

    this.items.forEach(item => this.initializeItem(item));
  }

  public initializeItem(item: OrderItem): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (!item.product) {
      debugger;
      item.quantityValue = 0;
      item.unitPriceAmount = 0;
      item.packagingSize = 1;
      return;
    }

    this.getProductInventory(item.product).then(inventory => {
      item.packagingSize = inventory.packagingSize;
      item.unitPriceAmount = pricing.getPriceAmount(this.pricing, inventory);
    });
  }

  public initializePage(): void {
    if (!this.items)
      this.items = [];

    this.items.forEach(item => {
      if (!item.discountRate || !item.discountAmount || !item.totalPriceAmount) {
        this.compute(item);
      }
    });

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

    if (!this.items)
      this.items = [];

    var item = <OrderItem>{
      quantityValue: 0,
      unitPriceAmountce: 0,
      packagingSize: 1,
    };

    this.items.push(item);
    this.selectedItem = item;
    this.initializePage();
  }

  public editItem(item: OrderItem): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (this.selectedItem !== item)
      this.selectedItem = item;
  }

  public deleteItem(item: OrderItem): void {
    if (this.isModificationDisallowed) {
      return;
    }

    var index = this.items.indexOf(item);
    if (index > -1) {
      this.items.splice(index, 1);
    }
    this.initializePage();
  }

  public compute(item: OrderItem): void {
    if (!item.discountRate) {
      item.discountRate = 0;
    }
    item.extendedPriceAmount = item.unitPriceAmount * item.quantityValue;
    item.discountAmount = item.discountRate * item.extendedPriceAmount;
    item.totalPriceAmount = item.extendedPriceAmount - item.discountAmount;
  }
}