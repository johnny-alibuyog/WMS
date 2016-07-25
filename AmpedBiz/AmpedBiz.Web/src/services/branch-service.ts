import {autoinject} from 'aurelia-framework';
import {Branch} from '../common/models/branch'
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';
import {Lookup} from '.././common/custom_types/lookup';

@autoinject
export class BranchService extends ServiceBase<Branch> {
  constructor(httpClient: HttpClientFacade) {
    super('branches', httpClient);
  } 

  getLookups(): Promise<Lookup<string>[]>{
    var url = "branch-lookups";
    return this._httpClient.get(url)
      .then(data => <Lookup<string>[]>data);
  }
}