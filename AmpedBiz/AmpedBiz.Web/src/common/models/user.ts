import { Role } from './role';
import { Person } from './person';
import { Branch } from './branch';
import { Address } from './address';

export type User = {
  id?: string;
  username?: string;
  password?: string;
  branchId?: string;
  branch?: Branch;
  person?: Person;
  address?: Address;
  roles?: Role[];
}

export type UserPageItem = {
  id?: string;
  username?: string;
  branchName?: string;
  person?: Person;
}

export type UserAddress = {
  id?: string;
  address: Address;
}

export type UserInfo = {
  id?: string;
  person?: Person;
}

export type UserPassword = {
  id?: string;
  oldPassword?: string;
  newPassword?: string;
  confirmPassword?: string;
}

export type UserResetPassword = {
  id?: string;
}
