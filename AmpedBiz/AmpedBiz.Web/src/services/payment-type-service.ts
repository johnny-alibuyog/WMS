import {autoinject} from 'aurelia-framework';
import {PaymentType} from './common/models/payment-type'
import {HttpClientFacade, Callback} from './http-client-facade';

@autoinject
export class PaymentTypeService {
  private resouce: string = 'payment-types';
  private httpClient: HttpClientFacade;
  
  constructor(httpClient: HttpClientFacade) {
    this.httpClient = httpClient;
  } 
  
  getPaymentType(id: string, callback: Callback){
    var url = this.resouce + '/' + id;
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  getPaymentTypes(params: any, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  createPaymentType(paymentType: PaymentType, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'POST', data: paymentType, callback: callback}); 
  }

  updatePaymentType(paymentType: PaymentType, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'PUT', data: paymentType, callback: callback}); 
  }
}