import {autoinject} from 'aurelia-framework';
import {Lookup} from '../common/custom_types/lookup';
import {PageRequest} from '../common/models/paging';
import {PurchaseOrder, PurchaseOrderStatus, PurchaseOrderPayment} from '../common/models/purchase-order';
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

  createNew(entity: PurchaseOrder) {
    var url = this._resouce + '/new';
    return this._httpClient.post(url, <PurchaseOrder>{
      userId: this._auth.user.id,
      supplierId: entity.supplierId,
      expectedOn: entity.expectedOn,
      items: entity.items
    });
  }

  updateNew(entity: PurchaseOrder) {
    var url = this._resouce + '/' + entity.id + '/new';
    return this._httpClient.patch(url, <PurchaseOrder>{
      id: entity.id,
      userId: this._auth.user.id,
      supplierId: entity.supplierId,
      expectedOn: entity.expectedOn,
      items: entity.items
    });
  }

  submit(entity: PurchaseOrder) {
    var url = this._resouce + '/' + entity.id + '/submitted';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: entity.id,
      userId: this._auth.user.id,
    });
  }

  approve(entity: PurchaseOrder) {
    var url = this._resouce + '/' + entity.id + '/approved';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: entity.id,
      userId: this._auth.user.id,
    });
  }

  reject(entity: PurchaseOrder) {
    var url = this._resouce + '/' + entity.id + '/new';
    return this._httpClient.patch(url, <PurchaseOrder>{
      id: entity.id,
      userId: this._auth.user.id,
    });
  }

  pay(entity: PurchaseOrderPayment) {
    var url = this._resouce + '/' + entity.purchaseOrderId + '/paid';
    return this._httpClient.post(url, entity);
  }

  receive(entity: PurchaseOrder) {
    var url = this._resouce + '/' + entity.id + '/received';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: entity.id,
      userId: this._auth.user.id,
      /* todo: add receipts */
    });
  }

  complete(entity: PurchaseOrder) {
    var url = this._resouce + '/' + entity.id + '/completed';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: entity.id,
      userId: this._auth.user.id,
    });
  }

  cancel(entity: PurchaseOrder) {
    var url = this._resouce + '/' + entity.id + '/cancelled';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: entity.id,
      userId: this._auth.user.id,
      cancellationReason: entity.cancellationReason
    });
  }
}
