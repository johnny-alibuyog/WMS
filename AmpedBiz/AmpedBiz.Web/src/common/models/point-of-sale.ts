import { Lookup } from './../custom_types/lookup';
import { Measure } from './measure';
import { UnitOfMeasure } from './unit-of-measure';

export const pointOfSaleEvents = {
  item: {
    add: 'point-of-sale-item-add',
    added: 'point-of-sale-item-added',
    deleted: 'point-of-sale-item-deleted',
  },
  payment: {
    add: 'point-of-sale-payment-add',
    added: 'point-of-sale-payment-added',
    deleted: 'point-of-sale-payment-deleted',
  },
};

export interface PointOfSale {
  id?: string;
  invoiceNumber?: string;
  branch?: Lookup<string>;
  customer?: Lookup<string>;
  pricing?: Lookup<string>;
  paymentType?: Lookup<string>;
  tenderedBy?: Lookup<string>;
  tenderedOn?: Date;
  discountAmount?: number;
  subTotalAmount?: number;
  totalAmount?: number;
  paidAmount?: number;
  createdOn?: Date;
  modifiedOn?: Date;
  createdBy?: Lookup<string>;
  modifiedBy?: Lookup<string>;
  items?: PointOfSaleItem[];
  payments?: PointOfSalePayment[];
}

export interface PointOfSaleItem {
  id?: string;
  barcode?: string;
  pointOfSaleId?: string;
  unitOfMeasures?: UnitOfMeasure[];
  product?:  Lookup<string>;
  quantity?: Measure;
  standard?: Measure;
  discountRate?: number;
  discountAmount?: number;
  unitPriceAmount?: number;
  extendedPriceAmount?: number;
  totalPriceAmount?: number;
}

export interface PointOfSalePayment {
  id?: string;
  pointOfSaleId?: string;
  paymentType?: Lookup<string>;
  paymentAmount?: number;
  balanceAmount?: number;
}

export interface PointOfSalePageItem
{
  id?: string;
  invoiceNumber?: string;
  tenderedByName?: string;
  tenderedOn?: Date;
  customerName?: string;
  discountAmount?: string;
  subTotalAmount?: string;
  totalAmount?: string;
}
