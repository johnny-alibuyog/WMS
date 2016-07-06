import {autoinject} from 'aurelia-framework';
import {Customer} from '../common/models/customer'
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class CustomerService extends ServiceBase<Customer> {
  constructor(httpClient: HttpClientFacade) {
    super('customers', httpClient);
  } 
}