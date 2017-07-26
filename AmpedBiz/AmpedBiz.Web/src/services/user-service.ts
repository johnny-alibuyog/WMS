import { autoinject } from 'aurelia-framework';
import { User } from '../common/models/user'
import { Lookup } from "../common/custom_types/lookup";
import { ServiceBase } from './service-base'
import { HttpClientFacade } from './http-client-facade';

@autoinject
export class UserService extends ServiceBase<User> {
  constructor(httpClient: HttpClientFacade) {
    super('users', httpClient);
  }

  getLookups(): Promise<Lookup<string>[]> {
    var url = "user-lookups";
    return this._httpClient.get(url)
      .then(data => <Lookup<string>[]>data);
  }

  getInitialUser(id: string): Promise<any> {
    var url = this._resouce + '/initial';
    return this._httpClient.get(url);
  }
}