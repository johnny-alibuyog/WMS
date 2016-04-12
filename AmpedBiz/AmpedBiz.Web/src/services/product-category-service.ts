import {autoinject} from 'aurelia-framework';
import {ProductCategory} from './common/models/product-category'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class ProductCategoryService {
  private resouce: string = 'product-categories';
  private httpClient: HttpClientFacade;
  
  constructor(httpClient: HttpClientFacade) {
    this.httpClient = httpClient;
  } 
  
  getProductCategory(id: string) : Promise<any> {
    var url = this.resouce + '/' + id;
    return this.httpClient.send({url: url}); 
  }
  
  getProductCategories(params: any) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url}); 
  }
  
  createProductCategory(productCategory: ProductCategory) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'POST', data: productCategory}); 
  }

  updateProductCategory(productCategory: ProductCategory) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'PUT', data: productCategory}); 
  }
}