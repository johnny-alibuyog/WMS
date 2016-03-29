import {autoinject} from 'aurelia-framework';
import {Supplier} from './common/models/supplier'
import {HttpClientFacade, Callback} from './http-client-facade';

@autoinject
export class SupplierService {
  private resouce: string = 'suppliers';
  private httpClient: HttpClientFacade;
  
  constructor(httpClient: HttpClientFacade) {
    this.httpClient = httpClient;
  } 
  
  getSupplier(id: string, callback: Callback){
    var url = this.resouce + '/' + id;
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  getSuppliers(params: any, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  createSupplier(supplier: Supplier, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'POST', data: supplier, callback: callback}); 
  }

  updateSupplier(supplier: Supplier, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'PUT', data: supplier, callback: callback}); 
  }
}