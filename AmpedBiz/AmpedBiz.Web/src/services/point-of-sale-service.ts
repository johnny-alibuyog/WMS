import { autoinject } from "aurelia-framework";
import { PointOfSale } from "../common/models/point-of-sale";
import { HttpClientFacade } from "./http-client-facade";
import { ServiceBase } from "./service-base";


@autoinject
export class PointOfSaleService extends ServiceBase<PointOfSale> {
  constructor(httpClient: HttpClientFacade) {
    super('point-of-sales', httpClient);
  }
}
