import { Filter, Pager, PagerRequest, PagerResponse, SortDirection, Sorter } from '../common/models/paging';
import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'

import { Dictionary } from '../common/custom_types/dictionary';
import { Inventory } from '../common/models/inventory';
import { Lookup } from '../common/custom_types/lookup';
import { Router } from 'aurelia-router';
import { ServiceApi } from '../services/service-api';

@autoinject
@customElement("product-inventory")
export class ProductInventory {
  private _api: ServiceApi;


  public unitOfMeasures: Lookup<string>[];

  @bindable()
  public productId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public inventory: Inventory;


  constructor(api: ServiceApi, router: Router) {
    this._api = api;
  }

  attached() {
    var requests: [Promise<Lookup<string>[]>] = [this._api.unitOfMeasures.getLookups()];

    Promise.all(requests).then(data => {
      this.unitOfMeasures = data[0];
    });
  }
}