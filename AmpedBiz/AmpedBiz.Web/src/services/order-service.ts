import { autoinject } from 'aurelia-framework';
import { Lookup } from '../common/custom_types/lookup';
import { PageRequest, PagerResponse } from '../common/models/paging';
import { Order, OrderPayable, OrderReturnable, OrderStatus, OrderInvoiceDetail, OrderReportPageItem } from '../common/models/order';
import { ServiceBase } from './service-base'
import { AuthService } from './auth-service';
import { HttpClientFacade } from './http-client-facade';

@autoinject
export class OrderService extends ServiceBase<Order> {
  private _auth: AuthService;

  constructor(httpClient: HttpClientFacade, auth: AuthService) {
    super('orders', httpClient);
    this._auth = auth;
  }

  getStatusList(): Promise<OrderStatus[]> {
    var url = this._resouce + '/statuses';
    return this._httpClient.get(url)
      .then(response => <OrderStatus[]>response);
  }

  getStatusLookup(): Promise<Lookup<OrderStatus>[]> {
    var url = this._resouce + '/status-lookups';
    return this._httpClient.get(url)
      .then(response => <Lookup<OrderStatus>[]>response);
  }

  getPayables(orderId: string): Promise<OrderPayable> {
    var url = this._resouce + '/' + orderId + '/payables';
    return this._httpClient.get(url)
      .then(response => <OrderPayable>response);
  }

  getReturnables(orderId: string): Promise<OrderReturnable> {
    var url = this._resouce + '/' + orderId + '/returnables';
    return this._httpClient.get(url)
      .then(response => <OrderReturnable>response);
  }

  getInvoiceDetail(orderId: string): Promise<OrderInvoiceDetail> {
    var url = this._resouce + '/' + orderId + '/invoice-detail';
    return this._httpClient.get(url)
      .then(response => <OrderInvoiceDetail>response);
  }

  getOrderReportPage(page: PageRequest): Promise<PagerResponse<OrderReportPageItem>> {
    var url = this._resouce + '/report/page';
    return this._httpClient.post(url, page)
      .then(response => <PagerResponse<OrderReportPageItem>>response);
  }

  /*
    getReceivables(id: string): Promise<OrderReceivable[]> {
      var url = this._resouce + '/' + id + '/receivables';
      return this._httpClient.get(url)
        .then(response => <OrderReceivable[]>response);
    }
  */

  save(order: Order): Promise<Order> {
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
        pricingScheme: order.pricingScheme,
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
        pricingScheme: order.pricingScheme,
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

  stage(order: Order): Promise<Order> {
    var url = this._resouce + '/' + order.id + '/staged';
    return this._httpClient.post(url, <Order>{
      id: order.id,
      stagedBy: this._auth.userAsLookup,
      stagedOn: new Date()
    });
  }

  route(order: Order): Promise<Order> {
    var url = this._resouce + '/' + order.id + '/routed';
    return this._httpClient.post(url, <Order>{
      id: order.id,
      routedBy: this._auth.userAsLookup,
      routedOn: new Date()
    });
  }

  invoice(order: Order): Promise<Order> {
    var url = this._resouce + '/' + order.id + '/invoiced';
    return this._httpClient.post(url, <Order>{
      id: order.id,
      invoicedBy: this._auth.userAsLookup,
      invoicedOn: new Date()
    });
  }

  ship(order: Order): Promise<Order> {
    var url = this._resouce + '/' + order.id + '/shipped';
    return this._httpClient.post(url, <Order>{
      id: order.id,
      shippedBy: this._auth.userAsLookup,
      shippedOn: new Date()
    });
  }

  complete(order: Order): Promise<Order> {
    var url = this._resouce + '/' + order.id + '/completed';
    return this._httpClient.post(url, <Order>{
      id: order.id,
      completedBy: this._auth.userAsLookup,
      completedOn: new Date()
    });
  }

  cancel(order: Order): Promise<Order> {
    var url = this._resouce + '/' + order.id + '/cancelled';
    return this._httpClient.post(url, <Order>{
      id: order.id,
      cancelledBy: this._auth.userAsLookup,
      cancelledOn: new Date()
    });
  }

  
}
