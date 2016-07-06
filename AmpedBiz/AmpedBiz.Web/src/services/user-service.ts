import {autoinject} from 'aurelia-framework';
import {User} from '../common/models/user'
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class UserService extends ServiceBase<User> {
  constructor(httpClient: HttpClientFacade) {
    super('users', httpClient);
  } 
    
  getInitialUser(id: string) : Promise<any> {
    var url = this._resouce + '/initial';
    return this._httpClient.get(url); 
  }
}