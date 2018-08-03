import { autoinject } from 'aurelia-framework';
import { buildQueryString } from 'aurelia-path';
import { Router } from 'aurelia-router';
import { PurchaseOrderPageItem } from '../common/models/purchase-order';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class ActivePurchaseOrderPage {

  public header: string = 'Active Purchase Orders';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<PurchaseOrderPageItem> = new Pager<PurchaseOrderPageItem>();

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService
  ) {
    this.filter.onFilter = () => this.getPage();
    this.sorter["supplier"] = SortDirection.None;
    this.sorter["status"] = SortDirection.Ascending;
    this.sorter["createdBy"] = SortDirection.None;
    this.sorter["createdOn"] = SortDirection.None;
    this.sorter["submittedBy"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public activate(): void {
    this.getPage();
  }

  public attached(): void {
    this.getPage();
  }

  public async getPage(): Promise<void> {
    try {
      let data = await this._api.purchaseOrders.getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      var response = <PagerResponse<PurchaseOrderPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }

  public edit(item: PurchaseOrderPageItem): void {
    this._router.navigate('#purchases/purchase-order-create?' + buildQueryString({ id: item.id }));
  }
}