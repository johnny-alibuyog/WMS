import { getValue } from "../common/models/measure";
import { isNullOrWhiteSpace } from '../common/utils/string-helpers';
import { Order, OrderInvoiceDetail, OrderPayable, OrderReportPageItem, OrderReturn, OrderReturnable, OrderStatus, SalesReportPageItem, OrderPageItem } from '../common/models/order';
import { PageRequest, PagerResponse } from '../common/models/paging';

import { AuthService } from './auth-service';
import { HttpClientFacade } from './http-client-facade';
import { Lookup } from '../common/custom_types/lookup';
import { ServiceBase } from './service-base'
import { autoinject } from 'aurelia-framework';
import * as Enumerable from 'linq';

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
    var products = order.items.map(x => x.product);

    return products.map(product => <OrderReturnable>{
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
      orderedQuantity: Enumerable.from(order.items)
        .where(x => x.product.id == product.id)
        .sum(x => getValue(x.quantity)),
      returnableQuantity: Enumerable.from(order.items)
        .where(x => x.product.id == product.id)
        .sum(x => getValue(x.quantity)),
      returnedQuantity: Enumerable.from(order.returns)
        .where(x => x.product.id == product.id)
        .sum(x => getValue(x.quantity)),
      returning: {
        returnedBy: this._auth.userAsLookup,
        returnedOn: new Date(),
        quantity: {
          value: null,
          unit: order.items.find(x => x.product.id == product.id).quantity.unit
        },
        standard: order.items.find(x => x.product.id == product.id).standard
      }
    });
  }

  public generateNewReturns(order: Order): boolean {

    this.validateReturns(order);

    let returns = Enumerable
      .from(order.returnables)
      .where(x =>
        x.returning &&
        x.returning.reason &&
        x.returning.quantity &&
        x.returning.quantity.value > 0 //&&
        //x.returning.amount > 0
      )
      .select(x => <OrderReturn>{
        product: x.product,
        reason: x.returning.reason,
        returnedBy: this._auth.userAsLookup,
        returnedOn: new Date(),
        quantity: x.returning.quantity,
        standard: x.returning.standard,
        returnedAmount: 0 //x.returning.amount 
        // NOTE: customer cannot return amount on this transcation since payment is not yet processed.
        //       If the customer has already processed payment and would like to refund payment, 
        //       you must use the return module and not order return.
      })
      .toArray();

    returns.forEach(item => {
      let instance = Enumerable.from(order.returns)
        .firstOrDefault(x => isNullOrWhiteSpace(x.id) && x.product.id == item.product.id);

      if (instance != null) {
        instance.reason = item.reason;
        instance.returnedBy = item.returnedBy;
        instance.returnedOn = item.returnedOn;
        instance.quantity = item.quantity;
        instance.standard = item.standard;
        instance.returnedAmount = item.returnedAmount;
      }
      else {
        order.returns.push(item);
      }
    });

    return returns && returns.length > 0;
  }

  private validateReturns(order: Order) {

    let invalidReturningItems = Enumerable
      .from(order.returnables)
      .where(x => x.returning.quantity.value > x.returnableQuantity)
      .toArray();

    if (invalidReturningItems.length > 0) {
      let buildErrorMessage = (arg: OrderReturnable) => {
        return `Invalid return quantity of ${arg.returning.quantity.value} because only ${arg.returnableQuantity} is returnable for product ${arg.product.name}`;
      };

      let errorMessage = Enumerable
        .from(invalidReturningItems)
        .select(buildErrorMessage)
        .toArray()
        .join(' ');

      throw new Error(errorMessage);
    }
  }
}
