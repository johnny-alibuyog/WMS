import {Address} from 'address';
import {Contact} from 'contact';

export interface Supplier {
  id: string;
  name: string;
  address: Address;
  contact: Contact;
}