import {autoinject} from 'aurelia-framework';
import {UnitOfMeasureClass} from '../common/models/unit-of-measure-class'
import {ServiceBase} from './service-base'
import {HttpClientFacade} from './http-client-facade';

@autoinject
export class UnitOfMeasureClassService extends ServiceBase<UnitOfMeasureClass> {
  constructor(httpClient: HttpClientFacade) {
    super('unit-of-measure-classes', httpClient);
  } 
}
