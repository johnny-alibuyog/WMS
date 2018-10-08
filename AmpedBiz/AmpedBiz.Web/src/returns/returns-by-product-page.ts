import { ReturnsByProductReport, ReturnsByProductReportModel, ReturnsByProductReportItem } from './returns-by-product-report';
import { Router, RouteConfig, NavigationInstruction } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { ReturnsByProductPageItem } from '../common/models/return';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class ReturnsByProductPage {

  public header: string = ' Returns By Product';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ReturnsByProductPageItem> = new Pager<ReturnsByProductPageItem>();
  public products: Lookup<string>[] = [];
  public branches: Lookup<string>[] = [];

  constructor(
    private readonly _router: Router,
    private readonly _api: ServiceApi,
    private readonly _notification: NotificationService,
    private readonly _report: ReturnsByProductReport
  ) {
    this.filter["branch"] = null;
    this.filter["product"] = null;
    this.filter["includeOrderReturns"] = true;
    this.filter.onFilter = () => this.getPage();
    this.sorter["branchName"] = SortDirection.None;
    this.sorter["productName"] = SortDirection.Ascending;
    this.sorter["quantityValue"] = SortDirection.None;
    this.sorter["returnedAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): Promise<void> {
    [this.branches, this.products] = await Promise.all([
      this._api.branches.getLookups(),
      this._api.products.getLookups(),
    ]);
    await this.getPage();
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.returns.getReturnsByProductPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<ReturnsByProductPageItem>>data;
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

  public show(item: ReturnsByProductPageItem): void {
    let params = { 
      productId: item.id,
      branchId: this.filter["branch"],
      includeOrderReturns: this.filter["includeOrderReturns"]
   };

    this._router.navigateToRoute("returns-by-product-details-page", params);
  }

  public delete(item: ReturnsByProductPageItem) {
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.returns.getReturnsByProductPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });

      let header = {
        branch: this.branches.find(x => x.id == this.filter["branch"]),
        product: this.products.find(x => x.id == this.filter["product"]),
      };

      let reportModel = <ReturnsByProductReportModel>{
        branchName: header.branch && header.branch.name || "All Branches",
        productName: header.product && header.product.name || "All Products",
        items: data.items.map(x => <ReturnsByProductReportItem>{
          branchName: x.branchName,
          productCode: x.productCode,
          productName: x.productName,
          quantityValue: x.quantityValue,
          quantityUnit: x.quantityUnit,
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
