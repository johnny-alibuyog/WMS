import { Router, RouteConfig, NavigationInstruction } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { Return, ReturnsByProductPageItem } from '../common/models/return';
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

  constructor(
    private readonly _api: ServiceApi,
    private readonly _notification: NotificationService
  ) {
    this.filter["product"] = null;
    this.filter.onFilter = () => this.getPage();
    this.sorter["product"] = SortDirection.Ascending;
    this.sorter["quantityValue"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): Promise<void> {
    this.products = await this._api.products.getLookups()
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
    //this._router.navigateToRoute('return-create', <Return>{ id: item.id });
  }

  public delete(item: ReturnsByProductPageItem) {
  }
}