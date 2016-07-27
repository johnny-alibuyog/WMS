import {Lookup} from '../custom_types/lookup';
import {ProductInventory} from './product';
import {Dictionary} from '../custom_types/dictionary';
import {Address} from './Address';

export enum OrderStatus {
  new = 1,
  staged = 2,
  routed = 3,
  invoiced = 4,
  paid = 5,
  shipped = 6,
  //delivered = 7,
  completed = 7,
  cancelled = 8
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
  payments?: OrderPayment[];
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
  total?: number;
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