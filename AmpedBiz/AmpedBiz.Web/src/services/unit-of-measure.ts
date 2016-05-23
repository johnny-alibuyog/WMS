import {autoinject} from 'aurelia-framework';
import {UnitOfMeasure} from './common/models/unit-of-measure'
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class UnitOfMeasureService extends ServiceBase<UnitOfMeasure> {
  constructor(httpClient: HttpClientFacade) {
    super('unit-of-measures', httpClient);
  } 
}
