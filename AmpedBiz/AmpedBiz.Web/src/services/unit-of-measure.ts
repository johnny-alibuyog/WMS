import { autoinject } from 'aurelia-framework';
import { UnitOfMeasure } from '../common/models/unit-of-measure'
import { ServiceBase } from './service-base'
import { HttpClientFacade } from './http-client-facade';
import { Lookup } from '../common/custom_types/lookup';

@autoinject
export class UnitOfMeasureService extends ServiceBase<UnitOfMeasure> {
  constructor(httpClient: HttpClientFacade) {
    super('unit-of-measures', httpClient);
  }

  getLookups(): Promise<Lookup<string>[]> {
    var url = "unit-of-measure-lookups";
    return this._httpClient.get(url)
      .then(data => <Lookup<string>[]>data);
  }
}
