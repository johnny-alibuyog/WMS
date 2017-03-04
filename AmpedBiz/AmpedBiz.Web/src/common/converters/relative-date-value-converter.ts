import * as moment from 'moment';

export class RelativeDateValueConverter {
  toView(value) {
    if (!value)
      return null;
    return moment(value).fromNow();
  }
}