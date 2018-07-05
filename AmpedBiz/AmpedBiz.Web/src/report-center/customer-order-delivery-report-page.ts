import { autoinject } from 'aurelia-framework';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { CustomerOrderDeliveryReport, CustomerOrderDeliveryReportModel, CustomerOrderDeliveryReportItemModel } from './customer-order-delivery-report';
import { CustomerOrderDeliveryReportPageItem } from '../common/models/customer';

@autoinject
export class CustomerSalesReportPage {
  private _api: ServiceApi;
  private _report: CustomerOrderDeliveryReport;
  private _notification: NotificationService;

  public header: string = ' Customer Order Delivery';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<CustomerOrderDeliveryReportPageItem>;

  public customers: Lookup<string>[];
  public branches: Lookup<string>[];

  constructor(api: ServiceApi, report: CustomerOrderDeliveryReport, notification: NotificationService) {
    this._api = api;
    this._report = report;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["branchId"] = null;
    this.filter["fromDate"] = null;
    this.filter["toDate"] = null;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["shippedOn"] = SortDirection.Ascending;
    this.sorter["invoiceNumber"] = SortDirection.None;
    this.sorter["branchName"] = SortDirection.None;
    this.sorter["customerName"] = SortDirection.None;
    this.sorter["pricingName"] = SortDirection.None;
    this.sorter["discountAmount"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter["subTotalAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<CustomerOrderDeliveryReportPageItem>();
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
      let data = await this._api.customers.getCustomerOrderDeliveryReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });

      let header = {
        branch: this.branches.find(x => x.id == this.filter["branchId"]),
        fromDate: this.filter["fromDate"],
        toDate: this.filter["toDate"],
      };

      let reportModel = <CustomerOrderDeliveryReportModel>{
        branchName: header.branch && header.branch.name || "All Branches",
        fromDate: header.fromDate,
        toDate: header.toDate,
        items: data.items.map(x => <CustomerOrderDeliveryReportItemModel>{
          shippedOn: x.shippedOn,
          invoiceNumber: x.invoiceNumber,
          branchName: x.branchName,
          customerName: x.customerName,
          pricingName: x.pricingName,
          discountAmount: x.discountAmount,
          totalAmount: x.totalAmount,
          subTotalAmount: x.subTotalAmount
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
      let response = await this._api.customers.getCustomerOrderDeliveryReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });

      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}