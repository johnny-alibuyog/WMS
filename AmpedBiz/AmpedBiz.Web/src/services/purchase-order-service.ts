import {autoinject} from 'aurelia-framework';
import {Lookup} from './common/custom_types/lookup';
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

  getStatusLookup(): Promise<Lookup<PurchaseOrderStatus>[]> {
    var url = this._resouce + '/status-lookups';
    return this._httpClient.get(url)
      .then(response => <Lookup<PurchaseOrderStatus>[]>response);
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
    var url = this._resouce + '/' + entity.id + '/new';
    return this._httpClient.patch(url, <PurchaseOrder>{
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
