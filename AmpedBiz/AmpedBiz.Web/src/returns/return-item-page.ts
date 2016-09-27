import {DialogService} from 'aurelia-dialog';
import {EventAggregator, Subscription} from 'aurelia-event-aggregator';
import {autoinject, bindable, bindingMode, customElement, computedFrom} from 'aurelia-framework'
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';
import {Lookup} from '../common/custom_types/lookup';
import {ServiceApi} from '../services/service-api';
import {Dictionary} from '../common/custom_types/dictionary';
import {ReturnItem, returnEvents} from '../common/models/return';
import {pricingScheme} from '../common/models/pricing-scheme';
import {NotificationService} from '../common/controls/notification-service';
import {ProductInventory} from '../common/models/product';

@autoinject
@customElement("return-item-page")
export class ReturnItemPage {

  private readonly _api: ServiceApi;
  private readonly _dialog: DialogService;
  private readonly _notification: NotificationService;
  private readonly _eventAggregator: EventAggregator;

  private _subscriptions: Subscription[] = [];
  private _productInventories: ProductInventory[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public returnId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public items: ReturnItem[] = [];

  public selectedItem: ReturnItem;

  public products: Lookup<string>[] = [];

  public returnReasons: Lookup<string>[] = [];

  public itemPager: Pager<ReturnItem> = new Pager<ReturnItem>();

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, eventAggregator: EventAggregator) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;
    this._eventAggregator = eventAggregator;

    this.itemPager.onPage = () => this.initializePage();
  }

  private getUnitPriceAmount(product: Lookup<string>): Promise<number> {
    let wholesalePrice = pricingScheme.wholesalePrice;
    let inventory = this._productInventories.find(x => x.id == product.id);

    if (inventory) {
      let unitPrice = pricingScheme.getPriceAmount(wholesalePrice, inventory);
      return Promise.resolve(unitPrice);
    }
    else {
      return this._api.products.getInventory(product.id).then(data => {
        if (data) {
          this._productInventories.push(data);
        }

        let unitPrice = pricingScheme.getPriceAmount(wholesalePrice, data);
        return unitPrice;
      });
    }
  }

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
        this.returnReasons = responses[1];
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
      .then(result => this._productInventories = result);
  }

  public initializeItem(item: ReturnItem): void {
    if (!item.product) {
      item.quantityValue = 0;
      item.unitPriceAmount = 0;
      item.totalPriceAmount = 0;
      return;
    }

    this.getUnitPriceAmount(item.product).then(unitPrice => {
      item.unitPriceAmount = unitPrice;
      this.compute(item);
    });
  }

  public initializePage(): void {
    if (!this.items)
      this.items = [];

    this.items.forEach(item => {
      if (!item.totalPriceAmount) {
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
    if (!this.items)
      this.items = <ReturnItem[]>[];

    var item = <ReturnItem>{
      quantityValue: 0,
      unitPriceAmount: 0,
    };

    this.items.push(item);
    this.selectedItem = item;
    this.initializePage();
  }

  public editItem(item: ReturnItem): void {
    if (this.selectedItem !== item)
      this.selectedItem = item;
  }

  public deleteItem(item: ReturnItem): void {
    var index = this.items.indexOf(item);
    if (index > -1) {
      this.items.splice(index, 1);
    }
    this.initializePage();
  }

  public compute(item: ReturnItem): void {
    item.extendedPriceAmount = item.unitPriceAmount * item.quantityValue;
    item.totalPriceAmount = item.extendedPriceAmount;
  }
}