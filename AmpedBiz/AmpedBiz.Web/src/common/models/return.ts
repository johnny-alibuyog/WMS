import { Address } from './Address';
import { Dictionary } from '../custom_types/dictionary';
import { Lookup } from '../custom_types/lookup';
import { Measure } from "./measure";
import { UnitOfMeasure } from "./unit-of-measure";

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
  remarks?: string;
  totalAmount?: number;
  items?: ReturnItem[];
}

export interface ReturnPageItem {
  id?: string;
  branchName?: string;
  customerName?: string;
  returnedByName?: string;
  returnedOn?: Date;
  remarks?: string;
  totalAmount?: number;
}

export interface ReturnItem {
  id?: string;
  returnId?: string;
  product?: Lookup<string>;
  returnReason?: Lookup<string>;
  unitOfMeasures?: UnitOfMeasure[];
  quantity?: Measure;
  standard?: Measure;
  unitPriceAmount?: number;
  extendedPriceAmount?: number;
  totalPriceAmount?: number;
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

export interface ReturnsByCustomerPageItem {
  id?: string;
  customerName?: string;
  totalAmount?: number;
}

export interface ReturnsByProductPageItem {
  id?: string;
  productName?: string;
  quantityValue?: number;
  totalAmount?: number;
}

export interface ReturnsByReasonPageItem {
  id?: string;
  returnReasonName?: string;
  totalAmount?: number;
}