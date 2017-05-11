import { Contact } from './contact';
import { Address } from './address';

export interface Branch {
  id?: string;
  name?: string;
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