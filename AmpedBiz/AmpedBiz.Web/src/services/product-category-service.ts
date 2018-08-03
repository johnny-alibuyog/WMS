import { autoinject } from 'aurelia-framework';
import { ProductCategory } from '../common/models/product-category'
import { ServiceBase } from './service-base'
import { HttpClientFacade } from './http-client-facade';
import { Lookup } from '.././common/custom_types/lookup';

@autoinject
export class ProductCategoryService extends ServiceBase<ProductCategory> {
  constructor(httpClient: HttpClientFacade) {
    super('product-categories', httpClient);
  }

  public getLookups(): Promise<Lookup<string>[]> {
    var url = "product-category-lookups";
    return this._httpClient.get(url);
  }
}