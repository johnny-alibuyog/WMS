import { PurchaseOrderItem, Voucher } from '../common/models/purchase-order';

import { AuthService } from './auth-service';
import { HttpClientFacade } from './http-client-facade';
import { Lookup } from '../common/custom_types/lookup';
import { Measure } from "../common/models/measure";
import { PageRequest } from '../common/models/paging';
import { Product } from "../common/models/product";
import { PurchaseOrder } from '../common/models/purchase-order';
import { PurchaseOrderPayable } from '../common/models/purchase-order';
import { PurchaseOrderReceipt } from '../common/models/purchase-order';
import { PurchaseOrderReceivable } from '../common/models/purchase-order';
import { PurchaseOrderReceiving } from '../common/models/purchase-order';
import { PurchaseOrderStatus } from '../common/models/purchase-order';
import { ServiceBase } from './service-base'
import { UnitOfMeasure } from "../common/models/unit-of-measure";
import { autoinject } from 'aurelia-framework';

@autoinject
export class PurchaseOrderService extends ServiceBase<PurchaseOrder> {
  private _auth: AuthService;

  constructor(httpClient: HttpClientFacade, auth: AuthService) {
    super('purchase-orders', httpClient);
    this._auth = auth;
  }

  public getStatusList(): Promise<PurchaseOrderStatus[]> {
    var url = this._resouce + '/statuses';
    return this._httpClient.get(url);
  }

  public getStatusLookup(): Promise<Lookup<PurchaseOrderStatus>[]> {
    var url = this._resouce + '/status-lookups';
    return this._httpClient.get(url);
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
    return this._httpClient.get(url);
  }

  public getVoucher(purchaseOrderId: string): Promise<Voucher> {
    var url = this._resouce + '/' + purchaseOrderId + '/voucher';
    return this._httpClient.get(url);
  }

  public getPayables(orderId: string): Promise<PurchaseOrderPayable> {
    var url = this._resouce + '/' + orderId + '/payables';
    return this._httpClient.get(url);
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

  public recreate(purchaseOrder: PurchaseOrder): Promise<PurchaseOrder> {
    var url = this._resouce + '/' + purchaseOrder.id + '/recreated';
    return this._httpClient.post(url, <PurchaseOrder>{
      id: purchaseOrder.id,
      recreatedBy: this._auth.userAsLookup,
      recreatedOn: new Date()
    });
  }

  // TODO: deprecate this method soon. recreate is the appropriate action
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
    type Key = {
      product: Product,
      unit: UnitOfMeasure
    };

    let composeKey = (args: PurchaseOrderItem | PurchaseOrderReceipt) => {
      return <Key>{
        product: args.product,
        unit: args.quantity.unit
      };
    };

    let compareKey = (left: Key, right: Key) =>{
      return left.product.id === right.product.id && left.unit.id === right.unit.id;
    }

    let itemKeys = ((purchaseOrder.items && purchaseOrder.items.map(composeKey)) || []);
    let receiptKeys = ((purchaseOrder.receipts && purchaseOrder.receipts.map(composeKey)) || []);
    let allKeys: Key[] = [];

    for (let i = 0; i < itemKeys.length; i++) {
      if (!allKeys.find(x => compareKey(x, itemKeys[i]))) {
        allKeys.push(itemKeys[i]);
      }
    }

    for (let i = 0; i < receiptKeys.length; i++) {
      if (!allKeys.find(x => compareKey(x, receiptKeys[i]))) {
        allKeys.push(receiptKeys[i]);
      }
    }

    let findUnit = (purchaseOrder: PurchaseOrder, product: Lookup<string>) => {
      let item = purchaseOrder.items.find(item => item.product.id == product.id);
      let receipt = purchaseOrder.receipts.find(receipt => receipt.product.id == product.id);
      if (!item && receipt) {
        return null;
      }

      let quantity = item.quantity || receipt.quantity;
      if (!quantity) {
        return null;
      }

      return quantity.unit;
    };

    let findStandard = (key: Key, purchaseOrder: PurchaseOrder) => {
      let item = purchaseOrder.items.find(item => compareKey(key, composeKey(item)));
      if (!item) {
        return null;
      }

      return item.standard;
    };

    return allKeys.map(key => {
      let receivable = <PurchaseOrderReceivable>{
        purchaseOrderId: purchaseOrder.id,
        product: <Lookup<string>>{
          id: key.product.id,
          name: key.product.name
        },
        orderedQuantity: purchaseOrder.items
          .filter(item => compareKey(key, composeKey(item)))
          .reduce((prevVal, item) => prevVal + item.quantity.value, 0),
        receivedQuantity: purchaseOrder.receipts
          .filter(item => compareKey(key, composeKey(item)))
          .reduce((prevVal, receipt) => prevVal + receipt.quantity.value, 0),
        receiving: <PurchaseOrderReceiving>{
          receivedBy: this._auth.userAsLookup,
          receivedOn: new Date(),
          quantity: <Measure>{
            unit: key.unit,
            value: 0
          },
          standard: findStandard(key, purchaseOrder)
        }
      };

      receivable.receivableQuantity = receivable.orderedQuantity > receivable.receivedQuantity
        ? receivable.orderedQuantity - receivable.receivedQuantity : 0;

      return receivable;
    });
  }

  public generateNewReceiptsFrom(purchaseOrder: PurchaseOrder): PurchaseOrderReceipt[] {
    if (purchaseOrder.receivables.some(x => !x.product)) {
      throw new Error("Product for newly added inventory receiving is not selected.");
    }

    let unfulfilledReveiving = purchaseOrder
      .receivables.filter(x =>
        x.orderedQuantity == 0 &&
        x.receivedQuantity == 0 &&
        x.receiving.quantity == 0
      );

    let plural = unfulfilledReveiving && unfulfilledReveiving.length > 1;
    if (unfulfilledReveiving && unfulfilledReveiving.length > 0) {
      let products = unfulfilledReveiving.map(x => x.product.name).join('; ');
      throw new Error(`Receiving quantity for newly added product${plural ? 's' : ''} (${products}) ${plural ? 'are' : 'is'} not filled.`);
    }

    return purchaseOrder.receivables
      .filter(x =>
        x.receiving &&
        x.receiving.quantity &&
        x.receiving.quantity.value > 0 &&
        x.receiving.quantity.unit != null
      )
      .map(x => <PurchaseOrderReceipt>{
        purchaseOrderId: x.purchaseOrderId,
        product: x.product,
        batchNumber: x.receiving.batchNumber,
        receivedBy: x.receiving.receivedBy,
        receivedOn: x.receiving.receivedOn,
        expiresOn: x.receiving.expiresOn,
        quantity: x.receiving.quantity,
        standard: x.receiving.standard
      });
  }
}