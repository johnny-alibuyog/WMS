import { OrderAggregate, OrderStatus } from '../common/models/order'

export class IsTransitionAllowedToValueConverter {

  toView(allowedTransitions: OrderStatus[], statusString: string) :boolean {
    if (!allowedTransitions) {
      return false;
    }

    return allowedTransitions.find(x => x == OrderStatus[statusString]) != undefined;
  }
}

export class IsModificationDisallowedToValueConverter {

  toView(allowedModifications: OrderAggregate[], aggregateString: string) :boolean {
    if (!allowedModifications) {
      return false;
    }

    return allowedModifications.find(x => x == OrderAggregate[aggregateString]) == undefined;
  }
}

export class CanModifyValueConverter{
  
  toView(allowedModifications: OrderAggregate[], aggregateString: string) :boolean {
    if (!allowedModifications) {
      return false;
    }

    return allowedModifications.length > 0;
  }
}