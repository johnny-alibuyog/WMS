import { Router, RouteConfig, NavigationInstruction } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { ReturnsByReasonPageItem } from '../common/models/return';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { ReturnsByReasonReportModel, ReturnsByReasonReportItem, ReturnsByReasonReport } from './returns-by-reason-report';

@autoinject
export class ReturnsByReasonPage {

  public header: string = ' Returns By Reason';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ReturnsByReasonPageItem> = new Pager<ReturnsByReasonPageItem>();
  public reasons: Lookup<string>[] = [];
  public branches: Lookup<string>[] = [];

  constructor(
    private readonly _router: Router,
    private readonly _api: ServiceApi,
    private readonly _notification: NotificationService,
    private readonly _report: ReturnsByReasonReport
  ) {
    this.filter["branch"] = null;
    this.filter["reason"] = null;
    this.filter["includeOrderReturns"] = true;
    this.filter.onFilter = () => this.getPage();
    this.sorter["branchName"] = SortDirection.Ascending;
    this.sorter["reasonName"] = SortDirection.Ascending;
    this.sorter["returnedAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): Promise<void> {
    [this.reasons, this.branches] = await Promise.all([
      this._api.returnReasons.getLookups(),
      this._api.branches.getLookups()
    ]);
    await this.getPage();
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.returns.getReturnsByReasonPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      var response = <PagerResponse<ReturnsByReasonPageItem>>data;
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

  public show(item: ReturnsByReasonPageItem): void {
    let params = {
      reasonId: item.id,
      branchId: this.filter["branch"],
      includeOrderReturns: this.filter["includeOrderReturns"]
    };

    this._router.navigateToRoute("returns-by-reason-details-page", params);
  }

  public delete(item: ReturnsByReasonPageItem) {
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.returns.getReturnsByReasonPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });

      let header = {
        branch: this.branches.find(x => x.id == this.filter["branch"]),
        reason: this.reasons.find(x => x.id == this.filter["reason"]),
      };

      let reportModel = <ReturnsByReasonReportModel>{
        branchName: header.branch && header.branch.name || "All Branches",
        reasonName: header.reason && header.reason.name || "All Reasons",
        items: data.items.map(x => <ReturnsByReasonReportItem>{
          branchName: x.branchName,
          reasonName: x.reasonName,
          returnedAmount: x.returnedAmount
        })
      };
      this._report.show(reportModel)
    }
    catch (error) {
      this._notification.error("Error encountered during report generation!");
    }
  }}
