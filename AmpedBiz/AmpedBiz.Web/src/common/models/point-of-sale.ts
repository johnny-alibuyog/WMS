import { ServiceApi } from './../../services/service-api';
import { Lookup } from './../custom_types/lookup';
import { Measure } from './measure';
import { UnitOfMeasure } from './unit-of-measure';
import { pricing } from './pricing';

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
  // total: {
  //   changed: 'point-of-sale-total-changed',
  // },
  saved: 'point-of-sale-saved'
};

export enum PointOfSaleStatus {
  unPaid = 1,
  partiallyPaid = 2,
  fullyPaid = 3,
}

export type PointOfSale = {
  id?: string;
  invoiceNumber?: string;
  branch?: Lookup<string>;
  customer?: Lookup<string>;
  pricing?: Lookup<string>;
  paymentType?: Lookup<string>;
  tenderedBy?: Lookup<string>;
  tenderedOn?: Date;
  discountRate?: number;
  discountAmount?: number;
  subTotalAmount?: number;
  totalAmount?: number;
  receivedAmount?: number;
  changeAmount?: number;
  paidAmount?: number;
  balanceAmount?: number;
  createdOn?: Date;
  modifiedOn?: Date;
  createdBy?: Lookup<string>;
  modifiedBy?: Lookup<string>;
  status?: PointOfSaleStatus;
  items?: PointOfSaleItem[];
  payments?: PointOfSalePayment[];
}

export type PointOfSaleItem = {
  id?: string;
  barcode?: string;
  pointOfSaleId?: string;
  unitOfMeasures?: UnitOfMeasure[];
  product?: Lookup<string>;
  quantity?: Measure;
  standard?: Measure;
  status?: PointOfSaleStatus;
  discountRate?: number;
  discountAmount?: number;
  unitPriceAmount?: number;
  extendedPriceAmount?: number;
  totalPriceAmount?: number;
  focus?: boolean;
}

export type PointOfSalePayment = {
  id?: string;
  pointOfSaleId?: string;
  paidOn?: Date;
  paidTo?: Lookup<string>;
  paymentType?: Lookup<string>;
  paymentAmount?: number;
  balanceAmount?: number;
  focus?: boolean;
}

export type PointOfSalePayable = {
  pointOfSaleId?: string;
  paymentType?: Lookup<string>;
  discountRate?: number;
  discountAmount?: number;
  subTotalAmount?: number;
  totalAmount?: number;
}

export type PointOfSalePageItem = {
  id?: string;
  invoiceNumber?: string;
  tenderedByName?: string;
  tenderedOn?: Date;
  customerName?: string;
  discountAmount?: string;
  subTotalAmount?: string;
  totalAmount?: string;
}

export type PointOfSaleDetail = {
  branchName?: string;
  customerName?: string;
  invoiceNumber?: string;
  tenderedOn?: Date;
  tenderedByName?: string;
  pricingName?: string;
  paymentTypeName?: string;

  subTotalAmount?: number;
  discountAmount?: number;
  totalAmount?: number;
  receivedAmount?: number;
  changeAmount?: number;
  paidAmount?: number;
  balanceAmount?: number;
  status?: PointOfSaleStatus;

  items?: PointOfSaleItem[];
}

export const initialPointOfSale =
  (api: ServiceApi) => <PointOfSale>{
    id: null,
    branch: api.auth.userBranchAsLookup,
    tenderedBy: api.auth.userAsLookup,
    tenderedOn: new Date(),
    discountRate: null,
    pricing: pricing.retailPrice,
    status: PointOfSaleStatus.unPaid,
    items: [],
    payments: []
  }

  export const initializePointOfSalePayable =
  (instance: PointOfSalePayable) => {
    if (!instance){
      instance = {};
    }

    if (!instance.totalAmount){
      instance.totalAmount = 0;
    }

    return instance;
  }
