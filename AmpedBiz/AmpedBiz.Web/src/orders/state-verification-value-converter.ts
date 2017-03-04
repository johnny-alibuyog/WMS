import { OrderAggregate, OrderStatus, OrderReturnable } from '../common/models/order'

export class IsTransitionAllowedToValueConverter {

  toView(allowedTransitions: OrderStatus[], statusString: string): boolean {
    if (!allowedTransitions) {
      return false;
    }

    return allowedTransitions.find(x => x == OrderStatus[statusString]) != undefined;
  }
}

export class IsModificationDisallowedToValueConverter {

  toView(allowedModifications: OrderAggregate[], aggregateString: string): boolean {
    if (!allowedModifications) {
      return false;
    }

    return allowedModifications.find(x => x == OrderAggregate[aggregateString]) == undefined;
  }
}

export class CanModifyValueConverter {

  toView(allowedModifications: OrderAggregate[], aggregateString: string): boolean {
    if (!allowedModifications) {
      return false;
    }

    return allowedModifications.length > 0;
  }
}

export class ProductCanBeChangedValueConverter {

  toView(returnable: OrderReturnable): boolean {
    var cannotBeChanged = new ProductCannotBeChangedValueConverter().toView;
    return !cannotBeChanged(returnable);
  }
}

export class ProductCannotBeChangedValueConverter {

  toView(returnable: OrderReturnable): boolean {
    if (returnable && returnable.returnedQuantity > 0) {
      return true;
    }

    return false;
  }
}
