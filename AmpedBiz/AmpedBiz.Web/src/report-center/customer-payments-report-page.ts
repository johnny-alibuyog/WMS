import { autoinject } from 'aurelia-framework';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { CustomerPaymentsReportPageItem } from '../common/models/customer';
import { CustomerPaymentsReport, CustomerPaymentsReportModel, CustomerPaymentsReportItemModel } from './customer-payments-report';

@autoinject
export class CustomerPaymentsReportPage {

  public header: string = ' Customer Order';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<CustomerPaymentsReportPageItem> = new Pager<CustomerPaymentsReportPageItem>();
  public customers: Lookup<string>[];
  public branches: Lookup<string>[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _report: CustomerPaymentsReport,
    private readonly _notification: NotificationService
  ) {
    this.filter["customerId"] = null;
    this.filter["branchId"] = null;
    this.filter["fromDate"] = null;
    this.filter["toDate"] = null;
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
    [this.customers, this.branches] = await Promise.all([
      this._api.customers.getLookups(),
      this._api.branches.getLookups()
    ]);

    await this.getPage();
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.customers.getCustomerPaymentsReportPage({
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

      let reportModel = <CustomerPaymentsReportModel>{
        branchName: header.branch && header.branch.name || "All Branches",
        customerName: header.customer && header.customer.name || "All Customers",
        fromDate: header.fromDate,
        toDate: header.toDate,
        items: data.items.map(x => <CustomerPaymentsReportItemModel>{
          paymentOn: x.paymentOn,
          invoiceNumber: x.invoiceNumber,
          branchName: x.branchName,
          customerName: x.customerName,
          paymentTypeName: x.paymentTypeName,
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
      let data = await this._api.customers.getCustomerPaymentsReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });

      let response = <PagerResponse<CustomerPaymentsReportPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}
