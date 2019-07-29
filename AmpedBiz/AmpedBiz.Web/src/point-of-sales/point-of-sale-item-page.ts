import { UnitOfMeasure } from './../common/models/unit-of-measure';
import { ProductRetailPriceDetails } from './../common/models/product';
import { Role, role } from './../common/models/role';
import { paymentType } from './../common/models/payment-type';
import { pricing } from './../common/models/pricing';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { Pager } from '../common/models/paging';
import { PointOfSaleItem, pointOfSaleEvents, PointOfSalePayable } from '../common/models/point-of-sale';
import { ProductInventory, ProductInventoryFacade } from '../common/models/product';
import { autoinject, bindable, bindingMode, customElement, observable } from 'aurelia-framework'
import { isNullOrWhiteSpace } from '../common/utils/string-helpers';

import { Lookup } from '../common/custom_types/lookup';
import { ServiceApi } from '../services/service-api';
import { ensureNumeric } from '../common/utils/ensure-numeric';
import { getValue, Measure } from "../common/models/measure";
import { Override, OverrideParams } from 'users/override';
import { DialogService } from 'aurelia-dialog';
import * as KeyCode from 'keycode-js';
import * as Enumerable from 'linq';
import * as $ from 'jquery';

export type FocusOn = "product" | "uom";

@autoinject
@customElement("point-of-sale-item-page")
export class PointOfSaleItemPage {

  private _subscriptions: Subscription[] = [];
  private _productInventories: ProductInventory[] = [];
  private _canEditItem: boolean = false;
  private _canDeleteItem: boolean = false;
  //private _scanner = BarcodeScanner();

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
  public payable: PointOfSalePayable;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public taxAmount: number;

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public discountRate: number;

  public discountAmount: number;

  public subTotalAmount: number;

  public grandTotalAmount: number;

  public itemPager: Pager<PointOfSaleItem> = new Pager<PointOfSaleItem>();

  @observable()
  public selectedItem: PointOfSaleItem;

  public selectedProductPriceDetails: ProductRetailPriceDetails = {};

  public focusBarcodeInput: boolean;

  public focusProductInput: boolean;

  public focusUomInput: boolean;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _dialog: DialogService,
    private readonly _eventAggregator: EventAggregator
  ) {
    this.itemPager.onPage = () => this.initializePage();
    this._canEditItem = this._api.auth.isAuthorized([role.admin, role.manager]);
    this._canDeleteItem = this._api.auth.isAuthorized([role.admin, role.manager]);
    new BarcodeScanerEvents();
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
        (focusOn?: FocusOn) => this.addItem(focusOn)
      ),
      this._eventAggregator.subscribe(
        pointOfSaleEvents.item.useOnHand,
        () => this.useOnHand()
      ),
      this._eventAggregator.subscribe(
        pointOfSaleEvents.saved,
        () => this.selectedItem = null
      ),
    ];
    window.addEventListener('keydown', this.handleKeyInput, false);

    document.addEventListener('onbarcodescaned', alert);

    // window.addEventListener('textInput', alert, false);
    // this._scanner.on(x => {
    //   debugger;
    //   console.log(x);
    //   this.addItem("uom");
    // });
  }

  public detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
    // this._scanner.off();
    // window.removeEventListener('textInput', alert);
    document.removeEventListener('onbarcodescaned', alert);

    window.removeEventListener('keydown', this.handleKeyInput);
  }

  private handleKeyInput = (event: KeyboardEvent) => {
    /* Ctrl + Alt + b */
    if (event.ctrlKey && event.altKey && event.keyCode === KeyCode.KEY_B) {
      this.focusBarcodeInput = true;
    }

    console.log(event.key);
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

  public discountRateChanged(): void {
    this.items.forEach(item => {
      item.discountRate = this.discountRate;
      this.compute(item);
    });

    this.total();
  }

  public getOnHand(unit: UnitOfMeasure): number {
    const productId = (this.selectedItem && this.selectedItem.product && this.selectedItem.product.id || null);

    const unitId = (unit && unit.id || null);

    if (!productId || !unitId) {
      return 0;
    }

    return Enumerable
      .from(this._productInventories)
      .where(x => x.id == productId)
      .selectMany(x => x.unitOfMeasures)
      .where(x => x.unitOfMeasure.id == unit.id)
      .select(x => x.onHand.value > 0
        ? x.onHand.value : 0
      )
      .firstOrDefault();
  }

  public useOnHand(unit: UnitOfMeasure = null): void {
    const onHandUnit = (unit) || (this.selectedItem && this.selectedItem.quantity && this.selectedItem.quantity.unit || null);
    if (!onHandUnit || !this.selectedItem) return;
    this.selectedItem.quantity.value = this.getOnHand(onHandUnit);
    this.compute(this.selectedItem);
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
    item.discountRate = this.discountRate;

    this.compute(item);

    setTimeout(() => this.barcode = '', 20);
  }

  public async computeUnitPriceAmount(): Promise<void> {
    let item = this.selectedItem;
    let inventory = await this.getProductInventory(item.product.id);
    let facade = new ProductInventoryFacade(inventory);
    let current = facade.current(item.quantity.unit);
    item.standard = current.standard;
    item.unitPriceAmount = facade.getPriceAmount(inventory, item.quantity.unit, this.pricing);
    this.compute(item);
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
      item.discountRate = this.discountRate;
      item.unitPriceAmount = null;
      item.unitOfMeasures = [];
      this.compute(item);
      return;
    }

    var inventory = await this.getProductInventory(item.product.id);
    var facade = new ProductInventoryFacade(inventory);
    var current = facade.standard;

    item.barcode = facade.getBarcode(inventory, current.unitOfMeasure, this.pricing);
    item.unitOfMeasures = inventory.unitOfMeasures.map(x => x.unitOfMeasure);
    item.quantity.unit = current.unitOfMeasure;
    //item.quantity.value = 0;
    item.standard.unit = current.standard.unit;
    item.standard.value = current.standard.value;
    item.discountRate = this.discountRate;
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

  public addItem(focusOn: FocusOn = null): void {
    if (this.isModificationDisallowed) {
      return;
    }

    if (!this.items) {
      this.items = [];
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
      discountAmount: this.discountRate,
      unitPriceAmount: null,
      extendedPriceAmount: null,
      totalPriceAmount: null,
    };

    this.items.unshift(item);
    this.selectedItem = item;
    this.initializePage();

    this.focusProductInput = focusOn === "product";
    this.focusUomInput = focusOn === "uom";
  }

  public async editItem(item: PointOfSaleItem): Promise<void> {
    if (this.isModificationDisallowed) {
      return;
    }

    if (this.selectedItem === item) {
      return;
    }

    if ((!item.unitOfMeasures || item.unitOfMeasures.length == 0) && item.product) {
      let data = await this.getProductInventory(item.product.id);
      if (data && data.unitOfMeasures) {
        item.unitOfMeasures = data.unitOfMeasures.map(x => x.unitOfMeasure);
      }
    }

    if (!this._canEditItem) {
      let params = <OverrideParams>{ title: "Edit Item Override" };
      let confirmation = await this._dialog.open({ viewModel: Override, model: params }).whenClosed();
      if (confirmation.wasCancelled) {
        return;
      }
    }

    this.selectedItem = item;
  }

  public async deleteItem(item: PointOfSaleItem): Promise<void> {
    if (this.isModificationDisallowed) {
      return;
    }

    if (!this._canDeleteItem) {
      let params = <OverrideParams>{ title: "Delete Item Override" };
      let confirmation = await this._dialog.open({ viewModel: Override, model: params }).whenClosed();
      if (confirmation.wasCancelled) {
        return;
      }
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

    this.payable = <PointOfSalePayable>{
      pointOfSaleId: this.pointOfSaleId,
      paymentType: paymentType.cash,
      discountRate: this.discountRate,
      discountAmount: this.discountAmount,
      subTotalAmount: this.subTotalAmount,
      totalAmount: this.grandTotalAmount
    };
  }
}


var BarcodeScanerEvents = function () {
  this.initialize.apply(this, arguments);
};

BarcodeScanerEvents.prototype = {
  initialize: function () {
    $(document).on({
      keyup: $.proxy(this._keyup, this)
    });
  },
  _timeoutHandler: 0,
  _inputString: '',
  _keyup: function (e) {
    if (this._timeoutHandler) {
      clearTimeout(this._timeoutHandler);
      this._inputString += String.fromCharCode(e.which);
    }

    this._timeoutHandler = setTimeout($.proxy(function () {
      if (this._inputString.length <= 3) {
        this._inputString = '';
        return;
      }

      $(document).trigger('onbarcodescaned', this._inputString);

      this._inputString = '';

    }, this), 20);
  }
};
