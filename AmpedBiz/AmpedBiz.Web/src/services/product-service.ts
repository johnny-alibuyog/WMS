import {autoinject} from 'aurelia-framework';
import {Product} from './common/models/product';
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class ProductService extends ServiceBase<Product> {
  constructor(httpClient: HttpClientFacade) {
    super('products', httpClient);
  } 
}
