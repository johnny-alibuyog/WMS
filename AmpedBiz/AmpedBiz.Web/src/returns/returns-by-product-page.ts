import { Router, RouteConfig, NavigationInstruction } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { Return, ReturnsByProductPageItem } from '../common/models/return';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class ReturnsByProductPage {
  private _api: ServiceApi;
  private _router: Router;
  private _notification: NotificationService;

  public header: string = ' Returns By Product';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<ReturnsByProductPageItem>;
  public products: Lookup<string>[] = [];

  constructor(api: ServiceApi, router: Router, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["product"] = null;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["product"] = SortDirection.Ascending;
    this.sorter["quantityValue"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<ReturnsByProductPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  public activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): any {

    let requests: [Promise<Lookup<string>[]>] = [
      this._api.products.getLookups(),
    ];

    Promise.all(requests).then((responses: [Lookup<string>[], Lookup<string>[]]) => {
      this.products = responses[0];

      this.getPage();
    });
  }

  private getPage(): void {
    this._api.returns
      .getReturnsByProductPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<ReturnsByProductPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
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