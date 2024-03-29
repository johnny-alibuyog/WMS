import { autoinject } from 'aurelia-framework';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { InventoryMovementsReport, InventoryMovementsReportModel, InventoryMovementsReportItemModel } from './inventory-movements-report';
import { InventoryMovementsReportPageItem } from '../common/models/inventory';

@autoinject
export class InventoryMovementsReportPage {

  public header: string = ' Inventory Movements';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<InventoryMovementsReportPageItem> = new Pager<InventoryMovementsReportPageItem>();
  public products: Lookup<string>[];
  public branches: Lookup<string>[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _report: InventoryMovementsReport,
    private readonly _notification: NotificationService
  ) {
    this.filter["customerId"] = null;
    this.filter["branchId"] = null;
    this.filter["fromDate"] = new Date();
    this.filter["toDate"] = new Date();
    this.filter.onFilter = () => this.getPage();
    this.sorter["paymentOn"] = SortDirection.Ascending;
    this.sorter["branchName"] = SortDirection.None;
    this.sorter["customerName"] = SortDirection.None;
    this.sorter["invoice"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter["balanceAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    [this.products, this.branches] = await Promise.all([
      this._api.products.getLookups(),
      this._api.branches.getLookups()
    ]);
    await this.getPage();
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.inventories.getMovementsReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });

      let header = {
        branch: this.branches.find(x => x.id == this.filter["branchId"]),
        product: this.products.find(x => x.id == this.filter["productId"]),
        fromDate: this.filter["fromDate"],
        toDate: this.filter["toDate"],
      };

      let reportModel = <InventoryMovementsReportModel>{
        branchName: header.branch && header.branch.name || "All Branches",
        productName: header.product && header.product.name || "All Products",
        fromDate: header.fromDate,
        toDate: header.toDate,
        items: data.items.map(x => <InventoryMovementsReportItemModel>{
          date: x.date,
          branchName: x.branchName,
          productName: x.productName,
          in: x.in,
          out: x.out,
        })
      };
      this._report.show(reportModel)
    }
    catch (error) {
      this._notification.error("Error encountered during report generation!");
    }
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.inventories.getMovementsReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });

      let response = <PagerResponse<InventoryMovementsReportPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}
