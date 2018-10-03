import { autoinject } from 'aurelia-framework';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { PurchaseOrderReport, PurchaseOrderReportModel, PurchaseOrderReportItemModel } from './purchase-order-report';
import { PurchaseOrderReportPageItem, PurchaseOrderStatus } from '../common/models/purchase-order';
import * as Enumerable from 'linq';

@autoinject
export class PurchaseOrderReportPage {

  public header: string = ' Customer Order';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<PurchaseOrderReportPageItem> = new Pager<PurchaseOrderReportPageItem>();
  public suppliers: Lookup<string>[];
  public branches: Lookup<string>[];
  public statuses: Lookup<PurchaseOrderStatus>[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _report: PurchaseOrderReport,
    private readonly _notification: NotificationService
  ) {
    this.filter["branchId"] = null;
    this.filter["supplierId"] = null;
    this.filter["fromDate"] = null;
    this.filter["toDate"] = null;
    this.filter["status"] = null;
    this.filter.onFilter = () => this.getPage();
    this.sorter["createdOn"] = SortDirection.Ascending;
    this.sorter["branchName"] = SortDirection.None;
    this.sorter["supplierName"] = SortDirection.None;
    this.sorter["voucherNumber"] = SortDirection.None;
    this.sorter["createdByName"] = SortDirection.None;
    this.sorter["approvedByName"] = SortDirection.None;
    this.sorter["status"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter["paidAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    [
      this.suppliers,
      this.branches,
      this.statuses
    ] = await Promise.all([
      this._api.suppliers.getLookups(),
      this._api.branches.getLookups(),
      this._api.purchaseOrders.getStatusLookup()
    ]);

    await this.getPage();
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.purchaseOrders.getReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });

      let header = {
        branch: this.branches.find(x => x.id == this.filter["branchId"]),
        supplier: this.suppliers.find(x => x.id == this.filter["supplierId"]),
        status: this.statuses.find(x => x.id == this.filter["status"]),
        fromDate: this.filter["fromDate"],
        toDate: this.filter["toDate"],
      };

      let reportModel = <PurchaseOrderReportModel>{
        branchName: header.branch && header.branch.name || "All Branches",
        supplierName: header.supplier && header.supplier.name || "All Suppliers",
        status: header.status && header.status.name || "All Status",
        fromDate: header.fromDate,
        toDate: header.toDate,
        items: Enumerable.from(data.items)
          .select(x => <PurchaseOrderReportItemModel>{
            id: x.id,
            createdOn: x.createdOn,
            branchName: x.branchName,
            supplierName: x.supplierName,
            voucherNumber: x.voucherNumber,
            createdByName: x.createdByName,
            approvedByName: x.approvedByName,
            status: x.status,
            totalAmount: x.totalAmount,
            paidAmount: x.paidAmount,
            balanceAmount: x.balanceAmount,
          })
          .toArray()
      };

      await this._report.show(reportModel)
    }
    catch (error) {
      this._notification.error("Error encountered during report generation!");
    }
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.purchaseOrders.getReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<PurchaseOrderReportPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}