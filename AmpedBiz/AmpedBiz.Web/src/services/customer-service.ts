import { autoinject } from 'aurelia-framework';
import { Customer, CustomerReportPageItem } from '../common/models/customer'
import { PageRequest, PagerResponse } from '../common/models/paging';
import { ServiceBase } from './service-base'
import { HttpClientFacade } from './http-client-facade';
import { Lookup } from '.././common/custom_types/lookup';

@autoinject
export class CustomerService extends ServiceBase<Customer> {
  constructor(httpClient: HttpClientFacade) {
    super('customers', httpClient);
  }

  getLookups(): Promise<Lookup<string>[]> {
    var url = "customer-lookups";
    return this._httpClient.get(url)
      .then(data => <Lookup<string>[]>data);
  }

  getCustomerReportPage(page: PageRequest): Promise<PagerResponse<CustomerReportPageItem>> {
    var url = this._resouce + '/report/page';
    return this._httpClient.post(url, page)
      .then(response => <PagerResponse<CustomerReportPageItem>>response);
  }
}