import {Lookup} from '../custom_types/lookup';
import {ProductInventory} from './product';
import {Dictionary} from '../custom_types/dictionary';
import {Address} from './Address';

export const orderEvents = {
  pricingScheme: {
    changed: 'order-pricing-scheme-changed',
  },
  item: {
    add: 'order-item-add',
    added: 'order-item-added',
    deleted: 'order-item-deleted',
  },
  payment: {
    pay: 'order-pay',
    paid: 'order-paid',
  },
  return: {
    add: 'order-return-add',
    added: 'order-return-added',
    deleted: 'order-return-deleted',
  },
  invoiceDetail: {
    show: "invoice-detail:show"
  }
}

export enum OrderStatus {
  new = 1,
  invoiced = 2,
  paid = 3,
  staged = 4,
  routed = 5,
  shipped = 6,
  //delivered = 7,
  returned = 7,
  completed = 8,
  cancelled = 9
}

export interface Order {
  id?: string;
  branch?: Lookup<string>;
  customer?: Lookup<string>;
  pricingScheme?: Lookup<string>;
  paymentType?: Lookup<string>;
  shipper?: Lookup<string>;
  shippingAddress?: Address;
  taxRate?: number;
  taxAmount?: number;
  shippingFeeAmount?: number;
  discountAmount?: number;
  subTotalAmount?: number;
  totalAmount?: number;
  status?: OrderStatus;
  orderedOn?: Date;
  orderedBy?: Lookup<string>;
  createdOn?: Date;
  createdBy?: Lookup<string>;
  stagedOn?: Date
  stagedBy?: Lookup<string>;
  shippedOn?: Date;
  shippedBy?: Lookup<string>;
  routedOn?: Date;
  routedBy?: Lookup<string>;
  invoicedOn?: Date;
  invoicedBy?: Lookup<string>;
  paidOn?: Date;
  paidTo?: Lookup<string>;
  completedOn?: Date;
  completedBy?: Lookup<string>;
  cancelledOn?: Date;
  cancelledBy?: Lookup<string>;
  cancellationReason?: string;
  items?: OrderItem[];
  returns?: OrderReturn[];
  payments?: OrderPayment[];
  allowedTransitions?: Dictionary<string>;
}

export interface OrderPageItem {
  id?: string;
  status?: string;
  createdBy?: string;
  customer?: string;
  orderedOn?: Date;
  paidOn?: Date;
  taxAmount?: number;
  shippingFeeAmount?: number;
  subTotalAmount?: number;
  totalAmount?: number
}

export interface OrderItem {
  id?: string;
  product?: Lookup<string>;
  quantityValue?: number;
  discountRate?: number;
  discountAmount?: number;
  unitPriceAmount?: number;
  extendedPriceAmount?: number;
  totalPriceAmount?: number;
}

export interface OrderReturn {
  id?: string;
  product?: Lookup<string>;
  returnedOn?: Date;
  returnedBy?: Lookup<string>;
  quantityValue?: number;
  discountRate?: number;
  discountAmount?: number;
  unitPriceAmount?: number;
  extendedPriceAmount?: number;
  totalPriceAmount?: number;
}

export interface OrderReturnable {
  id?: string;
  product?: Lookup<string>;
  quantityValue?: number;
  discountRate?: number;
  discountAmount?: number;
  unitPriceAmount?: number;
  extendedPriceAmount?: number;
  totalPriceAmount?: number;
}

export interface OrderPayment {
  id?: string;
  paidOn?: Date;
  paidBy?: Lookup<string>;
  paymentType?: Lookup<string>;
  paymentAmount?: number;
}

export interface OrderPayable {
  id?: string;
  orderId?: string;
  paidOn?: Date;
  paidBy?: Lookup<string>;
  paymentType?: Lookup<string>;
  taxAmount?: number;
  shippingFeeAmount?: number;
  discountAmount?: number;
  subTotalAmount?: number;
  totalAmount?: number;
  balanceAmount?: number;
  paidAmount?: number;
  paymentAmount?: number;
}


export interface OrderInvoiceDetail {
  customerName?: string;
  invoiceNumber?: string;
  invoicedOn?: Date;
  invoicedByName?: string;
  pricingSchemeName?: string;
  paymentTypeName?: string;

  branchName?: string;
  orderedOn?: Date;
  orderedByName?: string;
  shippedOn?: Date;
  shippedByName?: string;
  shippingAddress?: Address;

  taxAmount?: number;
  shippingFeeAmount?: number;
  discountAmount?: number;
  returnedAmount?: number;
  subTotalAmount?: number;
  totalAmount?: number;

  items?: OrderInvoiceDetailItem[];
}

export class OrderInvoiceDetailItem {
  id?: string;
  orderId?: string;
  product?: Lookup<string>;
  quantityValue?: number;
  discountRate?: number;
  discountAmount?: number;
  unitPriceAmount?: number;
  extendedPriceAmount?: number;
  totalPriceAmount?: number;
}