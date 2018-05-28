import { Filter, Pager, PagerRequest, PagerResponse, SortDirection, Sorter } from '../common/models/paging';
import { autoinject, bindable, bindingMode, customElement } from 'aurelia-framework'

import { DialogService, DialogSettings } from 'aurelia-dialog';
import { Dictionary } from '../common/custom_types/dictionary';
import { ProductOrderReturnPageItem } from '../common/models/product';
import { Router } from 'aurelia-router';
import { ServiceApi } from '../services/service-api';
import { buildQueryString } from 'aurelia-path';
import { InventoryAdjustmentPageItem, inventoryEvents } from '../common/models/inventory';
import { ProductInventoryAdjustmentCreate } from './product-inventory-adjustment-create';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';
import { role } from '../common/models/role';

@autoinject
@customElement("product-inventory-adjustment-page")
export class ProductInventoryAdjustmentPage {
  private readonly _api: ServiceApi;
  private readonly _router: Router;
  private readonly _dialog: DialogService;
  private readonly _eventAggregator: EventAggregator;
  private _subscriptions: Subscription[] = [];

  public canCreateAdjustment = false;
  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<InventoryAdjustmentPageItem>;

  @bindable()
  public productId: string = '';

  @bindable()
  public inventoryId: string = '';

  constructor(api: ServiceApi, router: Router, dialog: DialogService, eventAggrigator: EventAggregator) {
    this._api = api;
    this._router = router;
    this._dialog = dialog;
    this._eventAggregator = eventAggrigator;

    this.filter = new Filter();
    this.filter["id"] = this.inventoryId;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["adjustedBy"] = SortDirection.None;
    this.sorter["adjustedOn"] = SortDirection.Descending;
    this.sorter["reason"] = SortDirection.None;
    this.sorter["type"] = SortDirection.None;
    this.sorter["quantity"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<InventoryAdjustmentPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  public attached(): void {
    this._subscriptions = [
      this._eventAggregator.subscribe(
        inventoryEvents.adjustment.create,
        response => this.createAdjustment()
      ),
    ];
  }

  public detached(): void {
    this._subscriptions.forEach(subscription => subscription.dispose());
  }

  public async inventoryIdChanged(): Promise<void> {
    if (this.inventoryId) {
      this.canCreateAdjustment = this._api.auth.isAuthorized([role.admin, role.manager]);
      this.filter["id"] = this.inventoryId;
      await this.getPage();
    }
    else {
      this.canCreateAdjustment = false;
    }
  }

  public async getPage(): Promise<void> {
    if (!this.filter["id"]) {
      return;
    }

    let response = await this._api.inventories.getAdjustmentPage({
      filter: this.filter,
      sorter: this.sorter,
      pager: <PagerRequest>this.pager
    });
    this.pager.count = response.count;
    this.pager.items = response.items;
  }

  public async createAdjustment(): Promise<void> {
    let settings = <DialogSettings>{
      viewModel: ProductInventoryAdjustmentCreate,
      model: { productId: this.productId, inventoryId: this.inventoryId }
    };
    let response = await this._dialog.open(settings).whenClosed();
    if (!response.wasCancelled) {
      await this.getPage();
      this._eventAggregator.publish(inventoryEvents.adjustment.created);
    }
  }
}