import { Router, RouteConfig, NavigationInstruction } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { Return, ReturnsByReasonPageItem } from '../common/models/return';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class ReturnsByReasonPage {
  private _api: ServiceApi;
  private _router: Router;
  private _notification: NotificationService;

  public header: string = ' Returns By Reason';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<ReturnsByReasonPageItem>;
  public returnReasons: Lookup<string>[] = [];

  constructor(api: ServiceApi, router: Router, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["returnReason"] = null;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["returnReason"] = SortDirection.Ascending;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<ReturnsByReasonPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  public activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): any {

    let requests: [Promise<Lookup<string>[]>] = [
      this._api.returnReasons.getLookups(),
    ];

    Promise.all(requests).then((responses: [Lookup<string>[], Lookup<string>[]]) => {
      this.returnReasons = responses[0];

      this.getPage();
    });
  }

  private getPage(): void {
    this._api.returns
      .getReturnsByReasonPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<ReturnsByReasonPageItem>>data;
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

  public show(item: ReturnsByReasonPageItem): void {
    //this._router.navigateToRoute('return-create', <Return>{ id: item.id });
  }

  public delete(item: ReturnsByReasonPageItem) {
  }
}