import { PagerResponse } from './paging';
import { Address } from './Address';
import { Lookup } from '../custom_types/lookup';
import { Measure } from "./measure";
import { StageDefinition } from './stage-definition';
import { UnitOfMeasure } from "./unit-of-measure";

export const orderEvents = {
  pricingChanged: 'pricing-changed',
  item: {
    add: 'order-item-add',
    added: 'order-item-added',
    deleted: 'order-item-deleted',
  },
  payment: {
    add: 'order-payment-add',
    added: 'order-payment-added',
    deleted: 'order-payment-deleted',
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
  created = 1,
  invoiced = 2,
  staged = 3,
  routed = 4,
  shipped = 5,
  completed = 6,
  cancelled = 7
}

export enum OrderAggregate {
  items = 1,
  payments = 2,
  returns = 3
}

export type Order = {
  id?: string;
  orderNumber?: string;
  invoiceNumber?: string;
  branch?: Lookup<string>;
  customer?: Lookup<string>;
  pricing?: Lookup<string>;
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
  recreatedOn?: Date;
  recreatedBy?: Lookup<string>;
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
  paymentOn?: Date;
  paymentBy?: Lookup<string>;
  completedOn?: Date;
  completedBy?: Lookup<string>;
  cancelledOn?: Date;
  cancelledBy?: Lookup<string>;
  cancellationReason?: string;
  items?: OrderItem[];
  returns?: OrderReturn[];
  payments?: OrderPayment[];
  returnables?: OrderReturnable[];
  stage?: StageDefinition<OrderStatus, OrderAggregate>;
}

export type OrderPageItem = {
  id?: string;
  status?: string;
  createdBy?: string;
  customer?: string;
  orderedOn?: Date;
  paymentOn?: Date;
  taxAmount?: number;
  shippingFeeAmount?: number;
  subTotalAmount?: number;
  totalAmount?: number
}

export type OrderItem = {
  id?: string;
  product?: Lookup<string>;
  unitOfMeasures?: UnitOfMeasure[];
  quantity?: Measure;
  standard?: Measure;
  discountRate?: number;
  discountAmount?: number;
  unitPriceAmount?: number;
  extendedPriceAmount?: number;
  totalPriceAmount?: number;
}

export type OrderReturn = {
  id?: string;
  product?: Lookup<string>;
  reason?: Lookup<string>;
  returnedOn?: Date;
  returnedBy?: Lookup<string>;
  quantity?: Measure;
  standard?: Measure;
  returnedAmount?: number;
}

export type OrderReturnable = {
  orderId?: string;
  product?: Lookup<string>;
  discountRate?: number;
  discountAmount?: number;
  unitPriceAmount?: number;
  extendedPriceAmount?: number;
  totalPriceAmount?: number;
  orderedQuantity?: number;
  returnedQuantity?: number;
  returnableQuantity?: number;
  returning: OrderReturning;
}

export type OrderReturning = {
  reason?: Lookup<string>;
  returnedOn?: Date;
  returnedBy?: Lookup<string>;
  quantity?: Measure;
  standard?: Measure;
  amount?: number;
}

export type OrderPayment = {
  id?: string;
  paymentOn?: Date;
  paymentBy?: Lookup<string>;
  paymentType?: Lookup<string>;
  paymentAmount?: number;
  balanceAmount?: number;
}

export type OrderPayable = {
  id?: string;
  orderId?: string;
  paymentOn?: Date;
  paymentBy?: Lookup<string>;
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

export type OrderInvoiceDetail = {
  customerName?: string;
  orderNumber?: string;
  invoiceNumber?: string;
  invoicedOn?: Date;
  invoicedByName?: string;
  pricingName?: string;
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

export type OrderInvoiceDetailItem = {
  id?: string;
  orderId?: string;
  product?: Lookup<string>;
  quantity?: Measure;
  discountRate?: number;
  discountAmount?: number;
  unitPriceAmount?: number;
  extendedPriceAmount?: number;
  totalPriceAmount?: number;
}

export type OrderReportPageItem = {
  id?: string;
  branchName?: string;
  customerName?: string;
  invoiceNumber?: string
  pricingName?: string;
  orderedOn?: Date;
  orderedByName?: string;
  status?: OrderStatus;
  totalAmount?: number;
  paidAmount?: number;
  balanceAmount?: number;
}
