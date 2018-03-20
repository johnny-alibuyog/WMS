import { PageRequest, PagerResponse } from '../common/models/paging';

import { HttpClientFacade } from './http-client-facade';
import { Lookup } from '../common/custom_types/lookup';
import { Product, NeedsReorderingPageItem, InventoryLevelPageItem, ProductSalesReportPageItem } from '../common/models/product';
import { ProductInventory } from '../common/models/product';
import { ProductOrderPageItem } from '../common/models/product';
import { ProductOrderReturnPageItem } from '../common/models/product';
import { ProductPurchasePageItem } from '../common/models/product';
import { ProductReportPageItem } from '../common/models/product';
import { ProductReturnPageItem } from '../common/models/product';
import { ServiceBase } from './service-base'
import { autoinject } from 'aurelia-framework';
import { buildQueryString } from 'aurelia-framework';

@autoinject
export class ProductService extends ServiceBase<Product> {
  constructor(httpClient: HttpClientFacade) {
    super('products', httpClient);
  }

  public getInventory(productId: string): Promise<ProductInventory> {
    var url = 'product-inventories/' + productId;
    return this._httpClient.get(url);
  }

  public getInventoryList(productIds?: string[]): Promise<ProductInventory[]> {
    var queryString = (productIds) ? '/?' + buildQueryString({ productIds: productIds }) : '';
    var url = 'product-inventories' + queryString;

    return this._httpClient.get(url);
  }

  public getInventoryLevelPage(page: PageRequest): Promise<PagerResponse<InventoryLevelPageItem>> {
    var url = this._resouce + '/inventory-level/page';
    return this._httpClient.post(url, page);
  }

  public getDiscontinuedPage(page: PageRequest): Promise<any> {
    var url = this._resouce + '/discontinued/page';
    return this._httpClient.post(url, page);
  }

  public getLookups(): Promise<Lookup<string>[]> {
    var url = "product-lookups";
    return this._httpClient.get(url);
  }

  public getNeedsReorderingPage(page: PageRequest): Promise<PagerResponse<NeedsReorderingPageItem>> {
    var url = this._resouce + '/needs-reordering/page';
    return this._httpClient.post(url, page);
  }

  public getOrderPage(page: PageRequest): Promise<ProductOrderPageItem> {
    var url = this._resouce + '/' + page.filter["id"] + '/orders/page';
    return this._httpClient.post(url, page);
  }

  public getOrderReturnPage(page: PageRequest): Promise<ProductOrderReturnPageItem> {
    var url = this._resouce + '/' + page.filter["id"] + '/order-returns/page';
    return this._httpClient.post(url, page);
  }

  public getReturnPage(page: PageRequest): Promise<ProductReturnPageItem> {
    var url = this._resouce + '/' + page.filter["id"] + '/returns/page';
    return this._httpClient.post(url, page);
  }

  public getPurchasePage(page: PageRequest): Promise<ProductPurchasePageItem> {
    var url = this._resouce + '/' + page.filter["id"] + '/purchases/page';
    return this._httpClient.post(url, page);
  }

  public getProductReportPage(page: PageRequest): Promise<PagerResponse<ProductReportPageItem>> {
    var url = this._resouce + '/report/page';
    return this._httpClient.post(url, page);
  }

  public getProductSalesReportPage(page: PageRequest): Promise<PagerResponse<ProductSalesReportPageItem>> {
    var url = this._resouce + '/sales-report/page';
    return this._httpClient.post(url, page);
  }
}
