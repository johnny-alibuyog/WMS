import { Router, RouteConfig, NavigationInstruction, activationStrategy } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { ReturnCreate } from './return-create';
import { Return, ReturnPageItem } from '../common/models/return';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class ReturnPage {

  public header: string = ' Returns';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ReturnPageItem> = new Pager<ReturnPageItem>();
  public branches: Lookup<string>[];
  public customers: Lookup<string>[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService
  ) {
    this.filter["branch"] = null;
    this.filter["customer"] = null;
    this.filter.onFilter = () => this.getPage();
    this.sorter["branch"] = SortDirection.None;
    this.sorter["customer"] = SortDirection.Ascending;
    this.sorter["returnedBy"] = SortDirection.None;
    this.sorter["returnedOn"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): Promise<void> {
    [this.branches, this.customers] = await Promise.all([
      this._api.branches.getLookups(),
      this._api.customers.getLookups(),
    ]);
    await this.getPage();
  }

  public determineActivationStrategy(): string {
    return activationStrategy.invokeLifecycle;
  }

  public async getPage(): Promise<void> {
    try {
      let data = await this._api.returns.getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      var response = <PagerResponse<ReturnPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }

  public create(): void {
    this._router.navigateToRoute('return-create');
  }

  public show(item: ReturnPageItem): void {
    this._router.navigateToRoute('return-create', <Return>{ id: item.id });
  }

  public delete(item: ReturnPageItem) {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}