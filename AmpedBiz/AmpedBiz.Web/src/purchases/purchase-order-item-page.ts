import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Filter, Pager, PagerRequest, PagerResponse, SortDirection, Sorter } from '../common/models/paging';
import { ProductInventory1, ProductInventoryFacade } from "../common/models/product";
import { PurchaseOrderItem, purchaseOrderEvents } from '../common/models/purchase-order';
import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'

import { Dictionary } from '../common/custom_types/dictionary';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { ServiceApi } from '../services/service-api';
import { ensureNumeric } from '../common/utils/ensure-numeric';
import { getValue } from "../common/models/measure";
import { pricing } from '../common/models/pricing';

@autoinject
@customElement("purchase-order-item-page")
export class PurchaseOrderItemPage {

  private readonly _api: ServiceApi;
  private readonly _notification: NotificationService;
  private readonly _eventAggregator: EventAggregator;

  private _subscriptions: Subscription[] = [];
  private _productInventories1: ProductInventory1[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public purchaseOrderId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public items: PurchaseOrderItem[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public pricing: Lookup<string> = pricing.basePrice;

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

  constructor(api: ServiceApi, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._notification = notification;
    this._eventAggregator = eventAggregator;

    this.itemPager.onPage = () => this.initializePage();
  }

  private getProductInventory(product: Lookup<string>): Promise<ProductInventory1> {
    let productInventory = this._productInventories1.find(x => x.id === product.id);

    if (productInventory) {
      return Promise.resolve(productInventory);
    }

    return this._api.products.getInventory1(product.id).then(data => {
      this._productInventories1.push(data);
      return data;
    });
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

  public computeUnitPriceAmount() {
    var item = this.selectedItem;
    this.getProductInventory(item.product).then(inventory => {
      var facade = new ProductInventoryFacade(inventory);
      item.unitCostAmount = facade.getPriceAmount(inventory, item.quantity.unit, this.pricing);
      this.compute(item);
    });
  }

  public initializeItem(item: PurchaseOrderItem): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (!item.product) {
      item.quantity = {
        unit: null,
        value: 0
      };
      item.standard = {
        unit: null,
        value: 0
      };
      item.unitCostAmount = 0;
      return;
    }

    this.getProductInventory(item.product).then(inventory => {
      var facade = new ProductInventoryFacade(inventory);
      var current = facade.default;

      item.unitOfMeasures = inventory.unitOfMeasures.map(x => x.unitOfMeasure);
      item.quantity.unit = current.unitOfMeasure;
      //item.quantity.value = 0;
      item.standard.unit = current.standard.unit;
      item.standard.value = current.standard.value;
      item.unitCostAmount = facade.getPriceAmount(inventory, item.quantity.unit, this.pricing);
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

    var item: PurchaseOrderItem = {
      quantity: {
        unit: {},
        value: 0
      },
      standard: {
        unit: {},
        value: 0
      },
      unitCostAmount: 0,
    };

    this.items.unshift(item);
    this.selectedItem = item;
    this.initializePage();
  }

  public editItem(item: PurchaseOrderItem): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (!item.unitOfMeasures || item.unitOfMeasures.length == 0) {
      this.getProductInventory(item.product).then(data => {
        if (data && data.unitOfMeasures) {
          item.unitOfMeasures = data.unitOfMeasures.map(x => x.unitOfMeasure);
        }
      });
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
    item.totalCostAmount = ensureNumeric(item.unitCostAmount) * getValue(item.quantity);

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