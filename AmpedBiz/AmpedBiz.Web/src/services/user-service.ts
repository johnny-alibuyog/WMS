import {autoinject} from 'aurelia-framework';
import {User} from './common/models/user'
import {HttpClientFacade, Callback} from './http-client-facade';

@autoinject
export class UserService {
  private resouce: string = 'users';
  private httpClient: HttpClientFacade;
  
  constructor(httpClient: HttpClientFacade) {
    this.httpClient = httpClient;
  } 
  
  getUser(id: string, callback: Callback){
    var url = this.resouce + '/' + id;
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  getInitialUser(id: string, callback: Callback){
    var url = this.resouce + '/initial';
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  getUsers(params: any, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, callback: callback}); 
  }
  
  createUser(user: User, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'POST', data: user, callback: callback}); 
  }

  updateUser(user: User, callback: Callback){
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'PUT', data: user, callback: callback}); 
  }
}