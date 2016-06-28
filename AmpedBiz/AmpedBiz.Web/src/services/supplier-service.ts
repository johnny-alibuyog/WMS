import {autoinject} from 'aurelia-framework';
import {Supplier} from './common/models/supplier'
import {ProductInventory} from './common/models/product';
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class SupplierService extends ServiceBase<Supplier> {
  constructor(httpClient: HttpClientFacade) {
    super('suppliers', httpClient);
  }

  getProductInventories(supplierId: string): Promise<ProductInventory[]> {
    var url = this._resouce + '/' + supplierId + '/product-inventories';
    return this._httpClient.get(url)
      .then(response => <ProductInventory[]>response);
  }
}
