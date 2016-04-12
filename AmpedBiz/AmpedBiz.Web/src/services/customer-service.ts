import {autoinject} from 'aurelia-framework';
import {Customer} from './common/models/customer'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class CustomerService {
  private resouce: string = 'customers';
  private httpClient: HttpClientFacade;
  
  constructor(httpClient: HttpClientFacade) {
    this.httpClient = httpClient;
  } 
  
  getCustomer(id: string) : Promise<any> {
    var url = this.resouce + '/' + id;
    return this.httpClient.send({url: url}); 
  }
  
  getCustomers(params: any) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url}); 
  }
  
  createCustomer(customer: Customer) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'POST', data: customer}); 
  }

  updateCustomer(customer: Customer) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'PUT', data: customer}); 
  }
}