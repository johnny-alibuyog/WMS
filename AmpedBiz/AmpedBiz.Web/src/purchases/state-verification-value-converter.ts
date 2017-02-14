import { PurchaseOrderAggregate, PurchaseOrderStatus } from '../common/models/purchase-order'

export class IsTransitionAllowedToValueConverter {

  toView(allowedTransitions: PurchaseOrderStatus[], statusString: string) :boolean {
    if (!allowedTransitions) {
      return false;
    }

    return allowedTransitions.find(x => x == PurchaseOrderStatus[statusString]) != undefined;
  }
}

export class IsModificationDisallowedToValueConverter {

  toView(allowedModifications: PurchaseOrderAggregate[], aggregateString: string) :boolean {
    if (!allowedModifications) {
      return false;
    }

    return allowedModifications.find(x => x == PurchaseOrderAggregate[aggregateString]) == undefined;
  }
}

export class CanModifyValueConverter{
  
  toView(allowedModifications: PurchaseOrderAggregate[], aggregateString: string) :boolean {
    if (!allowedModifications) {
      return false;
    }

    return allowedModifications.length > 0;
  }
}