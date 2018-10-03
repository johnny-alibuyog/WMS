import { autoinject } from 'aurelia-framework';
import { User, UserAddress, UserInfo, UserPassword, UserResetPassword } from '../common/models/user'
import { Lookup } from "../common/custom_types/lookup";
import { ServiceBase } from './service-base'
import { HttpClientFacade } from './http-client-facade';

@autoinject
export class UserService extends ServiceBase<User> {
  constructor(httpClient: HttpClientFacade) {
    super('users', httpClient);
  }

  public getLookups(): Promise<Lookup<string>[]> {
    var url = "user-lookups";
    return this._httpClient.get(url)
      .then(data => <Lookup<string>[]>data);
  }

  public getInitialUser(id: string): Promise<any> {
    var url = this._resouce + '/initial';
    return this._httpClient.get(url);
  }

  public getAddress(id: string): Promise<UserAddress>{
    var url = `${this._resouce}/${id}/address`;
    return this._httpClient.get(url);
  }

  public updateAddress(userAddress: UserAddress): Promise<void>{
    var url = `${this._resouce}/${userAddress.id}/address`;
    return this._httpClient.put(url, userAddress);
  }

  public getInfo(id: string): Promise<UserInfo>{
    var url = `${this._resouce}/${id}/info`;
    return this._httpClient.get(url);
  }

  public updateInfo(userInfo: UserInfo): Promise<void>{
    var url = `${this._resouce}/${userInfo.id}/info`;
    return this._httpClient.put(url, userInfo);
  }

  public updatePassword(userPassword: UserPassword): Promise<void>{
    var url = `${this._resouce}/${userPassword.id}/password`;
    return this._httpClient.put(url, userPassword);
  }

  public resetPassword(user: UserResetPassword): Promise<void>{
    var url = `${this._resouce}/${user.id}/reset-password`;
    return this._httpClient.post(url, user);
  }
}