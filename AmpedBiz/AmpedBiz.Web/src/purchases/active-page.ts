import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {PurchaseOrderCreate} from './purchase-order-create';
import {PurchaseOrder, PurchaseOrderPageItem, PurchaseOrderStatus} from '../common/models/purchase-order';
import {Supplier} from '../common/models/supplier';
import {ServiceApi} from '../services/service-api';
import {Lookup} from '../common/custom_types/lookup';
import {NotificationService} from '../common/controls/notification-service';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class ActivePage {
  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<PurchaseOrderPageItem>;

  public suppliers: Lookup<string>[];

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, filter: Filter, sorter: Sorter, pager: Pager<PurchaseOrderPageItem>) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;

    this.filter = filter;
    this.filter["supplier"] = null;
    this.filter.onFilter = () => this.getPage();

    this.sorter = sorter;
    this.sorter["supplier"] = SortDirection.None;
    this.sorter["createdBy"] = SortDirection.None;
    this.sorter["createdOn"] = SortDirection.None;
    this.sorter["submittedBy"] = SortDirection.None;
    this.sorter["total"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = pager;
    this.pager.onPage = () => this.getPage();

    this._api.suppliers.getLookups()
      .then(data => this.suppliers = data);
  }

  activate() {
    this.getPage();
  }

  getPage(): void {
    this._api.purchaseOrders
      .getActivePage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<PurchaseOrderPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  create() {
    this._dialog
      .open({
        viewModel: PurchaseOrderCreate,
        model: null
      })
      .then(response => {
        if (!response.wasCancelled) {
          this.getPage();
        }
      });
  }

  edit(item: PurchaseOrderPageItem) {
    this._dialog
      .open({
        viewModel: PurchaseOrderCreate,
        model: <PurchaseOrder>{ id: item.id }
      })
      .then(response => {
        if (!response.wasCancelled) {
          this.getPage();
        }
      });
  }

  delete(item: PurchaseOrderPageItem) {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}