import { PageRequest, PagerResponse } from '../common/models/paging';
import { Supplier, SupplierReportPageItem } from '../common/models/supplier'

import { HttpClientFacade } from './http-client-facade';
import { Lookup } from '.././common/custom_types/lookup';
import { ServiceBase } from './service-base'
import { autoinject } from 'aurelia-framework';

@autoinject
export class SupplierService extends ServiceBase<Supplier> {
  constructor(httpClient: HttpClientFacade) {
    super('suppliers', httpClient);
  }

  public getProductLookups(supplierId: string): Promise<Lookup<string>[]> {
    var url = this._resouce + '/' + supplierId + '/product-lookups';
    return this._httpClient.get(url);
  }

  public getLookups(): Promise<Lookup<string>[]> {
    var url = "supplier-lookups";
    return this._httpClient.get(url);
  }

  public getSupplierReportPage(page: PageRequest): Promise<PagerResponse<SupplierReportPageItem>> {
    var url = this._resouce + '/report/page';
    return this._httpClient.post(url, page);
  }
}
