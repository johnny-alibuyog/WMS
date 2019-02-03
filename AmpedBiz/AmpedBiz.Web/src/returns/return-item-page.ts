import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { ProductInventory, ProductInventoryFacade } from '../common/models/product';
import { ReturnItem, returnEvents } from '../common/models/return';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { ServiceApi } from '../services/service-api';
import { ensureNumeric } from "../common/utils/ensure-numeric";
import { getValue } from "../common/models/measure";
import { pricing } from '../common/models/pricing';
import { Pager } from '../common/models/paging';
import * as Enumerable from 'linq';

@autoinject
@customElement("return-item-page")
export class ReturnItemPage {

  private _subscriptions: Subscription[] = [];
  private _productInventories1: ProductInventory[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public returnId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public items: ReturnItem[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public pricing: Lookup<string> = pricing.basePrice;

  public selectedItem: ReturnItem;

  public products: Lookup<string>[] = [];

  public reasons: Lookup<string>[] = [];

  public totalReturnedAmount: number;

  public itemPager: Pager<ReturnItem> = new Pager<ReturnItem>();

  constructor(
    private readonly _api: ServiceApi,
    private readonly _notification: NotificationService,
    private readonly _eventAggregator: EventAggregator
  ) {
    this.itemPager.onPage = () => this.initializePage();
  }

  private async getProductInventory(product: Lookup<string>): Promise<ProductInventory> {
    let data = this._productInventories1.find(x => x.id === product.id);
    if (!data) {
      data = await this._api.products.getInventory(product.id);
      this._productInventories1.push(data);
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

  public attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        returnEvents.item.add,
        response => this.addItem()
      ),
    ];

    let requests: [
      Promise<Lookup<string>[]>,
      Promise<Lookup<string>[]>] = [
        this._api.products.getLookups(),
        this._api.returnReasons.getLookups(),
      ];

    Promise.all(requests)
      .then(responses => {
        this.products = responses[0];
        this.reasons = responses[1];
      })
      .catch(error => {
        this._notification.error(error);
      });
  }

  public detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  public itemsChanged(): void {
    this.initializePage();

    let productIds = this.items.map(x => x.product.id);
    this._api.products.getInventoryList(productIds)
      .then(result => this._productInventories1 = result);
  }

  public async computeUnitPriceAmount(): Promise<void> {
    let item = this.selectedItem;
    let inventory = await this.getProductInventory(this.selectedItem.product)
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

    let inventory = await this.getProductInventory(item.product);
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

  public addItem(): void {
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
  }

  public async editItem(item: ReturnItem): Promise<void> {
    if (item.id) {
      return; // do not allow edit of items which is already saved
    }
    
    if (!item.unitOfMeasures || item.unitOfMeasures.length == 0) {
      let data = await this.getProductInventory(item.product);
      if (data && data.unitOfMeasures) {
        item.unitOfMeasures = data.unitOfMeasures.map(x => x.unitOfMeasure);
      }
    }

    if (this.selectedItem !== item) {
      this.selectedItem = item;
    }
  }

  public deleteItem(item: ReturnItem): void {
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
