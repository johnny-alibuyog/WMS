import {autoinject} from 'aurelia-framework';
import {Branch} from './common/models/branch'
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class BranchService extends ServiceBase<Branch> {
  constructor(httpClient: HttpClientFacade) {
    super('branches', httpClient);
  } 
}