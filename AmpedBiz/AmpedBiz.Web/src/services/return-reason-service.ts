import { autoinject } from 'aurelia-framework';
import { ReturnReason } from '../common/models/return-reason'
import { ServiceBase } from './service-base'
import { HttpClientFacade } from './http-client-facade';
import { Lookup } from '.././common/custom_types/lookup';

@autoinject
export class ReturnReasonService extends ServiceBase<ReturnReason> {
  constructor(httpClient: HttpClientFacade) {
    super('return-reasons', httpClient);
  }

  getLookups(): Promise<Lookup<string>[]> {
    var url = "return-reason-lookups";
    return this._httpClient.get(url)
      .then(data => <Lookup<string>[]>data);
  }
}
