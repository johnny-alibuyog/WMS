import { autoinject } from 'aurelia-framework';
import { CustomerReport, CustomerReportModel, CustomerReportModelItem } from './customer-report';
import { CustomerReportPageItem } from '../common/models/customer';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class CustomerReportPage {
  private _api: ServiceApi;
  private _report: CustomerReport;
  private _notification: NotificationService;

  public header: string = ' Customer Report';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<CustomerReportPageItem>;

  constructor(api: ServiceApi, report: CustomerReport, notification: NotificationService) {
    this._api = api;
    this._report = report;
    this._notification = notification;

    this.filter = new Filter();
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["customerName"] = SortDirection.Ascending;
    this.sorter["creditLimitAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<CustomerReportPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  public activate(): void {
    this.getPage();
  }

  public generateReport() {
    this._api.customers
      .getCustomerReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      })
      .then(data => {
        var response = <PagerResponse<CustomerReportPageItem>>data;
        var reportModel = <CustomerReportModel>{
          items: response.items.map(x => <CustomerReportModelItem>{
            id: x.id,
            name: x.name,
            contactPerson: x.contactPerson,
            creditLimitAmount: x.creditLimitAmount,
            contact: x.contact,
            officeAddress: x.officeAddress,
            billingAddress: x.billingAddress
          })
        };

        this._report.show(reportModel)
      })
      .catch(error => {
        this._notification.error("Error encountered during report generation!");
      });
  }

  private getPage(): void {
    this._api.customers
      .getCustomerReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<CustomerReportPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }
}