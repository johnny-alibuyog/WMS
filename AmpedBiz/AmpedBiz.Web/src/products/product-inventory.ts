import { Filter, Pager, PagerRequest, PagerResponse, SortDirection, Sorter } from '../common/models/paging';
import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'

import { Inventory } from '../common/models/inventory';

@autoinject
@customElement("product-inventory")
export class ProductInventory {

  @bindable()
  public productId: string = '';

  @bindable({ defaultBindingMode: bindingMode.twoWay })
  public inventory: Inventory;
}