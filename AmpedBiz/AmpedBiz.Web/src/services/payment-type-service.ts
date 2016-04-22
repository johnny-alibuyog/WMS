import {autoinject} from 'aurelia-framework';
import {PaymentType} from './common/models/payment-type'
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class PaymentTypeService extends ServiceBase<PaymentType> {
  constructor(httpClient: HttpClientFacade) {
    super('payment-types', httpClient);
  } 
}
