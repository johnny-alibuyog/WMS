import { Role } from './role';
import { Person } from './person';
import { Branch } from './branch';
import { Address } from './address';
import { Expression } from 'aurelia-binding';

export interface User {
  id?: string;
  username?: string;
  password?: string;
  branchId?: string;
  branch?: Branch;
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

export interface UserAddress {
  id?: string;
  address: Address;
}

export interface UserInfo {
  id?: string;
  person?: Person;
}

export interface UserPassword {
  id?: string;
  oldPassword?: string;
  newPassword?: string;
  confirmPassword?: string;
}

export interface UserResetPassword {
  id?: string;
}