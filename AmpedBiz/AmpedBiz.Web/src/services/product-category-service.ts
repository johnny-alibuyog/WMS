import {autoinject} from 'aurelia-framework';
import {ProductCategory} from '../common/models/product-category'
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class ProductCategoryService extends ServiceBase<ProductCategory> {
  constructor(httpClient: HttpClientFacade) {
    super('product-categories', httpClient);
  } 
}
