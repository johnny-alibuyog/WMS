import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Pager } from '../common/models/paging';
import { ProductInventory, ProductInventoryFacade } from "../common/models/product";
import { PurchaseOrderReceipt, PurchaseOrderReceivable, PurchaseOrderReceiving, purchaseOrderEvents } from '../common/models/purchase-order';
import { Lookup } from '../common/custom_types/lookup';
import { Measure } from "../common/models/measure";
import { NotificationService } from '../common/controls/notification-service';
import { ServiceApi } from '../services/service-api';
import * as Enumerable from 'linq';

@autoinject
@customElement("purchase-order-receipt-page")
export class PurchaseOrderReceiptPage {

  private _subscriptions: Subscription[] = [];
  private _productInventories: ProductInventory[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public purchaseOrderId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public receipts: PurchaseOrderReceipt[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public receivables: PurchaseOrderReceivable[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public products: Lookup<string>[] = [];

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public isModificationDisallowed: boolean = true;

  public receiptPager: Pager<PurchaseOrderReceipt> = new Pager<PurchaseOrderReceipt>();

  public receivablePager: Pager<PurchaseOrderReceivable> = new Pager<PurchaseOrderReceivable>();

  public selectedItem: PurchaseOrderReceivable;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _notification: NotificationService,
    private readonly _eventAggregator: EventAggregator
  ) {
    this.receiptPager.onPage = () => this.initializeReceiptPage();
    this.receivablePager.onPage = () => this.initializeReceivablePage();
  }

  public attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        purchaseOrderEvents.receipts.add,
        response => this.addItem()
      )
    ];
  }

  public detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  private async getProductInventory(product: Lookup<string>): Promise<ProductInventory> {
    if (product == null || product.id == null) {
      return null;
    }
    let data = this._productInventories.find(x => x.id === product.id);
    if (!data) {
      data = await this._api.products.getInventory(product.id);
      this._productInventories.push(data);
    }
    return data;
  }

  public receiptsChanged() {
    this.initializeReceiptPage();
  }

  public receivablesChanged() {
    this.initializeReceivablePage();
  }

  public async initializeItem(item: PurchaseOrderReceivable): Promise<void> {
    if (this.isModificationDisallowed) {
      return;
    }
    if (!item.product) {
      item.receiving.quantity = {
        unit: null,
        value: null
      };
      item.receiving.standard = {
        unit: null,
        value: null
      };
      return;
    }
    let inventory = await this.getProductInventory(item.product);
    let facade = new ProductInventoryFacade(inventory);
    let current = facade.default;
    item.unitOfMeasures = inventory.unitOfMeasures.map(x => x.unitOfMeasure);
    item.receiving.quantity.unit = current.unitOfMeasure;
    item.receiving.standard.unit = current.standard.unit;
    item.receiving.standard.value = current.standard.value;
  }

  public initializeReceiptPage(): void {
    if (!this.receipts)
      this.receipts = [];

    this.receiptPager.count = this.receipts.length;
    this.receiptPager.items = this.receipts.slice(
      this.receiptPager.start,
      this.receiptPager.end
    );
  }

  public initializeReceivablePage(): void {
    if (!this.receivables)
      this.receivables = [];

    this.receivablePager.count = this.receivables.length;
    this.receivablePager.items = this.receivables.slice(
      this.receivablePager.start,
      this.receivablePager.end
    );
  }

  private addItem(): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (!this.receivables)
      this.receivables = [];

    var current = this.receivables.find(x => !x.product && x.receiving.quantity.value == 0);
    if (current) {
      this.selectedItem = current;
      return;
    }

    var _receivable = <PurchaseOrderReceivable>{
      purchaseOrderId: this.purchaseOrderId,
      product: null,
      orderedQuantity: 0,
      receivedQuantity: 0,
      receivableQuantity: 0,
      receiving: <PurchaseOrderReceiving>{
        batchNumber: null,
        receivedBy: this._api.auth.userAsLookup,
        receivedOn: new Date(),
        expiresOn: null,
        quantity: <Measure>{
          unit: null,
          value: null
        },
        standard: <Measure>{
          unit: null,
          value: null
        }
      }
    };

    this.receivables.unshift(_receivable);
    this.selectedItem = _receivable;

    this.initializeReceiptPage();
    this.initializeReceivablePage();
  }

  public async editItem(_receivable: PurchaseOrderReceivable): Promise<void> {
    if (this.isModificationDisallowed) {
      return;
    }
    if (!_receivable.unitOfMeasures || _receivable.unitOfMeasures.length == 0) {
      let data = await this.getProductInventory(_receivable.product);
      if (data && data.unitOfMeasures) {
        _receivable.unitOfMeasures = data.unitOfMeasures.map(x => x.unitOfMeasure);
      }
    }
    if (this.selectedItem !== _receivable) {
      this.selectedItem = _receivable;
    }
  }

  public deleteItem(_receivable: PurchaseOrderReceivable): void {
    if (this.isModificationDisallowed) {
      return;
    }
    if (_receivable.orderedQuantity > 0) {
      this._notification.error("You cannot delete receipt item that has already been ordered.");
      return;
    }
    if (_receivable.receivedQuantity > 0) {
      this._notification.error("You cannot delete receipt item that has already been received.");
      return;
    }
    var index = this.receivables.indexOf(_receivable);
    if (index > -1) {
      this.receivables.splice(index, 1);
    }
    this.initializeReceiptPage();
    this.initializeReceivablePage();
  }
}