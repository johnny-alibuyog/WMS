import { ProductUnitOfMeasurePrice } from "../common/models/product";

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