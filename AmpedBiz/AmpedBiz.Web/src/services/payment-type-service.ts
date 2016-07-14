import {autoinject} from 'aurelia-framework';
import {PaymentType} from '../common/models/payment-type'
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';
import {Lookup} from '.././common/custom_types/lookup';

@autoinject
export class PaymentTypeService extends ServiceBase<PaymentType> {
  constructor(httpClient: HttpClientFacade) {
    super('payment-types', httpClient);
  } 

  getLookups(): Promise<Lookup<string>[]>{
    var url = "payment-type-lookups";
    return this._httpClient.get(url)
      .then(data => <Lookup<string>[]>data);
  }
}
