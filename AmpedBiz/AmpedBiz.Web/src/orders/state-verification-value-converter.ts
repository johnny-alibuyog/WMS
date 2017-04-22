import { OrderAggregate, OrderStatus, OrderItem, OrderPayment, OrderReturn, OrderReturnable, OrderReturning } from '../common/models/order'

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

    return allowedModifications.find(x => x == OrderAggregate[aggregateString]) === undefined;
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

    if (returnable.returnableQuantity === 0 && returnable.product) {
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

    if (returnable.returnableQuantity === 0 && returnable.product) {
      return true;
    }

    if (returnable.returnedQuantity > 0) {
      return true;
    }

    return false;
  }
}

export class SumItemDiscountValueConverter {

  toView(items: OrderItem[]): number {
    if (!items) {
      return 0;
    }

    if (items.length == 0) {
      return 0;
    }

    return items.reduce((value, item) => value + item.discountAmount, 0);
  }
}

export class SumItemPriceValueConverter {

  toView(items: OrderItem[]): number {
    if (!items) {
      return 0;
    }

    if (items.length == 0) {
      return 0;
    }

    return items.reduce((value, item) => value + item.totalPriceAmount, 0);
  }
}


export class TotalItemPriceValueConverter {

  toView(items: OrderItem[]): number {
    var discount = new SumItemDiscountValueConverter().toView(items);
    var price = new SumItemPriceValueConverter().toView(items);
    var total = price - discount;

    if (total < 0) {
      total = 0;
    }

    return total;
  }
}


export class TotalPaymentValueConverter {

  toView(payments: OrderPayment[]): number {
    if (!payments) {
      return 0;
    }

    if (payments.length == 0) {
      return 0;
    }

    return payments.reduce((value, item) => value + item.paymentAmount, 0);
  }
}

export class TotalReturnedValueConverter {

  toView(returns: OrderReturn[]): number {
    if (!returns) {
      return 0;
    }

    if (returns.length == 0) {
      return 0;
    }

    return returns.reduce((value, item) => value + item.returnedAmount, 0);
  }
}

export class TotalReturningValueConverter {

  toView(returnings: OrderReturnable[]): number {
    if (!returnings) {
      return 0;
    }

    if (returnings.length == 0) {
      return 0;
    }

    return returnings.reduce((value, item) => value + item.returning.amount, 0);
  }
}
