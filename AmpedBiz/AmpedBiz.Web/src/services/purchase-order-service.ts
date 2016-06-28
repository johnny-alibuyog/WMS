import {autoinject} from 'aurelia-framework';
import {KeyValuePair} from './common/custom_types/key-value-pair';
import {PageRequest} from '.././common/models/paging';
import {PurchaseOrder, PurchaseOrderStatus} from '.././common/models/purchase-order';
import {ServiceBase} from './service-base'
import {AuthService} from './auth-service';
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class PurchaseOrderService extends ServiceBase<PurchaseOrder> {
  private _auth: AuthService;

  constructor(httpClient: HttpClientFacade, auth: AuthService) {
    super('purchase-orders', httpClient);
    this._auth = auth;
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

  getNewPage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/new/page';
    return this._httpClient.post(url, page);
  }

  createNew(entity: PurchaseOrder) {
    var url = this._resouce + '/new';
    return this._httpClient.post(url, <PurchaseOrder>{
      userId: this._auth.user.id,
      supplierId: entity.supplierId,
      expectedOn: entity.expectedOn,
      purchaseOrderDetails: entity.purchaseOrderDetails
    });
  }

  updateNew(entity: PurchaseOrder) {
    var url = this._resouce + entity.id + '/new';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: entity.id,
      userId: this._auth.user.id,
      supplierId: entity.supplierId,
      expectedOn: entity.expectedOn,
      purchaseOrderDetails: entity.purchaseOrderDetails
    });
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
