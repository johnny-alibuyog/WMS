import { isNullOrWhiteSpace } from './../common/utils/string-helpers';
import { autoinject, bindable, bindingMode, customElement, observable } from 'aurelia-framework'
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { ProductInventory, ProductInventoryFacade } from '../common/models/product';
import { ReturnItem, returnEvents } from '../common/models/return';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { ServiceApi } from '../services/service-api';
import { ensureNumeric } from "../common/utils/ensure-numeric";
import { getValue, Measure } from "../common/models/measure";
import { pricing } from '../common/models/pricing';
import { Pager } from '../common/models/paging';
import * as Enumerable from 'linq';

export type FocusOn = "product" | "uom";

@autoinject
@customElement("return-item-page")
export class ReturnItemPage {
  private _subscriptions: Subscription[] = [];
  private _productInventories: ProductInventory[] = [];
  private _isPricingInitialized: boolean = false;

  @observable()
  public barcode: string = '';
  
  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public returnId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public isModificationDisallowed: boolean = true;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public items: ReturnItem[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public pricing: Lookup<string> = pricing.retailPrice;

  public selectedItem: ReturnItem;

  public products: Lookup<string>[] = [];

  public reasons: Lookup<string>[] = [];

  public totalReturnedAmount: number;

  public itemPager: Pager<ReturnItem> = new Pager<ReturnItem>();

  public focusBarcodeInput: boolean;

  public focusProductInput: boolean;

  public focusUomInput: boolean;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _notification: NotificationService,
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

  // private getUnitPriceAmount(product: Lookup<string>): Promise<number> {
  //   let retailPrice = pricing.retailPrice;
  //   let inventory = this._productInventories.find(x => x.id == product.id);

  //   if (inventory) {
  //     let unitPrice = pricing.getPriceAmount(retailPrice, inventory);
  //     return Promise.resolve(unitPrice);
  //   }
  //   else {
  //     return this._api.products.getInventory(product.id).then(data => {
  //       if (data) {
  //         this._productInventories.push(data);
  //       }

  //       let unitPrice = pricing.getPriceAmount(retailPrice, data);
  //       return unitPrice;
  //     });
  //   }
  // }

  public async attached(): Promise<void> {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        returnEvents.item.add,
        (focusOn?: FocusOn) => this.addItem(focusOn)
      ),
    ];

    [this.products, this.reasons] = await Promise.all([
      this._api.products.getLookups(),
      this._api.returnReasons.getLookups(),
    ]);
  }

  public detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  public itemsChanged(): void {
    debugger;
    this.initializePage();

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

  public async barcodeChangedHandler(): Promise<void> {
    let item = Enumerable
      .from(this.items)
      .firstOrDefault(x => x.barcode == this.barcode);

    let productInventory = await this.getProductInventory(this.barcode);

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
    let inventory = await this.getProductInventory(item.product.id)
    let facade = new ProductInventoryFacade(inventory);
    let current = facade.current(item.quantity.unit);
    item.standard = current.standard;
    item.unitPriceAmount = facade.getPriceAmount(inventory, item.quantity.unit, this.pricing);
    this.compute(item);
  }

  public async initializeItem(item: ReturnItem): Promise<void> {
    if (!item.product) {
      item.quantity = {
        unit: null,
        value: 0
      };
      item.standard = {
        unit: null,
        value: 0
      };
      item.unitPriceAmount = 0;
      item.returnedAmount = 0;
      this.compute(item);
      return;
    }

    let inventory = await this.getProductInventory(item.product.id);
    let facade = new ProductInventoryFacade(inventory);
    let current = facade.default;
    item.unitOfMeasures = inventory.unitOfMeasures.map(x => x.unitOfMeasure);
    item.quantity.unit = current.unitOfMeasure;
    //item.quantity.value = 0;
    item.standard.unit = current.standard.unit;
    item.standard.value = current.standard.value;
    item.unitPriceAmount = facade.getPriceAmount(inventory, item.quantity.unit, this.pricing);
    this.compute(item);
  }

  public async propagateProductChange(item: ReturnItem, productId: string): Promise<void> {
    item.product = this.products.find(x => x.id === productId);
    await this.initializeItem(item);
  }

  public pricingChanged(newValue: Lookup<string>, oldValue: Lookup<string>): void {
    if (newValue && oldValue && newValue.id == oldValue.id) {
      return;
    }

    if (!this._isPricingInitialized) {
      this._isPricingInitialized = true;
      return;
    }

    if (!this.items) {
      this.items = [];
    }

    this.items.forEach(item => this.initializeItem(item));
  }


  public initializePage(): void {
    if (!this.items)
      this.items = [];

    this.items.forEach(item => {
      if (!item.returnedAmount) {
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

  public addItem(focusOn: FocusOn = null): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (!this.items) {
      this.items = [];
    }

    var item: ReturnItem = {
      quantity: {
        unit: {},
        value: 0
      },
      standard: {
        unit: {},
        value: 0
      },
      unitPriceAmount: 0,
    };

    this.items.unshift(item);
    this.selectedItem = item;
    this.initializePage();

    this.focusProductInput = focusOn === "product";
    this.focusUomInput = focusOn === "uom";
  }

  public async editItem(item: ReturnItem): Promise<void> {
    if (this.isModificationDisallowed) {
      return;
    }

    if (item.id) {
      return; // do not allow edit of items which is already saved
    }

    if (!item.unitOfMeasures || item.unitOfMeasures.length == 0) {
      let data = await this.getProductInventory(item.product.id);
      if (data && data.unitOfMeasures) {
        item.unitOfMeasures = data.unitOfMeasures.map(x => x.unitOfMeasure);
      }
    }

    if (this.selectedItem !== item) {
      this.selectedItem = item;
    }
  }

  public deleteItem(item: ReturnItem): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (item.id) {
      return; // do not allow edit of items which is already saved
    }

    var index = this.items.indexOf(item);
    if (index > -1) {
      this.items.splice(index, 1);
    }
    this.initializePage();
  }

  public compute(item: ReturnItem): void {
    item.extendedPriceAmount = ensureNumeric(item.unitPriceAmount) * getValue(item.quantity);
    item.returnedAmount = ensureNumeric(item.extendedPriceAmount);
    this.total();
  }

  public total(): void {
    //this.taxAmount = ensureNumeric(this.taxAmount);

    //this.shippingFeeAmount = ensureNumeric(this.shippingFeeAmount);

    this.totalReturnedAmount = Enumerable.from(this.items).sum(x => x.returnedAmount);
  }
}
