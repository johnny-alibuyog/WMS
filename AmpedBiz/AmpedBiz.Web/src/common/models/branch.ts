import {Address} from './address';

export interface Branch {
  id?: string;
  name?: string;
  description?: string;
  address?: Address;
}

export interface BranchPageItem {
  id?: string;
  name?: string;
  description?: string;
  address?: Address;
}