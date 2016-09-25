import {autoinject} from 'aurelia-framework';
import {Lookup} from '../common/custom_types/lookup';
import {PageRequest} from '../common/models/paging';
import {Return} from '../common/models/return';
import {ServiceBase} from './service-base'
import {AuthService} from './auth-service';
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class ReturnService extends ServiceBase<Return> {
  private _auth: AuthService;

  constructor(httpClient: HttpClientFacade, auth: AuthService) {
    super('returns', httpClient);
    this._auth = auth;
  }
}
