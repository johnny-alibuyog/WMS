import {autoinject} from 'aurelia-framework';
import {Supplier} from './common/models/supplier'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class SupplierService {
  private resouce: string = 'suppliers';
  private httpClient: HttpClientFacade;
  
  constructor(httpClient: HttpClientFacade) {
    this.httpClient = httpClient;
  } 
  
  getSupplier(id: string) : Promise<any> {
    var url = this.resouce + '/' + id;
    return this.httpClient.send({url: url}); 
  }
  
  getSuppliers(params: any) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url}); 
  }
  
  createSupplier(supplier: Supplier) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'POST', data: supplier}); 
  }

  updateSupplier(supplier: Supplier) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'PUT', data: supplier}); 
  }
}