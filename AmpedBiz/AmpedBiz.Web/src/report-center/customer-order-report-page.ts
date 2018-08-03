import { autoinject } from 'aurelia-framework';
import { CustomerOrderReport, CustomerOrderReportModel, CustomerOrderReportItemModel } from './customer-order-report';
import { OrderStatus, OrderReportPageItem } from '../common/models/order';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class CustomerOrderReportPage {

  public header: string = ' Customer Order';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<OrderReportPageItem> = new Pager<OrderReportPageItem>();
  public pricings: Lookup<string>[];
  public customers: Lookup<string>[];
  public statuses: Lookup<OrderStatus>[];
  public branches: Lookup<string>[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _report: CustomerOrderReport,
    private readonly _notification: NotificationService
  ) {
    this.filter["customerId"] = null;
    this.filter["branchId"] = null;
    this.filter["pricingId"] = null;
    this.filter["fromDate"] = null;
    this.filter["toDate"] = null;
    this.filter["status"] = null;
    this.filter.onFilter = () => this.getPage();
    this.sorter["branchName"] = SortDirection.None;
    this.sorter["customerName"] = SortDirection.None;
    this.sorter["pricingName"] = SortDirection.None;
    this.sorter["status"] = SortDirection.None;
    this.sorter["orderedOn"] = SortDirection.Ascending;
    this.sorter["orderedByName"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    [
      this.pricings,
      this.customers,
      this.statuses,
      this.branches
    ] = await Promise.all([
      this._api.pricings.getLookups(),
      this._api.customers.getLookups(),
      this._api.orders.getStatusLookup(),
      this._api.branches.getLookups()
    ]);

    await this.getPage();
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.orders.getOrderReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });

      let header = {
        customer: this.customers.find(x => x.id == this.filter["customerId"]),
        branch: this.branches.find(x => x.id == this.filter["branchId"]),
        pricing: this.pricings.find(x => x.id == this.filter["pricingId"]),
        status: this.statuses.find(x => x.id == this.filter["status"]),
        fromDate: this.filter["fromDate"],
        toDate: this.filter["toDate"],
      };

      let reportModel = <CustomerOrderReportModel>{
        branchName: header.branch && header.branch.name || "All Branches",
        customerName: header.customer && header.customer.name || "All Customers",
        pricingName: header.pricing && header.pricing.name || "All Pricings",
        status: header.status && header.status.name || "All Status",
        fromDate: header.fromDate,
        toDate: header.toDate,
        items: data.items.map(x => <CustomerOrderReportItemModel>{
          id: x.id,
          branchName: x.branchName,
          customerName: x.customerName,
          invoiceNumber: x.invoiceNumber,
          pricingName: x.pricingName,
          orderedOn: x.orderedOn,
          orderedByName: x.orderedByName,
          status: x.status,
          totalAmount: x.totalAmount,
          paidAmount: x.paidAmount,
          balanceAmount: x.balanceAmount,
        })
      };

      await this._report.show(reportModel)
    }
    catch (error) {
      this._notification.error("Error encountered during report generation!");
    }
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.orders.getOrderReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<OrderReportPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}