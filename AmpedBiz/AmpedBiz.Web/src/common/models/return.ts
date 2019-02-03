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

export type Return = {
  id?: string;
  branch?: Lookup<string>;
  customer?: Lookup<string>;
  returnedBy?: Lookup<string>;
  returnedOn?: Date;
  remarks?: string;
  totalReturnedAmount?: number;
  items?: ReturnItem[];
}

export type ReturnPageItem = {
  id?: string;
  branchName?: string;
  customerName?: string;
  returnedByName?: string;
  returnedOn?: Date;
  remarks?: string;
  returnedAmount?: number;
}

export type ReturnItem = {
  id?: string;
  returnId?: string;
  product?: Lookup<string>;
  reason?: Lookup<string>;
  unitOfMeasures?: UnitOfMeasure[];
  quantity?: Measure;
  standard?: Measure;
  unitPriceAmount?: number;
  extendedPriceAmount?: number;
  returnedAmount?: number;
}

export type ReturnItemPageItem = {
  id?: string;
  returnId?: string;
  productName?: string;
  reasonName?: string;
  quantityValue?: string;
  unitPriceAmount?: string;
  extendedPriceAmount?: string;
  returnedAmount?: string;
}

export type ReturnsDetailsReportPageItem = {
  id?: string;
  branchName?: string;
  customerName?: string;
  productName?: string;
  reasonName?: string;
  returnedByName?: string;
  returnedOn?: Date;
  quantityValue?: number;
  quantityUnitId?: string;
  returnedAmount?: number;
}

export type ReturnsByCustomerPageItem = {
  id?: string;
  branchName?: string;
  customerName?: string;
  returnedAmount?: number;
}

export type ReturnsByCustomerDetailsPageItem = {
  id?: string;
  branchName?: string;
  customerName?: string;
  returnedOn?: Date;
  returnedAmount?: number;
}

export type ReturnsByProductPageItem = {
  id?: string;
  branchName?: string;
  productName?: string;
  productCode?: string;
  quantityUnit?: string;
  quantityValue?: number;
  returnedAmount?: number;
}

export type ReturnsByProductDetailsPageItem = {
  id?: string;
  branchName?: string;
  productName?: string;
  productCode?: string;
  returnedOn?: Date;
  quantityUnit?: string;
  quantityValue?: number;
  returnedAmount?: number;
}

export type ReturnsByReasonPageItem = {
  id?: string;
  branchName?: string;
  reasonName?: string;
  returnedAmount?: number;
}

export type ReturnsByReasonDetailsPageItem = {
  id?: string;
  branchName?: string;
  reasonName?: string;
  returnedOn?: Date;
  returnedAmount?: number;
}
