import { Address } from './address';
import { Contact } from './contact';

export type Supplier = {
  id?: string;
  code?: string;
  name?: string;
  contactPerson?: string;
  address?: Address;
  contact?: Contact;
  assigned?: boolean;
}

export type SupplierPageItem = {
  id?: string;
  code?: string;
  name?: string;
  contactPerson?: string;
  address?: Address;
  contact?: Contact;
}

export type SupplierReportPageItem = {
  id?: string;
  code?: string;
  name?: string;
  contactPerson?: string;
  contact?: Contact;
  address?: Address;
}
