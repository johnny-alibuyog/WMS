import { ReturnReason } from './../common/models/return-reason';
import { RouteConfig, NavigationInstruction } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { ReturnsByReasonPageItem } from '../common/models/return';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { Lookup } from '../common/custom_types/lookup';

@autoinject
export class ReturnsByReasonDetailsPage {

  public header: string = '';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ReturnsByReasonPageItem> = new Pager<ReturnsByReasonPageItem>();
  public reason: ReturnReason;
  public branches: Lookup<string>[] = [];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _notification: NotificationService,
  ) {
    this.filter["branch"] = null;
    this.filter["reason"] = null;
    this.filter["includeOrderReturns"] = false;
    this.filter.onFilter = () => this.getPage();
    this.sorter["branchName"] = SortDirection.Ascending;
    this.sorter["reasonName"] = SortDirection.Ascending;
    this.sorter["returnedAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): Promise<void> {
    [this.reason, this.branches] = await Promise.all([
      this._api.returnReasons.get(params.reasonId),
      this._api.branches.getLookups()
    ]);
    this.header = ` Returns Because of ${this.reason.name}`
    this.filter["reason"] = params.reasonId;
    this.filter["branch"] = params.branchId;
    this.filter["includeOrderReturns"] = params.includeOrderReturns == "true";
    await this.getPage();
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.returns.getReturnsByReasonDetailsPage({
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
}
