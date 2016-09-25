import {Lookup} from '../custom_types/lookup';
import {ProductInventory} from './product';
import {Dictionary} from '../custom_types/dictionary';
import {Address} from './Address';

export const returnEvents = {
  item: {
    add: 'return-item-add',
    added: 'return-item-added',
    deleted: 'return-item-deleted',
  },
}

export interface Return {
  id?: string;
  branch?: Lookup<string>;
  customer?: Lookup<string>;
  returnedBy?: Lookup<string>;
  returnedOn?: Date;
  reason?: Lookup<string>;
  remarks?: string;
  totalAmount?: number;
  items?: ReturnItem[];
}

export interface ReturnItem {
  id?: string;
  returnId?: string;
  product?: Lookup<string>;
  returnReason?: Lookup<string>;
  quantityValue?: number;
  unitPriceAmount?: number;
  extendedPriceAmount?: number;
  totalPriceAmount?: number;
}

export interface ReturnPageItem {
  id?: string;
  branchName?: string;
  customerName?: string;
  returnedByName?: string;
  returnedOn?: Date;
  reasonName?: string;
  remarks?: string;
  totalAmount?: number;
}

export interface ReturnByCustomerPageItem {
  id?: string;
  branchName?: string;
  customerName?: string;
  returnedByName?: string;
  returnedOn?: Date;
  reasonName?: string;
  remarks?: string;
  totalAmount?: number;
}

export interface ReturnByProductPageItem {
  id?: string;
  productName?: string;
  quantityValue?: number;
  totalAmount?: number;
}

export interface ReturnItemPageItem {
  id?: string;
  returnId?: string;
  productName?: string;
  returnReasonName?: string;
  quantityValue?: string;
  unitPriceAmount?: string;
  extendedPriceAmount?: string;
  totalPriceAmount?: string;
}