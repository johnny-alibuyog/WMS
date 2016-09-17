import {appConfig} from '../app-config';
import * as moment from 'moment';
import * as numeral from 'numeral';

export function formatDate(value: Date, format?: string): string {
  if (value == null) {
    return '';
  }

  if (format === undefined || format === null || format.trim() === '') {
    format = appConfig.default.dateFormat;
  }

  return moment(value).format(format);
}

export function formatNumber(value: number, format?: string): string {
  if (value == null) {
    return '';
  }

  if (format === undefined || format === null || format.trim() === '') {
    format = appConfig.default.numberFormat;
  }

  return numeral(value).format(format);
}

export function emptyIfNull(value: string) {
  return value || '';
}