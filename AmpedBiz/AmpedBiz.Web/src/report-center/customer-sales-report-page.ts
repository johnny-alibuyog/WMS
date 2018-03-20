import { autoinject } from 'aurelia-framework';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { CustomerSalesReportPageItem } from '../common/models/customer';
import { CustomerSalesReport, CustomerSalesReportModel, CustomerSalesReportItemModel } from './customer-sales-report';

@autoinject
export class CustomerSalesReportPage {
  private _api: ServiceApi;
  private _report: CustomerSalesReport;
  private _notification: NotificationService;

  public header: string = ' Customer Order';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<CustomerSalesReportPageItem>;

  public customers: Lookup<string>[];
  public branches: Lookup<string>[];

  constructor(api: ServiceApi, report: CustomerSalesReport, notification: NotificationService) {
    this._api = api;
    this._report = report;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["customerId"] = null;
    this.filter["branchId"] = null;
    this.filter["fromDate"] = null;
    this.filter["toDate"] = null;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["completedOn"] = SortDirection.Ascending;
    this.sorter["branchName"] = SortDirection.None;
    this.sorter["customerName"] = SortDirection.None;
    this.sorter["invoice"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter["balanceAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<CustomerSalesReportPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    [this.customers, this.branches] = await Promise.all([
      this._api.customers.getLookups(),
      this._api.branches.getLookups()
    ]);

    this.getPage();
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.customers.getCustomerSalesReportPage({
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
        fromDate: this.filter["fromDate"],
        toDate: this.filter["toDate"],
      };

      let reportModel = <CustomerSalesReportModel>{
        branchName: header.branch && header.branch.name || "All Branches",
        customerName: header.customer && header.customer.name || "All Customers",
        fromDate: header.fromDate,
        toDate: header.toDate,
        items: data.items.map(x => <CustomerSalesReportItemModel>{
          completedOn: x.completedOn,
          branchName: x.branchName,
          customerName: x.customerName,
          invoiceNumber: x.invoiceNumber,
          totalAmount: x.totalAmount,
          paidAmount: x.paidAmount,
          balanceAmount: x.balanceAmount,
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
      let data = await this._api.customers.getCustomerSalesReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });

      let response = <PagerResponse<CustomerSalesReportPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}