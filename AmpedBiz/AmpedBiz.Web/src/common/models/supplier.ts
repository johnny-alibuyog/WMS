import { Address } from './address';
import { Contact } from './contact';

export interface Supplier {
  id?: string;
  code?: string;
  name?: string;
  contactPerson?: string;
  address?: Address;
  contact?: Contact;
}

export interface SupplierPageItem {
  id?: string;
  code?: string;
  name?: string;
  contactPerson?: string;
  address?: Address;
  contact?: Contact;
}

export interface SupplierReportPageItem {
  id?: string;
  code?: string;
  name?: string;
  contactPerson?: string;
  contact?: Contact;
  address?: Address;
}