import {Router, RouteConfig, NavigationInstruction, activationStrategy} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';
import {ReturnCreate} from './return-create';
import {Return, ReturnPageItem} from '../common/models/return';
import {ServiceApi} from '../services/service-api';
import {Lookup} from '../common/custom_types/lookup';
import {NotificationService} from '../common/controls/notification-service';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class ReturnPage {
  private _api: ServiceApi;
  private _router: Router;
  private _notification: NotificationService;

  public header: string = ' Returns';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<ReturnPageItem>;
  public reasons: Lookup<string>[];
  public branches: Lookup<string>[];
  public customers: Lookup<string>[];

  constructor(api: ServiceApi, router: Router, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["reason"] = null;
    this.filter["branch"] = null;
    this.filter["customer"] = null;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["branch"] = SortDirection.None;
    this.sorter["customer"] = SortDirection.Ascending;
    this.sorter["returnedBy"] = SortDirection.None;
    this.sorter["returnedOn"] = SortDirection.None;
    this.sorter["reason"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<ReturnPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): any {

    let requests: [Promise<Lookup<string>[]>, Promise<Lookup<string>[]>, Promise<Lookup<string>[]>] = [
      this._api.returnReasons.getLookups(),
      this._api.branches.getLookups(),
      this._api.customers.getLookups(),
    ];

    Promise.all(requests).then((responses: [Lookup<string>[], Lookup<string>[], Lookup<string>[]]) => {
      this.reasons = responses[0];
      this.branches = responses[1];
      this.customers = responses[2];

      this.getPage();
    });
  }

  determineActivationStrategy(): string {
    return activationStrategy.invokeLifecycle;
  }

  getPage(): void {
    this._api.returns
      .getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<ReturnPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  create(): void {
    this._router.navigateToRoute('return-create');
  }

  show(item: ReturnPageItem): void {
    this._router.navigateToRoute('return-create', <Return>{ id: item.id });
  }

  delete(item: ReturnPageItem) {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}