import {Lookup} from '../custom_types/lookup';
import {ProductInventory} from './product';
import {Dictionary} from '../custom_types/dictionary';

export const purchaseOrderEvents = {
  item: {
    add: 'purchase-order-item-add',
    added: 'purchase-order-item-added',
    deleted: 'purchase-order-item-deleted',
  },
  payment: {
    pay: 'purchase-order-pay',
    paid: 'purchase-order-paid',
  },
  receipts: {
    receive: 'purchase-order-receive',
    received: 'purchase-order-received',
  }
}

export enum PurchaseOrderStatus {
  new = 1,
  submitted = 2,
  approved = 3,
  paid = 4,
  received = 5,
  completed = 6,
  cancelled = 7
}

export interface PurchaseOrder {
  id?: string;
  user?: Lookup<string>;
  supplier?: Lookup<string>;
  shipper?: Lookup<string>;
  paymentType?: string;
  taxAmount?: number;
  shippingFeeAmount?: number;
  paymentAmount?: number;
  subTotalAmount?: number;
  totalAmount?: number;
  status?: PurchaseOrderStatus;
  expectedOn?: Date;
  createdBy?: Lookup<string>;
  createdOn?: Date;
  submittedBy?: Lookup<string>;
  submittedOn?: Date;
  closedBy?: Lookup<string>;
  closedOn?: Date;
  cancelledBy?: Lookup<string>,
  cancelledOn?: Date,
  cancellationReason?: string,
  items?: PurchaseOrderItem[];
  payments?: PurchaseOrderPayment[];
  receipts?: PurchaseOrderReceipt[];
  receivables?: PurchaseOrderReceivable[];
  allowedTransitions?: Dictionary<string>;
}

export interface PurchaseOrderPageItem {
  id?: string;
  supplier?: string;
  status?: PurchaseOrderStatus;
  createdBy?: string;
  createdOn?: Date;
  submittedBy?: string;
  submittedOn?: Date;
  paidBy?: string;
  paidOn?: Date;
  total?: string;
}

export interface PurchaseOrderItem {
  id?: string;
  product?: Lookup<string>;
  quantityValue?: number;
  unitCostAmount?: number;
}

export interface PurchaseOrderPayment {
  id?: string;
  purchaseOrderId?: string;
  paidBy?: Lookup<string>;
  paidOn?: Date;
  paymentAmount?: number;
  paymentType?: Lookup<string>;
}

export interface PurchaseOrderReceipt {
  id?: string;
  purchaseOrderId?: string;
  batchNumber?: string;
  receivedBy?: Lookup<string>;
  receivedOn?: Date;
  expiresOn?: Date;
  product?: Lookup<string>;
  quantityValue?: number;
}

export interface PurchaseOrderReceivable {
  purchaseOrderId?: string;
  product?: Lookup<string>;
  expiresOn?: Date;
  orderedQuantity?: number;
  receivedQuantity?: number;
  receivableQuantity?: number;
  receivingQuantity?: number;
}

export interface PurchaseOrderPayable {
  id?: string;
  purchaseOrderId?: string;
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