import {autoinject} from 'aurelia-framework';
import {ProductType} from './common/models/product-type'
import {HttpClientFacade, Callback} from './http-client-facade';

@autoinject
export class ProductTypeService {
  private resouce: string = 'product-types';
  private httpClient: HttpClientFacade;
  
  constructor(httpClient: HttpClientFacade) {
    this.httpClient = httpClient;
  } 
  
  getProductType(id: string, callback: Callback){
    var url = this.resouce + '/' + id;
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  getProductTypes(params: any, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  createProductType(productType: ProductType, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'POST', data: productType, callback: callback}); 
  }

  updateProductType(productType: ProductType, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'PUT', data: productType, callback: callback}); 
  }
}