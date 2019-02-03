import { autoinject } from "aurelia-framework";
import { PointOfSale, PointOfSaleStatus } from "../common/models/point-of-sale";
import { HttpClientFacade } from "./http-client-facade";
import { ServiceBase } from "./service-base";
import { Lookup } from "common/custom_types/lookup";


@autoinject
export class PointOfSaleService extends ServiceBase<PointOfSale> {
  constructor(httpClient: HttpClientFacade) {
    super('point-of-sales', httpClient);
  }
  
  public getStatusList(): Promise<PointOfSaleStatus[]> {
    var url = this._resouce + '/statuses';
    return this._httpClient.get(url);
  }

  public getStatusLookup(): Promise<Lookup<PointOfSaleStatus>[]> {
    var url = this._resouce + '/status-lookups';
    return this._httpClient.get(url);
  }

}
