import { RouteConfig, NavigationInstruction, Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { ReturnsByCustomerPageItem } from '../common/models/return';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { ReturnByCustomerReportItem, ReturnByCustomerReportModel, ReturnByCustomerReport } from './returns-by-customer-report';

@autoinject
export class ReturnsByCustomerPage {

  public header: string = ' Returns By Customer';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ReturnsByCustomerPageItem> = new Pager<ReturnsByCustomerPageItem>();
  public branches: Lookup<string>[];
  public customers: Lookup<string>[];

  constructor(
    private readonly _router: Router,
    private readonly _api: ServiceApi,
    private readonly _notification: NotificationService,
    private readonly _report: ReturnByCustomerReport
  ) {
    this.filter["branch"] = null;
    this.filter["customer"] = null;
    this.filter["includeOrderReturns"] = false;
    this.filter.onFilter = () => this.getPage();
    this.sorter["branchName"] = SortDirection.Ascending;
    this.sorter["customerName"] = SortDirection.Ascending;
    this.sorter["returnedAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): Promise<void> {
    [this.branches, this.customers] = await Promise.all([
      this._api.branches.getLookups(),
      this._api.customers.getLookups()
    ]);

    await this.getPage();
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.returns.getReturnsByCustomerPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<ReturnsByCustomerPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    };
  }

  public create(): void {
    //this._router.navigateToRoute('return-create');
  }

  public show(item: ReturnsByCustomerPageItem): void {
    let params = {
      customerId: item.id,
      branchId: this.filter["branch"],
      includeOrderReturns: this.filter["includeOrderReturns"]
    };

    this._router.navigateToRoute("returns-by-customer-details-page", params);
  }

  public delete(item: ReturnsByCustomerPageItem) {
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.returns.getReturnsByCustomerPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });

      let header = {
        branch: this.branches.find(x => x.id == this.filter["branch"]),
        customer: this.customers.find(x => x.id == this.filter["customer"]),
      };

      let reportModel = <ReturnByCustomerReportModel>{
        branchName: header.branch && header.branch.name || "All Branches",
        customerName: header.customer && header.customer.name || "All Customers",
        items: data.items.map(x => <ReturnByCustomerReportItem>{
          branchName: x.branchName,
          customerName: x.customerName,
          returnedAmount: x.returnedAmount
        })
      };
      this._report.show(reportModel)
    }
    catch (error) {
      this._notification.error("Error encountered during report generation!");
    }
  }
}
