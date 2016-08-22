import {autoinject} from 'aurelia-framework';
import {Lookup} from '../common/custom_types/lookup';
import {PageRequest} from '../common/models/paging';
import {PurchaseOrder, PurchaseOrderStatus, PurchaseOrderPayment, PurchaseOrderReceivable, PurchaseOrderPayable} from '../common/models/purchase-order';
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


  getReceivables(id: string): Promise<PurchaseOrderReceivable[]> {
    var url = this._resouce + '/' + id + '/receivables';
    return this._httpClient.get(url)
      .then(response => <PurchaseOrderReceivable[]>response);
  }

  getPayables(orderId: string): Promise<PurchaseOrderPayable> {
    var url = this._resouce + '/' + orderId + '/payables';
    return this._httpClient.get(url)
      .then(response => <PurchaseOrderPayable>response);
  }

  createNew(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/new';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: purchaseOrder.id,
      createdBy: this._auth.userAsLookup,
      createdOn: purchaseOrder.createdOn,
      expectedOn: purchaseOrder.expectedOn,
      supplier: purchaseOrder.supplier,
      items: purchaseOrder.items
    });
  }

  updateNew(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/' + purchaseOrder.id + '/new';
    return this._httpClient.patch(url, <PurchaseOrder>{
      id: purchaseOrder.id,
      createdBy: this._auth.userAsLookup,
      createdOn: purchaseOrder.createdOn,
      expectedOn: purchaseOrder.expectedOn,
      supplier: purchaseOrder.supplier,
      items: purchaseOrder.items
    });
  }

  submit(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/' + purchaseOrder.id + '/submitted';
    return this._httpClient.post(url, <PurchaseOrder>{
      purchaseOrderId: purchaseOrder.id,
      submittedBy: this._auth.userAsLookup,
      submittedOn: new Date(),
    });
  }

  approve(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/' + purchaseOrder.id + '/approved';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: purchaseOrder.id,
      approvedBy: this._auth.userAsLookup,
      approvedOn: new Date()
    });
  }

  reject(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/' + purchaseOrder.id + '/new';
    return this._httpClient.patch(url, <PurchaseOrder>{
      id: purchaseOrder.id,
      createdBy: this._auth.userAsLookup,
      createdOn: purchaseOrder.createdOn,
    });
  }

  pay(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/' + purchaseOrder.id + '/paid';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: purchaseOrder.id,
      payments: purchaseOrder.payments
    });
  }

  receive(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/' + purchaseOrder.id + '/received';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: purchaseOrder.id,
      receipts: purchaseOrder.receipts
    });
  }

  complete(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/' + purchaseOrder.id + '/completed';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: purchaseOrder.id,
      completedBy: this._auth.userAsLookup,
      completedOn: new Date()
    });
  }

  cancel(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/' + purchaseOrder.id + '/cancelled';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: purchaseOrder.id,
      cancelledBy: this._auth.userAsLookup,
      cancelledOn: new Date(),
      cancellationReason: 'Cancellation Reason'
    });
  }
}
