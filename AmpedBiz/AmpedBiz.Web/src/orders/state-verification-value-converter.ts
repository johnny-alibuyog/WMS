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

  toView(returnable: OrderReturnable, selected: OrderReturnable): boolean {
    if (returnable !== selected) {
      return false;
    }

    if (!returnable) {
      return false;
    }

    if (returnable.returnableQuantity === 0) {
      return false;
    }

    if (returnable.returnedQuantity > 0) {
      return false;
    }

    return true;
  }
}

export class ProductCannotBeChangedValueConverter {

  toView(returnable: OrderReturnable, selected: OrderReturnable): boolean {
    if (returnable !== selected) {
      return true;
    }

    if (!returnable) {
      return true;
    }

    if (returnable.returnableQuantity === 0) {
      return true;
    }

    if (returnable.returnedQuantity > 0) {
      return true;
    }

    return false;
  }
}
