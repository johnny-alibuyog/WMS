import {autoinject, buildQueryString} from 'aurelia-framework';
import {Product, ProductInventory, ProductReportPageItem} from '../common/models/product';
import {PageRequest, PagerResponse} from '../common/models/paging';
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';
import {Lookup} from '../common/custom_types/lookup';

@autoinject
export class ProductService extends ServiceBase<Product> {
  constructor(httpClient: HttpClientFacade) {
    super('products', httpClient);
  }

  getInventory(productId: string): Promise<ProductInventory> {
    var url = 'product-inventories/' + productId;
    return this._httpClient.get(url)
      .then(response => <ProductInventory>response);
  }

  getInventoryList(productIds?: string[]): Promise<ProductInventory[]> {
    var queryString = (productIds) ? '/?' + buildQueryString({ productIds: productIds }) : '';
    var url = 'product-inventories' + queryString;

    return this._httpClient.get(url)
      .then(response => <ProductInventory[]>response);
  }

  getInventoryLevelPage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/inventory-level/page';
    return this._httpClient.post(url, page);
  }

  getDiscontinuedPage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/discontinued/page';
    return this._httpClient.post(url, page);
  }

  getLookups(): Promise<Lookup<string>[]> {
    var url = "product-lookups";
    return this._httpClient.get(url)
      .then(data => <Lookup<string>[]>data);
  }

  getNeedsReorderingPage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/needs-reordering/page';
    return this._httpClient.post(url, page);
  }

  getOrderPage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/' + page.filter["id"] + '/orders/page';
    return this._httpClient.post(url, page);
  }

  getPurchasePage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/' + page.filter["id"] + '/purchases/page';
    return this._httpClient.post(url, page);
  }

  getProductReportPage(page: PageRequest): Promise<PagerResponse<ProductReportPageItem>> {
    var url = this._resouce + '/report/page';
    return this._httpClient.post(url, page)
      .then(response => <PagerResponse<ProductReportPageItem>>response);
  }
}
