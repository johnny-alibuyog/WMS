import { pricing } from './../common/models/pricing';
import { Branch } from './../common/models/branch';
import { Customer } from './../common/models/customer';
import { RouteConfig, NavigationInstruction } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { ReturnsByCustomerPageItem } from '../common/models/return';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { Lookup } from '../common/custom_types/lookup';

@autoinject
export class ReturnsByCustomerDetailsPage {

  public header: string = '';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ReturnsByCustomerPageItem> = new Pager<ReturnsByCustomerPageItem>();
  public customer: Customer;
  public branches: Lookup<string>[] = [];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _notification: NotificationService
  ) {
    this.filter["branch"] = null;
    this.filter["customer"] = null;
    this.filter["includeOrderReturns"] = false;
    this.filter.onFilter = () => this.getPage();
    this.sorter["branch"] = SortDirection.None;
    this.sorter["customer"] = SortDirection.Ascending;
    this.sorter["returnedOn"] = SortDirection.None;
    this.sorter["returnedAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): Promise<void> {
    [this.branches, this.customer] = await Promise.all([
      this._api.branches.getLookups(),
      this._api.customers.get(params.customerId),
    ]);
    this.header = ` Returns of ${this.customer.name}`;
    this.filter["branch"] = params.branchId;
    this.filter["customer"] = params.customerId;
    this.filter["includeOrderReturns"] = params.includeOrderReturns == "true";

    await this.getPage();
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.returns.getReturnsByCustomerDetailsPage({
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
}
