import {autoinject} from 'aurelia-framework';
import {ProductCategory} from './common/models/product-category'
import {HttpClientFacade, Callback} from './http-client-facade';

@autoinject
export class ProductCategoryService {
  private resouce: string = 'product-categories';
  private httpClient: HttpClientFacade;
  
  constructor(httpClient: HttpClientFacade) {
    this.httpClient = httpClient;
  } 
  
  getProductCategory(id: string, callback: Callback){
    var url = this.resouce + '/' + id;
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  getProductCategorys(params: any, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  createProductCategory(productCategory: ProductCategory, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'POST', data: productCategory, callback: callback}); 
  }

  updateProductCategory(productCategory: ProductCategory, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'PUT', data: productCategory, callback: callback}); 
  }
}