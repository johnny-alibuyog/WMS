import {appConfig} from '../app-config';
import * as moment from 'moment';
import * as numeral from 'numeral';
import {Address} from '../common/models/address';
import {Contact} from '../common/models/contact';

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

export function addressAsTring(value: Address): string {
  let result = '';
  result += value.street ? `${value.street} ` : '';
  result += value.barangay ? `${value.barangay} ` : '';
  result += value.province ? `${value.province} ` : '';
  result += value.city ? `${value.city} ` : '';
  result += value.country ? `${value.country} ` : '';
  result += value.zipCode ? `${value.zipCode} ` : '';
  return result; 
}

export function contactAsString(value: Contact): string{
  let result = '';
  result += value.email ? `Email: ${value.email}\n` : '';
  result += value.landline ? `Landline: ${value.landline}\n` : '';
  result += value.fax ? `Fax: ${value.fax}\n` : '';
  result += value.mobile ? `Mobile: ${value.mobile}\n` : '';
  result += value.web ? `Web: ${value.web}\n` : '';
  return result; 
} 

export function emptyIfNull(value: string) {
  return value || '';
}