import { PointOfSaleStatus } from '../common/models/point-of-sale';
import { Lookup } from '../common/custom_types/lookup';
import * as Enumerable from 'linq';

export class StatusToClassValueConverter {
  
  public toView(status: PointOfSaleStatus, isSaleTendered: boolean): string {
    if (!status) {
      return '';
    }

    if (!isSaleTendered) {
      return '';
    }

    switch (status) {
      case PointOfSaleStatus.unPaid:
        return 'alert alert-danger';
      case PointOfSaleStatus.partiallyPaid:
        return 'alert alert-warning';
      case PointOfSaleStatus.fullyPaid:
        return 'alert alert-success';
      default:
        return '';
    }
  }
}

export class StatusToDescriptionValueConverter{
  public toView(status: PointOfSaleStatus, statuses: Lookup<PointOfSaleStatus>[]): string{
    return Enumerable.from(statuses)
      .where(x => x.id == status)
      .select(x => x.name)
      .firstOrDefault(); 
  }
}




