import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'
import { pricing } from '../common/models/pricing';
import { getValue } from "../common/models/measure";
import { ensureNumeric } from '../common/utils/ensure-numeric';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Pager } from '../common/models/paging';
import { ProductInventory, ProductInventoryFacade } from "../common/models/product";
import { PurchaseOrderItem, purchaseOrderEvents } from '../common/models/purchase-order';
import { Lookup } from '../common/custom_types/lookup';
import { ServiceApi } from '../services/service-api';
import * as Enumerable from 'linq';

@autoinject
@customElement("purchase-order-item-page")
export class PurchaseOrderItemPage {

  private _subscriptions: Subscription[] = [];
  private _productInventories: ProductInventory[] = [];

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

  constructor(
    private readonly _api: ServiceApi,
    private readonly _eventAggregator: EventAggregator
  ) {
    this.itemPager.onPage = () => this.initializePage();
  }

  private async getProductInventory(product: Lookup<string>): Promise<ProductInventory> {
    let data = this._productInventories.find(x => x.id === product.id);
    if (!data) {
      data = await this._api.products.getInventory(product.id);
      this._productInventories.push(data);
    }
    return data;
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

  public async computeUnitPriceAmount(): Promise<void> {
    let item = this.selectedItem;
    let inventory = await this.getProductInventory(this.selectedItem.product);
    let facade = new ProductInventoryFacade(inventory);
    let current = facade.current(item.quantity.unit);
    item.standard = current.standard;
    item.unitCostAmount = facade.getPriceAmount(inventory, item.quantity.unit, this.pricing);
    this.compute(item);
    this.total();
  }

  public async propagateProductChange(item: PurchaseOrderItem, productId: string): Promise<void> {
    item.product = this.products.find(x => x.id === productId);
    await this.initializeItem(item);
  }

  public async initializeItem(item: PurchaseOrderItem): Promise<void> {
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

    let inventory = await this.getProductInventory(item.product);
    let facade = new ProductInventoryFacade(inventory);
    let current = facade.default;

    item.unitOfMeasures = facade.getUnitOfMeasures();
    item.quantity.unit = current.unitOfMeasure;
    //item.quantity.value = 0;
    item.standard.unit = current.standard.unit;
    item.standard.value = current.standard.value;
    item.unitCostAmount = facade.getPriceAmount(inventory, item.quantity.unit, this.pricing);
  }

  public initializePage(): void {
    if (!this.items) {
      this.items = [];
    }

    this.items.forEach(item => {
      if (!item.totalCostAmount || item.totalCostAmount == 0) {
        this.compute(item);
      }
    });

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
        value: null
      },
      standard: {
        unit: {},
        value: null
      },
      unitCostAmount: null,
    };

    this.items.unshift(item);
    this.selectedItem = item;
    this.initializePage();
  }

  public async editItem(item: PurchaseOrderItem): Promise<void> {
    if (this.isModificationDisallowed) {
      return;
    }
    if (this.selectedItem === item) {
      return;
    }

    if (!item.unitOfMeasures || item.unitOfMeasures.length == 0) {
      let data = await this.getProductInventory(item.product);
      if (data && data.unitOfMeasures) {
        item.unitOfMeasures = data.unitOfMeasures.map(x => x.unitOfMeasure);
      }
    }

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
  }

  public total(): void {
    //this.taxAmount = ensureNumeric(this.taxAmount);

    //this.shippingFeeAmount = ensureNumeric(this.shippingFeeAmount);

    //this.discountAmount = ensureNumeric(this.discountAmount);

    // debugger;
    // console.log(from);

    this.subTotalAmount = Enumerable
      .from(this.items)
      .sum(x => x.totalCostAmount);

    this.grandTotalAmount =
      ensureNumeric(this.taxAmount) +
      ensureNumeric(this.shippingFeeAmount) +
      ensureNumeric(this.subTotalAmount) -
      ensureNumeric(this.discountAmount);
  }
}
