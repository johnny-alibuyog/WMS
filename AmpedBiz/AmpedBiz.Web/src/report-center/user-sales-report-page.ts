import { UserSalesReportPageItem } from './../common/models/user';
import { autoinject } from 'aurelia-framework';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { UserSalesReport, UserSalesReportModel, UserSalesReportModelItem } from './user-sales-report';

@autoinject
export class UserSalesReportPage {

  public header: string = 'User Sales';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<UserSalesReportPageItem> = new Pager<UserSalesReportPageItem>();

  constructor(
    private readonly _api: ServiceApi,
    private readonly _report: UserSalesReport,
    private readonly _notification: NotificationService
  ) {
    this.filter["fromDate"] = new Date();
    this.filter["toDate"] = new Date();
    this.filter.onFilter = () => this.getPage();
    this.sorter["userFullname"] = SortDirection.Ascending;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    await this.getPage();
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.users.getSalesPagePage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });
      let response = <PagerResponse<UserSalesReportPageItem>>data;
      let reportModel = <UserSalesReportModel>{
        items: <UserSalesReportModelItem[]>response.items
      };
      await this._report.show(reportModel);
    }
    catch (error) {
      this._notification.error("Error encountered during report generation!");
    }
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.users.getSalesPagePage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<UserSalesReportPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}
