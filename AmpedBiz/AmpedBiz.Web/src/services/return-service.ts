import { autoinject } from 'aurelia-framework';
import { PageRequest, PagerResponse } from '../common/models/paging';
import { Return, ReturnsByCustomerPageItem, ReturnsByProductPageItem, ReturnsByReasonPageItem, ReturnsByCustomerDetailsPageItem, ReturnsByProductDetailsPageItem, ReturnsByReasonDetailsPageItem } from '../common/models/return';
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

  public getReturnsByCustomerPage(page: PageRequest): Promise<PagerResponse<ReturnsByCustomerPageItem>> {
    var url = 'returns-by-customer/page';
    return this._httpClient.post(url, page);
  }

  public getReturnsByCustomerDetailsPage(page: PageRequest): Promise<PagerResponse<ReturnsByCustomerDetailsPageItem>> {
    var url = 'returns-by-customer-details/page';
    return this._httpClient.post(url, page);
  }

  public getReturnsByProductPage(page: PageRequest): Promise<PagerResponse<ReturnsByProductPageItem>> {
    var url = 'returns-by-product/page';
    return this._httpClient.post(url, page);
  }

  public getReturnsByProductDetailsPage(page: PageRequest): Promise<PagerResponse<ReturnsByProductDetailsPageItem>> {
    var url = 'returns-by-product-details/page';
    return this._httpClient.post(url, page);
  }

  public getReturnsByReasonPage(page: PageRequest): Promise<PagerResponse<ReturnsByReasonPageItem>> {
    var url = 'returns-by-reason/page';
    return this._httpClient.post(url, page);
  }
  
  public getReturnsByReasonDetailsPage(page: PageRequest): Promise<PagerResponse<ReturnsByReasonDetailsPageItem>> {
    var url = 'returns-by-reason-details/page';
    return this._httpClient.post(url, page);
  }}
