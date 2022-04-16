import moment from 'moment';
import { appConfig } from '../../app-config';

export class DateFormatValueConverter {

  toView(value, format) {
    if (!value) {
      return null;
    }

    if (!format) {
      format = appConfig.default.dateFormat;
    }

    return moment(value).format(format);
  }

  /*
  fromView(value) {
    if (!value) {
      return null;
    }

    return moment(value, appConfig.default.dateFormat);
  }
  */

  /*

  toView(value) {
    console.warn('to', value);
    return moment(value).format('YYYY-MM-DD HH:mm');
  }
  fromView(value) {
    console.warn('from', value);
    return moment(value,'YYYY-MM-DD HH:mm');
  }

  /*
  toView(value, format) {
    if (!value)
      return null;

    if (!format)
      format = 'MM/DD/YYYY'

    return moment(value).format(format);
  }
  */
}