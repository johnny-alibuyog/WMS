import {autoinject} from 'aurelia-framework';
import {PricingScheme} from '../common/models/pricing-scheme'
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';
import {Lookup} from '.././common/custom_types/lookup';

@autoinject
export class PricingSchemeService extends ServiceBase<PricingScheme> {
  constructor(httpClient: HttpClientFacade) {
    super('pricing-schemes', httpClient);
  }

  getLookups(): Promise<Lookup<string>[]> {
    var url = "pricing-scheme-lookups";
    return this._httpClient.get(url)
      .then(data => <Lookup<string>[]>data);
  }
}
