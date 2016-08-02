import * as moment from 'moment';

export class SortValueConverter {
  toView(array, propertyName, comparison = 'ordinalIgnoreCase', direction = 'ascending') {
    var directionFactor = direction === 'ascending' ? 1 : -1,
        comparer = this[comparison + 'Comparison'];
    if (propertyName === undefined)
      return array.sort((a, b) => comparer(a, b) * directionFactor);
    return array.sort((a, b) => comparer(a[propertyName], b[propertyName]) * directionFactor);
  }

  ordinalIgnoreCaseComparison(a, b) {
    if ((a === null || a === undefined) && (b === null || b === undefined))
      return 0;
    if (a === null || a === undefined)
      return -1;
    if (b === null || b === undefined)
      return 1;
    a = a.toString().toLowerCase();
    b = b.toString().toLowerCase();
    if (a < b)
        return -1;
    if (a > b)
        return 1;
    return 0;
  }

  ordinalComparison(a, b) {
    if ((a === null || a === undefined) && (b === null || b === undefined))
      return 0;
    if (a === null || a === undefined)
      return -1;
    if (b === null || b === undefined)
      return 1;
    a = a.toString();
    b = b.toString();
    if (a < b)
        return -1;
    if (a > b)
        return 1;
    return 0;
  }

  dateComparison(a, b) {
    if ((a === null || a === undefined) && (b === null || b === undefined))
      return 0;
    if (a === null || a === undefined)
      return -1;
    if (b === null || b === undefined)
      return 1;
    return moment(b).diff(moment(a));
  }

  numberComparison(a, b) {
    if ((a === null || a === undefined) && (b === null || b === undefined))
      return 0;
    if (a === null || a === undefined)
      return -1;
    if (b === null || b === undefined)
      return 1;
    return a - b;
  }
}