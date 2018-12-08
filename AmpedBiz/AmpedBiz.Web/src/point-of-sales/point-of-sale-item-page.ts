import { pricing } from './../common/models/pricing';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Pager } from '../common/models/paging';
import { PointOfSaleItem, pointOfSaleEvents } from '../common/models/point-of-sale';
import { ProductInventory, ProductInventoryFacade } from '../common/models/product';
import { autoinject, bindable, bindingMode, customElement, observable } from 'aurelia-framework'
import { isNullOrWhiteSpace } from '../common/utils/string-helpers';

import { Lookup } from '../common/custom_types/lookup';
import { ServiceApi } from '../services/service-api';
import { UnitOfMeasure } from "../common/models/unit-of-measure";
import { ensureNumeric } from '../common/utils/ensure-numeric';
import { getValue, Measure } from "../common/models/measure";
import * as Enumerable from 'linq';


type ProductAndUnitOfMeasure = {
  product: Lookup<string>,
  unit: UnitOfMeasure
};

@autoinject
@customElement("point-of-sale-item-page")
export class PointOfSaleItemPage {

  private _subscriptions: Subscription[] = [];
  private _productInventories: ProductInventory[] = [];
  private _isPricingInitialized: boolean = false;

  @observable()
  public barcode: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public pointOfSaleId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public items: PointOfSaleItem[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public pricing: Lookup<string> = pricing.retailPrice;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public products: Lookup<string>[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public isModificationDisallowed: boolean = true;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public taxAmount: number;

  public discountAmount: number;

  public subTotalAmount: number;

  public grandTotalAmount: number;

  public itemPager: Pager<PointOfSaleItem> = new Pager<PointOfSaleItem>();

  public selectedItem: PointOfSaleItem;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _eventAggregator: EventAggregator
  ) {
    this.itemPager.onPage = () => this.initializePage();
  }

  private async getProductInventory(key: string): Promise<ProductInventory> {
    let data = Enumerable
      .from(this._productInventories)
      .where(x =>
        x.id === key ||
        Enumerable.from(x.unitOfMeasures).any(o => o.barcode == key)
      )
      .firstOrDefault();

    if (!data) {
      data = await this._api.products.getInventory(key);
      if (data) {
        this._productInventories.push(data);
      }
    }
    return data;
  }

  public attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        pointOfSaleEvents.item.add,
        () => this.addItem()
      ),
    ];
  }

  public detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  public itemsChanged(): void {
    this.initializePage();

    if (!this.items || this.items.length == 0) {
      return;
    }

    let productIds = this.items.map(x => x.product.id);
    this._api.products.getInventoryList(productIds)
      .then(result => this._productInventories = result);
  }

  public barcodeChanged(newValue: string, oldValue: string) {
    if (isNullOrWhiteSpace(newValue)) {
      return;
    }

    this.barcodeChangedHandler();
  }

  public taxAmountChanged(): void {
    this.total();
  }

  public shippingFeeAmountChanged(): void {
    this.total();
  }

  public pricingChanged(newValue: Lookup<string>, oldValue: Lookup<string>): void {
    if (newValue && oldValue && newValue.id == oldValue.id) {
      return;
    }

    if (!this._isPricingInitialized) {
      this._isPricingInitialized = true;
      return;
    }

    if (this.isModificationDisallowed) {
      return;
    }

    if (!this.items) {
      this.items = [];
    }

    this.items.forEach(item => this.initializeItem(item));
  }

  public async barcodeChangedHandler(): Promise<void> {
    let item = Enumerable
      .from(this.items)
      .firstOrDefault(x => x.barcode == this.barcode);

    debugger;
    var productInventory = await this.getProductInventory(this.barcode);

    if (!productInventory) {
      return;
    }

    if (item == null) {
      this.addItem();
      item = this.selectedItem;
      item.barcode = this.barcode;
    }
    else {
      this.selectedItem = item;
    }

    if (!item.product) {
      item.product = Enumerable
        .from(this.products)
        .firstOrDefault(x => x.id === productInventory.id);
    }

    if (!item.unitOfMeasures || item.unitOfMeasures.length == 0) {
      item.unitOfMeasures = productInventory.unitOfMeasures.map(x => x.unitOfMeasure);
    }

    let productUnitOfMeasure = Enumerable
      .from(productInventory.unitOfMeasures)
      .firstOrDefault(x => x.barcode == this.barcode);

    if (!item.standard || !item.standard.unit || !item.standard.unit.id) {
      item.standard = <Measure>{
        unit: productUnitOfMeasure.standard.unit,
        value: productUnitOfMeasure.standard.value
      }
    }

    if (!item.quantity || !item.quantity.unit || !item.quantity.unit.id) {
      item.quantity = <Measure>{
        unit: productUnitOfMeasure.unitOfMeasure,
        value: 0
      };
    }

    if (!item.unitPriceAmount || item.unitPriceAmount == 0) {
      let facade = new ProductInventoryFacade(productInventory);
      item.unitPriceAmount = facade.getPriceAmount(productInventory, item.quantity.unit, this.pricing);
    }

    item.quantity.value += 1;

    this.compute(item);

    setTimeout(() => this.barcode = '', 20);
  }

  public async computeUnitPriceAmount(): Promise<void> {
    let item = this.selectedItem;
    let inventory = await this.getProductInventory(item.product.id);
    let facade = new ProductInventoryFacade(inventory);
    item.unitPriceAmount = facade.getPriceAmount(inventory, item.quantity.unit, this.pricing);
    this.compute(item);
  }

  // we added unit parameter just to make the binding watch when ever item.quantity.unit changes
  public getUnitOfMeasures(item: PointOfSaleItem): UnitOfMeasure[] {
    if (!item.product) {
      return [];
    }

    var productInventory = this._productInventories.find(x => x.id === item.product.id);
    if (!productInventory) {
      return [];
    }

    var productUnitOfMeasures = productInventory.unitOfMeasures;
    if (!productUnitOfMeasures) {
      return [];
    }

    return productUnitOfMeasures.map(x => x.unitOfMeasure);
  }

  public propagateProductChange(item: PointOfSaleItem, productId: string) {
    item.product = this.products.find(x => x.id === productId);
    this.initializeItem(item);
  }

  public async initializeItem(item: PointOfSaleItem): Promise<void> {
    if (this.isModificationDisallowed) {
      return;
    }

    if (!item.product) {
      item.quantity = {
        unit: {},
        value: null
      };
      item.standard = {
        unit: {},
        value: null
      };
      item.unitPriceAmount = null;
      item.unitOfMeasures = [];
      this.compute(item);
      return;
    }

    var inventory = await this.getProductInventory(item.product.id);
    var facade = new ProductInventoryFacade(inventory);
    var current = facade.default;

    item.barcode = facade.getBarcode(inventory, current.unitOfMeasure, this.pricing);
    item.unitOfMeasures = inventory.unitOfMeasures.map(x => x.unitOfMeasure);
    item.quantity.unit = current.unitOfMeasure;
    //item.quantity.value = 0;
    item.standard.unit = current.standard.unit;
    item.standard.value = current.standard.value;
    item.unitPriceAmount = facade.getPriceAmount(inventory, item.quantity.unit, this.pricing);
    this.compute(item);
  }

  public initializePage(): void {
    if (!this.items)
      this.items = [];

    this.items.forEach(item => {
      if (!item.discountRate || !item.discountAmount || !item.totalPriceAmount) {
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

    var current = this.items.find(x => !x.totalPriceAmount || x.totalPriceAmount == 0);
    if (current) {
      this.selectedItem = current;
      return;
    }

    var item: PointOfSaleItem = {
      barcode: '',
      quantity: {
        unit: {},
        value: null
      },
      standard: {
        unit: {},
        value: null
      },
      unitOfMeasures: [],
      discountRate: null,
      discountAmount: null,
      unitPriceAmount: null,
      extendedPriceAmount: null,
      totalPriceAmount: null,
    };

    this.items.unshift(item);
    this.selectedItem = item;
    this.initializePage();
  }

  public editItem(item: PointOfSaleItem): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (this.selectedItem === item) {
      return;
    }

    if (!item.unitOfMeasures || item.unitOfMeasures.length == 0) {
      this.getProductInventory(item.product.id).then(data => {
        if (data && data.unitOfMeasures) {
          item.unitOfMeasures = data.unitOfMeasures.map(x => x.unitOfMeasure);
        }
      });
    }

    this.selectedItem = item;
  }

  public deleteItem(item: PointOfSaleItem): void {
    if (this.isModificationDisallowed) {
      return;
    }

    var index = this.items.indexOf(item);
    if (index > -1) {
      this.items.splice(index, 1);
    }
    this.initializePage();
  }

  public compute(item: PointOfSaleItem): void {
    // if (!item.discountRate) {
    //   item.discountRate = 0;
    // }
    item.extendedPriceAmount = ensureNumeric(item.unitPriceAmount) * getValue(item.quantity);
    item.discountAmount = ensureNumeric(item.discountRate) * ensureNumeric(item.extendedPriceAmount);
    item.totalPriceAmount = ensureNumeric(item.extendedPriceAmount) - ensureNumeric(item.discountAmount);

    this.total();
  }

  public total(): void {
    this.discountAmount = Enumerable.from(this.items).sum(x => x.discountAmount);

    this.subTotalAmount = Enumerable.from(this.items).sum(x => x.totalPriceAmount);

    this.grandTotalAmount =
      ensureNumeric(this.taxAmount) +
      ensureNumeric(this.subTotalAmount) -
      ensureNumeric(this.discountAmount);
  }
}
