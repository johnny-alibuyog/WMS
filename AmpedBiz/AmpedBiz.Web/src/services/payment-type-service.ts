import {autoinject} from 'aurelia-framework';
import {PaymentType} from './common/models/payment-type'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class PaymentTypeService {
  private resouce: string = 'payment-types';
  private httpClient: HttpClientFacade;
  
  constructor(httpClient: HttpClientFacade) {
    this.httpClient = httpClient;
  } 
  
  getPaymentType(id: string) : Promise<any> {
    var url = this.resouce + '/' + id;
    return this.httpClient.send({url: url}); 
  }
  
  getPaymentTypes(params: any) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url}); 
  }
  
  createPaymentType(paymentType: PaymentType) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'POST', data: paymentType}); 
  }

  updatePaymentType(paymentType: PaymentType) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'PUT', data: paymentType}); 
  }
}