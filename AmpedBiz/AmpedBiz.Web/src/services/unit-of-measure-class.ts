import { autoinject } from 'aurelia-framework';
import { UnitOfMeasureClass } from '../common/models/unit-of-measure-class'
import { PageRequest, PagerResponse } from '../common/models/paging';
import { ServiceBase } from './service-base'
import { HttpClientFacade } from './http-client-facade';
import { Lookup } from '../common/custom_types/lookup';

@autoinject
export class UnitOfMeasureClassService extends ServiceBase<UnitOfMeasureClass> {
  constructor(httpClient: HttpClientFacade) {
    super('unit-of-measure-classes', httpClient);
  }

  getLookups(): Promise<Lookup<string>[]> {
    var url = "unit-of-measure-class-lookups";
    return this._httpClient.get(url)
      .then(data => <Lookup<string>[]>data);
  }

  getUnitOfMeasureClassReportPage(page: PageRequest): Promise<PagerResponse<UnitOfMeasureClass>> {
    var url = this._resouce + '/report/page';
    return this._httpClient.post(url, page)
      .then(response => <PagerResponse<UnitOfMeasureClass>>response);
  }
}
