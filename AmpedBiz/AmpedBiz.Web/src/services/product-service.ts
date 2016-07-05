import {autoinject} from 'aurelia-framework';
import {Product, ProductInventory} from './common/models/product';
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class ProductService extends ServiceBase<Product> {
  constructor(httpClient: HttpClientFacade) {
    super('products', httpClient);
  } 

  getInventory(productId: string): Promise<ProductInventory> {
    var url = 'product-inventories/' + productId;
    return this._httpClient.get(url)
      .then(response => <ProductInventory>response);
  }
}
