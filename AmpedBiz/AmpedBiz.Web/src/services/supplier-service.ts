import {autoinject} from 'aurelia-framework';
import {Supplier} from './common/models/supplier'
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class SupplierService extends ServiceBase<Supplier> {
  constructor(httpClient: HttpClientFacade) {
    super('suppliers', httpClient);
  } 

  
}
