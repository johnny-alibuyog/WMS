import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'
import { ProductUnitOfMeasure, ProductUnitOfMeasurePrice } from "../common/models/product";
import { Lookup } from '../common/custom_types/lookup';
import { Router } from 'aurelia-router';
import { ServiceApi } from '../services/service-api';
import { pricing } from '../common/models/pricing';

@autoinject
@customElement("product-uom")
export class ProductUOM {

  @bindable()
  public productId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public items: ProductUnitOfMeasure[];

  public selected: ProductUnitOfMeasure;

  public selectedPrice: ProductUnitOfMeasurePrice;

  public get standardName(): string {
    var standardItem = this.items && this.items.find(x => x.isStandard) || null;
    return standardItem && standardItem.unitOfMeasure && standardItem.unitOfMeasure.name || 'Item';
  }

  public get defaultName(): string {
    var defaultItem = this.items && this.items.find(x => x.isDefault) || null;
    return defaultItem && defaultItem.unitOfMeasure && defaultItem.unitOfMeasure.name || 'Package';
  }

  public lookups = {
    pricing: {
      original: <Lookup<string>[]>[],
      items: <Lookup<string>[]>[],
      selected: <Lookup<string>>{},
    },
    unitOfMeasure: {
      original: <Lookup<string>[]>[],
      items: <Lookup<string>[]>[],
      selected: <Lookup<string>>{},
    }
  };

  constructor(private readonly _api: ServiceApi) { }

  public selectPrice(item: ProductUnitOfMeasurePrice): void {
    this.selectedPrice = item;
  }

  public select(item: ProductUnitOfMeasure): void {
    this.selected = item;
  }

  public insert(): void {
    let unitOfMeasure = this.lookups.unitOfMeasure.selected;

    if (!unitOfMeasure) {
      return;
    }

    let exists = this.items.some(x => x.unitOfMeasure.id === unitOfMeasure.id);
    if (exists) {
      return;
    }

    let item: ProductUnitOfMeasure = {
      unitOfMeasure: this.lookups.unitOfMeasure.selected,
      isStandard: this.items.length === 0,
      isDefault: this.items.length === 0,
      standardEquivalentValue: this.items.length === 0 ? 1 : 0,
      prices: this.lookups.pricing.items
        .map(x => <ProductUnitOfMeasurePrice>{
          pricing: x,
          priceAmount: 0,
        }),
    };

    this.items.unshift(item);
    this.resetSelected();
  }

  public remove(item: ProductUnitOfMeasure): void {
    var index = this.items.indexOf(item);
    if (index > -1) {
      this.items.splice(index, 1);
      this.resetSelected();
    }
  }

  public toggleDefault(item: ProductUnitOfMeasure): boolean {
    /* only one item should be default, the rest is not  */
    this.items.filter(x => x !== item).forEach(x => x.isDefault = false);
    return true; // continue propagtion to UI
  }

  public toggleStandard(item: ProductUnitOfMeasure): boolean {
    /* only one item should be standard, the rest is not  */
    item.standardEquivalentValue = 1;
    this.items.filter(x => x !== item).forEach(x => x.isStandard = false);
    return true; // continue propagtion to UI
  }

  public itemsChanged(): void {
    this.resetSelected();
  }

  private resetSelected = () => {
    if (this.items && this.items.length > 0) {
      this.selected = this.items[0];
    }
    else {
      this.selected = null;
    }
    this.resetUnitOfMeasureItems();
  }

  private resetUnitOfMeasureItems(): void {
    let newItems = this.lookups.unitOfMeasure.original
      .filter(x => !this.items || !this.items.map(o => o.unitOfMeasure).some(o => x.id === o.id));

    this.lookups.unitOfMeasure.items = newItems;
    this.lookups.unitOfMeasure.selected = (this.lookups.unitOfMeasure.items.length > 0)
      ? this.lookups.unitOfMeasure.items[0] : null;
  }

  public async attached(): Promise<void> {
    let [pricings, unitOfMeasures] = await Promise.all([
      this._api.pricings.getLookups(),
      this._api.unitOfMeasures.getLookups()
    ]);;

    this.lookups.pricing.items = pricings;
    this.lookups.pricing.original = pricings;
    this.lookups.unitOfMeasure.items = unitOfMeasures;
    this.lookups.unitOfMeasure.original = unitOfMeasures;
    this.resetUnitOfMeasureItems();
  }
}