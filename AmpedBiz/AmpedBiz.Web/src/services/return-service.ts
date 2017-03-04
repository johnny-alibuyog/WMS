import { autoinject } from 'aurelia-framework';
import { Lookup } from '../common/custom_types/lookup';
import { PageRequest } from '../common/models/paging';
import { Return, ReturnsByCustomerPageItem, ReturnsByProductPageItem, ReturnsByReasonPageItem } from '../common/models/return';
import { ServiceBase } from './service-base'
import { AuthService } from './auth-service';
import { HttpClientFacade } from './http-client-facade';

@autoinject
export class ReturnService extends ServiceBase<Return> {
  private _auth: AuthService;

  constructor(httpClient: HttpClientFacade, auth: AuthService) {
    super('returns', httpClient);
    this._auth = auth;
  }

  getReturnsByCustomerPage(page: PageRequest): Promise<ReturnsByCustomerPageItem> {
    var url = 'returns-by-customer/page';
    return this._httpClient.post(url, page)
      .then(data => <ReturnsByCustomerPageItem>data);
  }

  getReturnsByProductPage(page: PageRequest): Promise<ReturnsByProductPageItem> {
    var url = 'returns-by-product/page';
    return this._httpClient.post(url, page)
      .then(data => <ReturnsByProductPageItem>data);
  }

  getReturnsByReasonPage(page: PageRequest): Promise<ReturnsByReasonPageItem> {
    var url = 'returns-by-reason/page';
    return this._httpClient.post(url, page)
      .then(data => <ReturnsByReasonPageItem>data);
  }
}
