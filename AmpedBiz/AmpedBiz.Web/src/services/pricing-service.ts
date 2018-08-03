import { autoinject } from 'aurelia-framework';
import { Pricing } from '../common/models/pricing'
import { ServiceBase } from './service-base'
import { HttpClientFacade } from './http-client-facade';
import { Lookup } from '.././common/custom_types/lookup';

@autoinject
export class PricingService extends ServiceBase<Pricing> {
  constructor(httpClient: HttpClientFacade) {
    super('pricings', httpClient);
  }

  public getLookups(): Promise<Lookup<string>[]> {
    var url = "pricing-lookups";
    return this._httpClient.get(url);
  }

  
}
