import * as numeral from 'numeral';

export class NumberFormatValueConverter {
  toView(value, format) {
    if (!value)
      return null;
    
    if (!format)
      format = '0,0.00';

    return numeral(value).format(format);
  }
}