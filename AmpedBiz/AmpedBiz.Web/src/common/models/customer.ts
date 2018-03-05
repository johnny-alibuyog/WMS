import { Address } from './address';
import { Contact } from './contact'

export interface Customer {
  id?: string;
  code?: string;
  name?: string;
  contactPerson?: string;
  contact?: Contact;
  billingAddress?: Address;
  officeAddress?: Address;
}

export interface CustomerPageItem {
  id?: string;
  code?: string;
  name?: string;
  description?: string;
  address?: Address;
}

export interface CustomerReportPageItem {
  id?: string;
  code?: string;
  name?: string;
  contactPerson?: string;
  creditLimitAmount?: number;
  contact?: Contact;
  officeAddress?: Address;
  billingAddress?: Address;
}
