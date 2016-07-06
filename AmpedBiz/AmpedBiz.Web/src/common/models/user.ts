import {Role} from './role';
import {Person} from './person';
import {Address} from './address';

export interface User {
  id?: string;
  username?: string;
  password?: string;
  branchId?: string;
  person?: Person;
  address?: Address;
  roles?: Role[]; 
}

export interface UserPageItem {
  id?: string;
  username?: string;
  branchName?: string;
  person?: Person;
}