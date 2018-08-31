import { Router, RouteConfig, NavigationInstruction } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { ReturnsByReasonPageItem } from '../common/models/return';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class ReturnsByReasonPage {

  public header: string = ' Returns By Reason';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ReturnsByReasonPageItem> = new Pager<ReturnsByReasonPageItem>();
  public returnReasons: Lookup<string>[] = [];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _notification: NotificationService,
  ) {
    this.filter["returnReason"] = null;
    this.filter.onFilter = () => this.getPage();
    this.sorter["returnReason"] = SortDirection.Ascending;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): Promise<void> {
    this.returnReasons = await this._api.returnReasons.getLookups();
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
    //this._router.navigateToRoute('return-create', <Return>{ id: item.id });
  }

  public delete(item: ReturnsByReasonPageItem) {
  }
}