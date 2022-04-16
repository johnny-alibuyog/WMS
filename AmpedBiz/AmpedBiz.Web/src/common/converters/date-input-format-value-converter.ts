import moment from 'moment';
import { appConfig } from '../../app-config';

export class DateInputFormatValueConverter {
  toView(value) {
    if (!value) {
      return null;
    }

    return moment(value).format('YYYY-MM-DD');
  }
}