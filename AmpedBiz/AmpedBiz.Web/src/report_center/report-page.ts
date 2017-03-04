import { Router, RouteConfig, NavigationInstruction, activationStrategy } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { ReportName } from '../common/models/reports';
import { ServiceApi } from '../services/service-api';

@autoinject
export class ReportPage {
  private _api: ServiceApi;
  private _router: Router;

  public reportPath: string = '';

  constructor(api: ServiceApi, router: Router) {
    this._api = api;
    this._router = router;
  }

  activate(params: any, routeConfig: RouteConfig, $navigationInstruction: NavigationInstruction): any {
    this.reportPath = routeConfig.settings.reportSource;
  }

  determineActivationStrategy(): string {
    return activationStrategy.invokeLifecycle;
  }
}