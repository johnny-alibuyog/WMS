import {autoinject} from 'aurelia-framework';
import {User} from './common/models/user'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class UserService {
  private resouce: string = 'users';
  private httpClient: HttpClientFacade;
  
  constructor(httpClient: HttpClientFacade) {
    this.httpClient = httpClient;
  } 
  
  getUser(id: string) : Promise<any> {
    var url = this.resouce + '/' + id;
    return this.httpClient.send({url: url}); 
  }
  
  getInitialUser(id: string) : Promise<any> {
    var url = this.resouce + '/initial';
    return this.httpClient.send({url: url}); 
  }
  
  getUsers(params: any) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url}); 
  }
  
  createUser(user: User) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'POST', data: user}); 
  }

  updateUser(user: User) : Promise<any> {
    var url = this.resouce;
    return this.httpClient.send({url: url, method: 'PUT', data: user}); 
  }
}