import { Address } from './Address';
import { Dictionary } from '../custom_types/dictionary';
import { Lookup } from '../custom_types/lookup';
import { Measure } from "./measure";
import { ProductInventory } from './product';
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
  new = 1,
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

export interface Order {
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
  returnables?: OrderReturnable[];
  stage?: StageDefinition<OrderStatus, OrderAggregate>;
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
  unitOfMeasures?: UnitOfMeasure[];
  quantity?: Measure;
  standard?: Measure;
  discountRate?: number;
  discountAmount?: number;
  unitPriceAmount?: number;
  extendedPriceAmount?: number;
  totalPriceAmount?: number;
}

export interface OrderReturn {
  id?: string;
  product?: Lookup<string>;
  reason?: Lookup<string>;
  returnedOn?: Date;
  returnedBy?: Lookup<string>;
  quantity?: Measure;
  returnedAmount?: number;
}

export interface OrderReturnable {
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

export interface OrderReturning {
  reason?: Lookup<string>;
  returnedOn?: Date;
  returnedBy?: Lookup<string>;
  quantity?: Measure;
  amount?: number;
}

export interface OrderPayment {
  id?: string;
  paidOn?: Date;
  paidTo?: Lookup<string>;
  paymentType?: Lookup<string>;
  paymentAmount?: number;
  balanceAmount?: number;
}

export interface OrderPayable {
  id?: string;
  orderId?: string;
  paidOn?: Date;
  paidTo?: Lookup<string>;
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

export class OrderInvoiceDetailItem {
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

export class OrderReportPageItem {
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

export interface SalesReportPageItem
{
    productId?: string;
    productName?: string;
    totalSoldItems?: string;
    totalSoldPrice?: string;
    details?: SalesReportPageDetailItem[];
}

export interface SalesReportPageDetailItem
{
    customerName?: string;
    invoiceNumber?: string;
    soldItems?: string;
    soldPrice?: string;
}