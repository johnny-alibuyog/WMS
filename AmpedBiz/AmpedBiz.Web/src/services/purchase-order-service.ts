import { autoinject } from 'aurelia-framework';
import { Lookup } from '../common/custom_types/lookup';
import { PageRequest } from '../common/models/paging';
import { PurchaseOrder } from '../common/models/purchase-order';
import { PurchaseOrderStatus } from '../common/models/purchase-order';
import { PurchaseOrderPayment } from '../common/models/purchase-order';
import { PurchaseOrderReceipt } from '../common/models/purchase-order';
import { PurchaseOrderReceivable } from '../common/models/purchase-order';
import { PurchaseOrderReceiving } from '../common/models/purchase-order';
import { PurchaseOrderPayable } from '../common/models/purchase-order';
import { Voucher } from '../common/models/purchase-order';
import { ServiceBase } from './service-base'
import { AuthService } from './auth-service';
import { HttpClientFacade } from './http-client-facade';

@autoinject
export class PurchaseOrderService extends ServiceBase<PurchaseOrder> {
  private _auth: AuthService;

  constructor(httpClient: HttpClientFacade, auth: AuthService) {
    super('purchase-orders', httpClient);
    this._auth = auth;
  }

  public getStatusList(): Promise<PurchaseOrderStatus[]> {
    var url = this._resouce + '/statuses';
    return this._httpClient.get(url)
      .then(response => <PurchaseOrderStatus[]>response);
  }

  public getStatusLookup(): Promise<Lookup<PurchaseOrderStatus>[]> {
    var url = this._resouce + '/status-lookups';
    return this._httpClient.get(url)
      .then(response => <Lookup<PurchaseOrderStatus>[]>response);
  }

  public getNewPage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/new/page';
    return this._httpClient.post(url, page);
  }

  public getActivePage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/active/page';
    return this._httpClient.post(url, page);
  }

  public getApprovedPage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/approved/page';
    return this._httpClient.post(url, page);
  }

  public getCompletedPage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/completed/page';
    return this._httpClient.post(url, page);
  }

  public getReceivables(id: string): Promise<PurchaseOrderReceivable[]> {
    var url = this._resouce + '/' + id + '/receivables';
    return this._httpClient.get(url)
      .then(response => <PurchaseOrderReceivable[]>response);
  }

  public getVoucher(purchaseOrderId: string): Promise<Voucher> {
    var url = this._resouce + '/' + purchaseOrderId + '/voucher';
    return this._httpClient.get(url)
      .then(response => <Voucher>response);
  }

  public getPayables(orderId: string): Promise<PurchaseOrderPayable> {
    var url = this._resouce + '/' + orderId + '/payables';
    return this._httpClient.get(url)
      .then(response => <PurchaseOrderPayable>response);
  }

  public save(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var request = (purchaseOrder.id)
      ? this._httpClient.put(this._resouce + '/' + purchaseOrder.id, purchaseOrder)
      : this._httpClient.post(this._resouce, purchaseOrder);

    return request.then(data => {
      data.receivables = this.computeReceivablesFrom(data);
      return data;
    });

    /*
    if (purchaseOrder.id) {
      var url = this._resouce + '/' + purchaseOrder.id;
      return this._httpClient
        .put(url, <PurchaseOrder>{
          id: purchaseOrder.id,
          createdBy: this._auth.userAsLookup,
          createdOn: purchaseOrder.createdOn,
          expectedOn: purchaseOrder.expectedOn,
          supplier: purchaseOrder.supplier,
          shippingFeeAmount: purchaseOrder.shippingFeeAmount,
          taxAmount: purchaseOrder.taxAmount,
          items: purchaseOrder.items,
          payments: purchaseOrder.payments,
          receipts: purchaseOrder.receipts,
        })
        .then(data => {
          data.receivables = this.computeReceivablesFrom(data);
          return data;
        });
    }
    else {
      var url = this._resouce;
      return this._httpClient
        .post(url, <PurchaseOrder>{
          id: purchaseOrder.id,
          createdBy: this._auth.userAsLookup,
          createdOn: purchaseOrder.createdOn,
          expectedOn: purchaseOrder.expectedOn,
          supplier: purchaseOrder.supplier,
          shippingFeeAmount: purchaseOrder.shippingFeeAmount,
          taxAmount: purchaseOrder.taxAmount,
          items: purchaseOrder.items,
          payments: purchaseOrder.payments,
          receipts: purchaseOrder.receipts,
        })
        .then(data => {
          data.receivables = this.computeReceivablesFrom(data);
          return data;
        });
    }
    */
  }

  public submit(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/' + purchaseOrder.id + '/submitted';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: purchaseOrder.id,
      submittedBy: this._auth.userAsLookup,
      submittedOn: new Date(),
    });
  }

  public approve(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/' + purchaseOrder.id + '/approved';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: purchaseOrder.id,
      approvedBy: this._auth.userAsLookup,
      approvedOn: new Date()
    });
  }

  public reject(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/' + purchaseOrder.id + '/new';
    return this._httpClient.patch(url, <PurchaseOrder>{
      id: purchaseOrder.id,
      createdBy: this._auth.userAsLookup,
      createdOn: purchaseOrder.createdOn,
    });
  }

  public complete(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/' + purchaseOrder.id + '/completed';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: purchaseOrder.id,
      completedBy: this._auth.userAsLookup,
      completedOn: new Date()
    });
  }

  public cancel(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/' + purchaseOrder.id + '/cancelled';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: purchaseOrder.id,
      cancelledBy: this._auth.userAsLookup,
      cancelledOn: new Date(),
      cancellationReason: 'Cancellation Reason'
    });
  }

  public computeReceivablesFrom(purchaseOrder: PurchaseOrder): PurchaseOrderReceivable[] {
    var itemProducts = ((purchaseOrder.items && purchaseOrder.items.map(x => x.product)) || []);
    var receiptProducts = ((purchaseOrder.receipts && purchaseOrder.receipts.map(x => x.product)) || []);
    var allProducts: Lookup<string>[] = [];

    for (var i = 0; i < itemProducts.length; i++) {
      if (!allProducts.find(x => x.id === itemProducts[i].id)) {
        allProducts.push(itemProducts[i]);
      }
    }

    for (var i = 0; i < receiptProducts.length; i++) {
      if (!allProducts.find(x => x.id === receiptProducts[i].id)) {
        allProducts.push(receiptProducts[i]);
      }
    }

    return allProducts.map(product => {
      var receivable = <PurchaseOrderReceivable>{
        purchaseOrderId: purchaseOrder.id,
        product: <Lookup<string>>{
          id: product.id,
          name: product.name
        },
        orderedQuantity: purchaseOrder.items
          .filter(item => item.product.id == product.id)
          .reduce((prevVal, item) => prevVal + item.quantityValue, 0),
        receivedQuantity: purchaseOrder.receipts
          .filter(receipt => receipt.product.id == product.id)
          .reduce((prevVal, receipt) => prevVal + receipt.quantityValue, 0),
        receiving: <PurchaseOrderReceiving>{
          receivedBy: this._auth.userAsLookup,
          receivedOn: new Date(),
          quantity: 0
        }
      };

      receivable.receivableQuantity = receivable.orderedQuantity > receivable.receivedQuantity
        ? receivable.orderedQuantity - receivable.receivedQuantity : 0;

      return receivable;
    });
  }

  public generateNewReceiptsFrom(purchaseOrder: PurchaseOrder): PurchaseOrderReceipt[] {
    return purchaseOrder.receivables
      .filter(x =>
        x.receiving &&
        x.receiving.quantity > 0
      )
      .map(x => <PurchaseOrderReceipt>{
        purchaseOrderId: x.purchaseOrderId,
        product: x.product,
        batchNumber: x.receiving.batchNumber,
        receivedBy: x.receiving.receivedBy,
        receivedOn: x.receiving.receivedOn,
        expiresOn: x.receiving.expiresOn,
        quantityValue: x.receiving.quantity
      });
  }
}