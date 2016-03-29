import {autoinject} from 'aurelia-framework';
import {Customer} from './common/models/customer'
import {HttpClientFacade, Callback} from './http-client-facade';

@autoinject
export class CustomerService {
  private resouce: string = 'customers';
  private httpClient: HttpClientFacade;
  
  constructor(httpClient: HttpClientFacade) {
    this.httpClient = httpClient;
  } 
  
  getCustomer(id: string, callback: Callback){
    var url = this.resouce + '/' + id;
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  getCustomers(params: any, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  createCustomer(customer: Customer, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'POST', data: customer, callback: callback}); 
  }

  updateCustomer(customer: Customer, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'PUT', data: customer, callback: callback}); 
  }
}