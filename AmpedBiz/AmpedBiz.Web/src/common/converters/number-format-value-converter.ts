import * as numeral from 'numeral';

export class NumberFormatValueConverter {
  public toView(value: number, format: string): any {
    if (!value)
      value = 0;

    if (!format)
      format = '0,0.00';

    let number = numeral(value).format(format);

    console.log('converted: ', number);

    return number;
  }
}
