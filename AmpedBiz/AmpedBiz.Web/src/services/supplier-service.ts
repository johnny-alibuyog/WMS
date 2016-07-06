import {autoinject} from 'aurelia-framework';
import {Supplier} from '../common/models/supplier'
import {ProductInventory} from '../common/models/product';
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';
import {Lookup} from '.././common/custom_types/lookup';

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

  getProductLookups(supplierId: string): Promise<Lookup<string>[]>  {
    var url = this._resouce + '/' + supplierId + '/product-lookups';
    return this._httpClient.get(url)
      .then(data => <Lookup<string>[]>data);
  }

  getLookups(): Promise<Lookup<string>[]>{
    var url = "supplier-lookups";
    return this._httpClient.get(url)
      .then(data => <Lookup<string>[]>data);
  }
}
