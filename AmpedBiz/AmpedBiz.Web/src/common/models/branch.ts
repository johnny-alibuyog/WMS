import { Address } from './address';
import { Contact } from './contact';
import { Tenant } from "./tenant";

export interface Branch {
  id?: string;
  name?: string;
  tenant?: Tenant;
  description?: string;
  taxpayerIdentificationNumber?: string;
  contact?: Contact;
  address?: Address;
}

export interface BranchPageItem {
  id?: string;
  name?: string;
  description?: string;
  address?: Address;
}