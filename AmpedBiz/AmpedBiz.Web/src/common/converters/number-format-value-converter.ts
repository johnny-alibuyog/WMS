import * as numeral from 'numeral';

export class NumberFormatValueConverter {
  toView(value, format) {
    if (!value)
      value = 0;
    
    if (!format)
      format = '0,0.00';

    return numeral(value).format(format);
  }
}