import { PurchaseOrderAggregate, PurchaseOrderStatus, PurchaseOrderReceivable } from '../common/models/purchase-order'

export class IsTransitionAllowedToValueConverter {

  toView(allowedTransitions: PurchaseOrderStatus[], statusString: string): boolean {
    if (!allowedTransitions) {
      return false;
    }

    return allowedTransitions.find(x => x == PurchaseOrderStatus[statusString]) != undefined;
  }
}

export class IsModificationDisallowedToValueConverter {

  toView(allowedModifications: PurchaseOrderAggregate[], aggregateString: string, isPermitted: boolean): boolean {
    if (!allowedModifications) {
      return false;
    }

    var isAllowedViaConfiguration = allowedModifications.find(x => x == PurchaseOrderAggregate[aggregateString]) !== undefined;
    if (!isAllowedViaConfiguration) {
      return true;
    }

    var isAllowedViaPermission = isPermitted;
    if (!isAllowedViaPermission) {
      return true;
    }

    return false;
  }
}

export class CanModifyValueConverter {

  toView(allowedModifications: PurchaseOrderAggregate[], aggregateString: string): boolean {
    if (!allowedModifications) {
      return false;
    }

    return allowedModifications.length > 0;
  }
}

export class ProductCanBeChangedValueConverter {

  toView(receivable: PurchaseOrderReceivable): boolean {
    var cannotBeChanged = new ProductCannotBeChangedValueConverter().toView;
    return !cannotBeChanged(receivable);
  }
}

export class ProductCannotBeChangedValueConverter {

  toView(receivable: PurchaseOrderReceivable): boolean {
    if (receivable && receivable.orderedQuantity > 0) {
      return true;
    }

    if (receivable && receivable.receivedQuantity > 0) {
      return true;
    }

    return false;
  }
}

/*
export class HasBeenSavedValueConverter {

  toView(entity: any): boolean {
    if (entity && entity.id) {
      return true;
    }

    return false;
  }
}

export class HasNotBeenSavedValueConverter {

  toView(entity: any): boolean {
    if (!entity || !entity.id) {
      return false;
    }

    return true;
  }
}
*/