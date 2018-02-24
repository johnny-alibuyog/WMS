import { ProductUnitOfMeasurePrice, ProductUnitOfMeasure } from "../common/models/product";

export class SortPriceValueConverter {
  toView(array: ProductUnitOfMeasurePrice[] = [], direction: string = 'asc') {
    if (!array)
      return array;
    let factor = direction.match(/^desc*/i) ? 1 : -1;
    let retvalue = array.sort((left, right) => {
      let leftText = left.pricing && left.pricing.name || '';
      let rightText = right.pricing && right.pricing.name || '';
      return (leftText < rightText) ? factor : (leftText > rightText) ? -factor : 0;
    });
    return retvalue;
  }
}

export class DefaultValueConverter {

  toView(value: number, items: ProductUnitOfMeasure[]): number {
    // "value" is standard unit, we should display the "default" equivalent
    return value;

    /*
    if (!value) {
      return null;
    }

    if (!items || items.length === 0) {
      return null;
    }

    let tefault = items.find(x => x.isDefault);

    if (!tefault) {
      return null;
    }

    return value / tefault.standardEquivalentValue;
    */
  }

  fromView(value: number, items: ProductUnitOfMeasure[]): number {
    // "value" is standard unit, we should display the "default" equivalent
    return value;

    /*
    if (!value) {
      return null;
    }

    if (!items || items.length === 0) {
      return null;
    }

    let tefault = items.find(x => x.isDefault);

    if (!tefault) {
      return null;
    }

    return value * tefault.standardEquivalentValue;
    */
  }
}

export class DefaultUnitOfMeasureNameValueConverter {

  toView(items: ProductUnitOfMeasure[]): string {
    if (!items || items.length === 0) {
      return null;
    }

    let tefault = items.find(x => x.isDefault);

    return tefault && tefault.unitOfMeasure && tefault.unitOfMeasure.name || '';
  }

}