import moment from 'moment';

export class AgeValueConverter {
  toView(dob: any) {
    if (!dob)
      return null;
    return Math.floor(moment().diff(moment(dob), 'year', false));
  }
}