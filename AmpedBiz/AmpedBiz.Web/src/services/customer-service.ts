import { autoinject } from 'aurelia-framework';
import { Customer, CustomerReportPageItem, CustomerSalesReportPageItem, CustomerOrderDeliveryReportPageItem, CustomerPaymentsReportPageItem } from '../common/models/customer'
import { PageRequest, PagerResponse } from '../common/models/paging';
import { ServiceBase } from './service-base'
import { HttpClientFacade } from './http-client-facade';
import { Lookup } from '.././common/custom_types/lookup';

@autoinject
export class CustomerService extends ServiceBase<Customer> {
  constructor(httpClient: HttpClientFacade) {
    super('customers', httpClient);
  }

  public getLookups(): Promise<Lookup<string>[]> {
    let url = "customer-lookups";
    return this._httpClient.get(url);
  }

  public getCustomerReportPage(page: PageRequest): Promise<PagerResponse<CustomerReportPageItem>> {
    let url = this._resouce + '/report/page';
    return this._httpClient.post(url, page);
  }

  public getCustomerSalesReportPage(page: PageRequest): Promise<PagerResponse<CustomerSalesReportPageItem>> {
    let url = this._resouce + '/sales-report/page';
    return this._httpClient.post(url, page);
  }

  public getCustomerPaymentsReportPage(page: PageRequest): Promise<PagerResponse<CustomerPaymentsReportPageItem>> {
    let url = this._resouce + '/payments-report/page';
    return this._httpClient.post(url, page);
  }

  public getCustomerOrderDeliveryReportPage(page: PageRequest): Promise<PagerResponse<CustomerOrderDeliveryReportPageItem>> {
    let url = this._resouce + '/order-delivery-report/page';
    return this._httpClient.post(url, page);
  }

  
}
