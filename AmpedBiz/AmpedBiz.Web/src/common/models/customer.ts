import {Address} from 'address';
import {Contact} from 'contact'

export interface Customer {
  id: string;
  name: string;
  contact: Contact;
  billingAddress: Address;
  officeAddress: Address;
}