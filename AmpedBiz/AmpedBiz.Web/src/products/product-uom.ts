import { Filter, Pager, PagerRequest, PagerResponse, SortDirection, Sorter } from '../common/models/paging';
import { ProductUnitOfMeasure, ProductUnitOfMeasurePrice } from "../common/models/product";
import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'

import { Dictionary } from '../common/custom_types/dictionary';
import { Inventory } from '../common/models/inventory';
import { Lookup } from '../common/custom_types/lookup';
import { Router } from 'aurelia-router';
import { ServiceApi } from '../services/service-api';

@autoinject
@customElement("product-uom")
export class ProductUOM {
  private _api: ServiceApi;

  @bindable()
  public productId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public items: ProductUnitOfMeasure[];

  public selected: ProductUnitOfMeasure;

  public selectedPrice: ProductUnitOfMeasurePrice;

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


  constructor(api: ServiceApi, router: Router) {
    this._api = api;
  }

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

    let item = <ProductUnitOfMeasure>{
      unitOfMeasure: this.lookups.unitOfMeasure.selected,
      prices: this.lookups.pricing.items
        .map(x => <ProductUnitOfMeasurePrice>{
          pricing: x,
          priceAmount: 0
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

  public attached() {
    var requests: [
      Promise<Lookup<string>[]>,
      Promise<Lookup<string>[]>
    ] = [
        this._api.pricings.getLookups(),
        this._api.unitOfMeasures.getLookups()
      ];

    Promise.all(requests).then(data => {
      this.lookups.pricing.items = data[0];
      this.lookups.pricing.original = data[0];
      this.lookups.unitOfMeasure.items = data[1];
      this.lookups.unitOfMeasure.original = data[1];
      this.resetUnitOfMeasureItems();
    });
  }
}