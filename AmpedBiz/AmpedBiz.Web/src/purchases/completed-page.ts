import {autoinject} from 'aurelia-framework';
import {PurchaseOrderCreate} from './purchase-order-create';
import {PurchaseOrder, PurchaseOrderPageItem} from '../common/models/purchase-order';
import {Supplier} from '../common/models/supplier';
import {ServiceApi} from '../services/service-api';
import {Lookup} from '../common/custom_types/lookup';
import {NotificationService} from '../common/controls/notification-service';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class ApprovedPage {
  private _api: ServiceApi;
  private _notification: NotificationService;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<PurchaseOrderPageItem>;

  public suppliers: Lookup<string>[];

  constructor(api: ServiceApi, notification: NotificationService) {
    this._api = api;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["supplier"] = null;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["supplier"] = SortDirection.None;
    this.sorter["createdBy"] = SortDirection.None;
    this.sorter["createdOn"] = SortDirection.None;
    this.sorter["submittedBy"] = SortDirection.None;
    this.sorter["total"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<PurchaseOrderPageItem>();
    this.pager.onPage = () => this.getPage();

    this._api.suppliers.getLookups()
      .then(data => this.suppliers = data);
  }

  activate() {
    this.getPage();
  }

  getPage(): void {
    this._api.purchaseOrders
      .getCompletedPage({
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

  view(item: PurchaseOrderPageItem) {
    this._notification.info("View " + item.supplier);
  }
}