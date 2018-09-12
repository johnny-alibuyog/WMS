import { getValue } from "../common/models/measure";
import { Order, OrderInvoiceDetail, OrderPayable, OrderReportPageItem, OrderReturn, OrderReturnable, OrderReturning, OrderStatus, SalesReportPageItem, OrderPageItem } from '../common/models/order';
import { PageRequest, PagerResponse } from '../common/models/paging';

import { AuthService } from './auth-service';
import { HttpClientFacade } from './http-client-facade';
import { Lookup } from '../common/custom_types/lookup';
import { ServiceBase } from './service-base'
import { autoinject } from 'aurelia-framework';

@autoinject
export class OrderService extends ServiceBase<Order> {
  private _auth: AuthService;

  constructor(httpClient: HttpClientFacade, auth: AuthService) {
    super('orders', httpClient);
    this._auth = auth;
  }

  public getStatusList(): Promise<OrderStatus[]> {
    var url = this._resouce + '/statuses';
    return this._httpClient.get(url);
  }

  public getStatusLookup(): Promise<Lookup<OrderStatus>[]> {
    var url = this._resouce + '/status-lookups';
    return this._httpClient.get(url);
  }

  public getPayables(orderId: string): Promise<OrderPayable> {
    var url = this._resouce + '/' + orderId + '/payables';
    return this._httpClient.get(url);
  }

  public getReturnables(orderId: string): Promise<OrderReturnable[]> {
    var url = this._resouce + '/' + orderId + '/returnables';
    return this._httpClient.get(url);
  }

  public getInvoiceDetail(orderId: string): Promise<OrderInvoiceDetail> {
    var url = this._resouce + '/' + orderId + '/invoice-detail';
    return this._httpClient.get(url);
  }

  public getOrderReportPage(page: PageRequest): Promise<PagerResponse<OrderReportPageItem>> {
    var url = this._resouce + '/report/page';
    return this._httpClient.post(url, page);
  }

  public getSalesReportPage(page: PageRequest): Promise<PagerResponse<SalesReportPageItem>> {
    var url = this._resouce + '/sales-report/page';
    return this._httpClient.post(url, page);
  }

  public getActiveOrderPage(page: PageRequest): Promise<PagerResponse<OrderPageItem>> {
    var url = this._resouce + '/active-orders/page';
    return this._httpClient.post(url, page);
  }

  public save(order: Order): Promise<Order> {
    if (order.id) {
      var url = this._resouce + '/' + order.id;
      return this._httpClient.put(url, <Order>{
        id: order.id,
        createdBy: this._auth.userAsLookup,
        createdOn: new Date(),
        orderedBy: this._auth.userAsLookup,
        orderedOn: order.orderedOn || new Date(),
        branch: order.branch,
        customer: order.customer,
        pricing: order.pricing,
        shipper: order.shipper,
        shippingAddress: order.shippingAddress,
        taxRate: order.taxRate,
        shippingFeeAmount: order.shippingFeeAmount,
        items: order.items,
        payments: order.payments,
        returns: order.returns
      });
    }
    else {
      var url = this._resouce;
      return this._httpClient.post(url, <Order>{
        id: order.id,
        createdBy: this._auth.userAsLookup,
        createdOn: new Date(),
        orderedBy: this._auth.userAsLookup,
        orderedOn: order.orderedOn || new Date(),
        branch: order.branch,
        customer: order.customer,
        pricing: order.pricing,
        shipper: order.shipper,
        shippingAddress: order.shippingAddress,
        taxRate: order.taxRate,
        shippingFeeAmount: order.shippingFeeAmount,
        items: order.items,
        payments: order.payments,
        returns: order.returns
      });
    }
  }

  public stage(order: Order): Promise<Order> {
    var url = this._resouce + '/' + order.id + '/staged';
    return this._httpClient.post(url, <Order>{
      id: order.id,
      stagedBy: this._auth.userAsLookup,
      stagedOn: new Date()
    });
  }

  public recreate(order: Order): Promise<Order> {
    var url = this._resouce + '/' + order.id + '/recreated';
    return this._httpClient.post(url, <Order>{
      id: order.id,
      recreatedBy: this._auth.userAsLookup,
      recreatedOn: new Date()
    });
  }

  public route(order: Order): Promise<Order> {
    var url = this._resouce + '/' + order.id + '/routed';
    return this._httpClient.post(url, <Order>{
      id: order.id,
      routedBy: this._auth.userAsLookup,
      routedOn: new Date()
    });
  }

  public invoice(order: Order): Promise<Order> {
    var url = this._resouce + '/' + order.id + '/invoiced';
    return this._httpClient.post(url, <Order>{
      id: order.id,
      invoicedBy: this._auth.userAsLookup,
      invoicedOn: new Date()
    });
  }

  public ship(order: Order): Promise<Order> {
    var url = this._resouce + '/' + order.id + '/shipped';
    return this._httpClient.post(url, <Order>{
      id: order.id,
      shippedBy: this._auth.userAsLookup,
      shippedOn: new Date()
    });
  }

  public complete(order: Order): Promise<Order> {
    var url = this._resouce + '/' + order.id + '/completed';
    return this._httpClient.post(url, <Order>{
      id: order.id,
      completedBy: this._auth.userAsLookup,
      completedOn: new Date()
    });
  }

  public cancel(order: Order): Promise<Order> {
    var url = this._resouce + '/' + order.id + '/cancelled';
    return this._httpClient.post(url, <Order>{
      id: order.id,
      cancelledBy: this._auth.userAsLookup,
      cancelledOn: new Date()
    });
  }

  public computeReturnablesFrom(order: Order): OrderReturnable[] {
    var products = order.items.map(x => x.product);

    return products.map(product => {
      var returnable: OrderReturnable = {
        orderId: order.id,
        product: <Lookup<string>>{
          id: product.id,
          name: product.name
        },
        discountRate: order.items.find(x => x.product.id == product.id).discountRate,
        discountAmount: order.items.find(x => x.product.id == product.id).discountAmount,
        unitPriceAmount: order.items.find(x => x.product.id == product.id).unitPriceAmount,
        extendedPriceAmount: order.items.find(x => x.product.id == product.id).extendedPriceAmount,
        totalPriceAmount: order.items.find(x => x.product.id == product.id).totalPriceAmount,
        orderedQuantity: order.items
          .filter(item => item.product.id == product.id)
          .reduce((prevVal, item) => prevVal + getValue(item.quantity), 0),
        returnedQuantity: order.returns
          .filter(receipt => receipt.product.id == product.id)
          .reduce((prevVal, receipt) => prevVal + receipt.quantity.value, 0),
        returning: {
          returnedBy: this._auth.userAsLookup,
          returnedOn: new Date(),
          quantity: {
            value: 0,
            unit: order.items.find(x => x.product.id == product.id).quantity.unit
          },
          standard: order.items.find(x => x.product.id == product.id).standard
        }
      };

      returnable.returnableQuantity = returnable.orderedQuantity > returnable.returnedQuantity
        ? returnable.orderedQuantity - returnable.returnedQuantity : 0;

      return returnable;
    });
  }

  public generateNewReturns(order: Order): boolean {
    let returns = order.returnables
      .filter(x =>
        x.returning &&
        x.returning.reason &&
        x.returning.quantity &&
        x.returning.quantity.value > 0 &&
        x.returning.amount > 0
      )
      .map(x => <OrderReturn>{
        product: x.product,
        reason: x.returning.reason,
        returnedBy: this._auth.userAsLookup,
        returnedOn: new Date(),
        quantity: x.returning.quantity,
        standard: x.returning.standard,
        returnedAmount: x.returning.amount
      });

    let withReturns = returns && returns.length > 0;
    if (withReturns) {
      order.returns.push(...returns);
    }
    return withReturns;
  }
}
