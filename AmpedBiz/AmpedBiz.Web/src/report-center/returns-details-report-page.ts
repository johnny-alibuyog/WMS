import { RouteConfig, NavigationInstruction, Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { ReturnsDetailsReportPageItem } from '../common/models/return';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { ReturnByCustomerReportItem, ReturnByCustomerReportModel, ReturnByCustomerReport } from './returns-details-report';

@autoinject
export class ReturnsDetailsReportPage {

  public header: string = ' Returns Details';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ReturnsDetailsReportPageItem> = new Pager<ReturnsDetailsReportPageItem>();
  public branches: Lookup<string>[];
  public products: Lookup<string>[];
  public customers: Lookup<string>[];

  constructor(
    private readonly _router: Router,
    private readonly _api: ServiceApi,
    private readonly _notification: NotificationService,
    private readonly _report: ReturnByCustomerReport
  ) {
    this.filter["branch"] = null;
    this.filter["product"] = null;
    this.filter["customer"] = null;
    this.filter["fromDate"] = null;
    this.filter["toDate"] = null;
    this.filter["includeOrderReturns"] = true;
    this.filter.onFilter = () => this.getPage();
    this.sorter["branchName"] = SortDirection.Ascending;
    this.sorter["customerName"] = SortDirection.Ascending;
    this.sorter["productName"] = SortDirection.Ascending;
    this.sorter["returnedOn"] = SortDirection.Ascending;
    this.sorter["returnedByName"] = SortDirection.Ascending;
    this.sorter["returnedAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): Promise<void> {
    [this.branches, this.products, this.customers] = await Promise.all([
      this._api.branches.getLookups(),
      this._api.products.getLookups(),
      this._api.customers.getLookups()
    ]);

    await this.getPage();
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.returns.getReturnsDetailsReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<ReturnsDetailsReportPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    };
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.returns.getReturnsDetailsReportPage({
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
        product: this.products.find(x => x.id == this.filter["product"]),
        fromDate: this.filter["fromDate"],
        toDate: this.filter["toDate"],
      };

      let reportModel = <ReturnByCustomerReportModel>{
        branchName: header.branch && header.branch.name || "All Branches",
        customerName: header.customer && header.customer.name || "All Customers",
        productName: header.product && header.product.name || "All Products",
        fromDate: header.fromDate,
        toDate: header.toDate,
        items: data.items.map(x => <ReturnByCustomerReportItem>{
          branchName: x.branchName,
          customerName: x.customerName,
          productName: x.productName,
          quantiyValue: x.quantityValue,
          quantiyUnitId: x.quantityUnitId,
          reasonName: x.reasonName,
          returnedByName: x.returnedByName,
          returnedOn: x.returnedOn,
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
