import {Router, RouteConfig, NavigationInstruction, activationStrategy} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';
import {ReportName} from '../common/models/reports';
import {ServiceApi} from '../services/service-api';

@autoinject
export class ReportPage {
  private _api: ServiceApi;
  private _router: Router;

  public header: string = ' Reports';
  public reportPath: string = '';

  constructor(api: ServiceApi, router: Router) {
    this._api = api;
    this._router = router;
  }

  activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): any {
    /*
    let requests: [Promise<Lookup<string>[]>, Promise<Lookup<OrderStatus>[]>] = [
      this._api.customers.getLookups(),
      this._api.orders.getStatusLookup()
    ];

    Promise.all(requests).then((responses: [Lookup<string>[], Lookup<OrderStatus>[]]) => {
      this.customers = responses[0];
      this.statuses = responses[1];
      this.filter["status"] = routeConfig.settings.status;
      this.header = routeConfig.title;
      this.getPage();
    });*/
    this.reportPath = routeConfig.settings.reportSource;
  }

  determineActivationStrategy(): string {
    return activationStrategy.invokeLifecycle;
  }
}