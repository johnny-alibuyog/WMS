import { autoinject } from 'aurelia-framework';
import { CustomerReport, CustomerReportModel, CustomerReportModelItem } from './customer-report';
import { CustomerReportPageItem } from '../../common/models/customer';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../../common/models/paging';

@autoinject
export class CustomerReportPage {

  public header: string = ' Customer Report';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<CustomerReportPageItem> = new Pager<CustomerReportPageItem>();

  constructor(
    private readonly _api: ServiceApi,
    private readonly _report: CustomerReport,
    private readonly _notification: NotificationService
  ) {
    this.filter.onFilter = () => this.getPage();
    this.sorter["customerName"] = SortDirection.Ascending;
    this.sorter["creditLimitAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    await this.getPage();
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.customers.getCustomerReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });
      let response = <PagerResponse<CustomerReportPageItem>>data;
      let reportModel = <CustomerReportModel>{
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
      await this._report.show(reportModel)
    }
    catch (error) {
      this._notification.error("Error encountered during report generation!");
    }
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.customers.getCustomerReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<CustomerReportPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}
