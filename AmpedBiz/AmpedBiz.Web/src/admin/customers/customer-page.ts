import { Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { Customer, CustomerPageItem } from '../../common/models/customer';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../../common/models/paging';

@autoinject
export class CustomerPage {

  public header: string = ' Customers';
  public filter: Filter = new Filter(); 
  public sorter: Sorter = new Sorter(); 
  public pager: Pager<CustomerPageItem> = new Pager<CustomerPageItem>();

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService
  ) {
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getPage();
    this.sorter["code"] = SortDirection.None;
    this.sorter["name"] = SortDirection.Ascending;
    this.sorter["descirption"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    await this.getPage();
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.customers.getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<CustomerPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }

  public create(): void {
    this._router.navigateToRoute('customer-create');
  }

  public edit(item: CustomerPageItem): void {
    this._router.navigateToRoute('customer-create', <Customer>{ id: item.id });
  }

  public delete(): void {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}