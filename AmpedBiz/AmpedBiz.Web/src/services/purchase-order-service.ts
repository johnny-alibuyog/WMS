import {autoinject} from 'aurelia-framework';
import {KeyValuePair} from './common/custom_types/key-value-pair';
import {PageRequest} from '.././common/models/paging';
import {PurchaseOrder, PurchaseOrderStatus} from '.././common/models/purchase-order';
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class PurchaseOrderService extends ServiceBase<PurchaseOrder> {
  constructor(httpClient: HttpClientFacade) {
    super('purchase-orders', httpClient);
  } 

  getStatusList(): Promise<PurchaseOrderStatus[]> {
    var url = this._resouce + '/statuses';
    return this._httpClient.get(url)
      .then(response => <PurchaseOrderStatus[]>response);
  }

  getStatusLookup(): Promise<KeyValuePair<PurchaseOrderStatus, string>[]> {
    var url = this._resouce + '/statuses/lookup';
    return this._httpClient.get(url)
      .then(response => <KeyValuePair<PurchaseOrderStatus, string>[]>response);
  }

  getActivePage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/active/page';
    return this._httpClient.post(url, page);
  }

  getApprovedPage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/approved/page';
    return this._httpClient.post(url, page);
  }

  getCompletedPage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/completed/page';
    return this._httpClient.post(url, page);
  }
}
