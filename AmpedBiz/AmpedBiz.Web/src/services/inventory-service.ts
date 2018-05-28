import { PageRequest, PagerResponse } from '../common/models/paging';
import { Inventory, InventoryAdjustmentPageItem, InventoryAdjustmentType, InventoryAdjustmentReason, InventoryAdjustment } from '../common/models/inventory';

import { autoinject } from 'aurelia-framework';
import { buildQueryString } from 'aurelia-framework';
import { HttpClientFacade } from './http-client-facade';
import { Lookup } from '../common/custom_types/lookup';
import { ServiceBase } from './service-base'

@autoinject
export class InventoryService extends ServiceBase<Inventory> {
  constructor(httpClient: HttpClientFacade) {
    super('inventories', httpClient);
  }

  public getAdjustmentPage(page: PageRequest): Promise<PagerResponse<InventoryAdjustmentPageItem>> {
    var url = this._resouce + '/' + page.filter["id"] + '/adjustments/page';
    return this._httpClient.post(url, page);
  }

  public getAdjustmentTypeLookup(): Promise<Lookup<InventoryAdjustmentType>[]> {
    var url = this._resouce + '/adjustment-type-lookup';
    return this._httpClient.get(url);
  }

  public getAdjustmentTypeList(): Promise<InventoryAdjustmentType[]> {
    var url = this._resouce + '/adjustment-types';
    return this._httpClient.get(url);
  }

  public getAdjustmentReasonList(type?: InventoryAdjustmentType): Promise<InventoryAdjustmentReason[]> {
    var url = this._resouce + '/adjustment-reasons';

    if (type) {
      url += "/?" + buildQueryString({ type: type });
    }

    return this._httpClient.get(url);
  }

  public createAdjustment(entity: InventoryAdjustment): Promise<InventoryAdjustment>{
    var url = `${this._resouce}/${entity.inventoryId}/adjustments`;
    
    return this._httpClient.post(url, entity);
  }
}