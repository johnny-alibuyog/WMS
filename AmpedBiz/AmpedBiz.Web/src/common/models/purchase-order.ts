import { Lookup } from '../custom_types/lookup';
import { Measure } from "./measure";
import { StageDefinition } from './stage-definition';
import { UnitOfMeasure } from "./unit-of-measure";

export const purchaseOrderEvents = {
  item: {
    add: 'purchase-order-item-add',
    added: 'purchase-order-item-added',
    deleted: 'purchase-order-item-deleted',
  },
  payment: {
    add: 'purchase-order-payment-add',
    added: 'purchase-order-payment-added',
    deleted: 'purchase-order-payment-deleted',
  },
  receipts: {
    add: 'purchase-order-receipt-add',
    added: 'purchase-order-receipt-added',
    deleted: 'purchase-order-receipt-deleted',
    receive: 'purchase-order-receipt-receive',
    received: 'purchase-order-receipt-received',
  },
}

export enum PurchaseOrderStatus {
  created = 1,
  submitted = 2,
  approved = 3,
  completed = 4,
  cancelled = 5
}

export enum PurchaseOrderAggregate {
  items = 1,
  payments = 2,
  receipts = 3
}

export type PurchaseOrder = {
  id?: string;
  referenceNumber?: string;
  voucherNumber?: string;
  branch?: Lookup<string>;
  user?: Lookup<string>;
  supplier?: Lookup<string>;
  shipper?: Lookup<string>;
  paymentType?: string;
  taxAmount?: number;
  shippingFeeAmount?: number;
  discountAmount?: number;
  subTotalAmount?: number;
  totalAmount?: number;
  paymentAmount?: number;
  status?: PurchaseOrderStatus;
  expectedOn?: Date;
  createdBy?: Lookup<string>;
  createdOn?: Date;
  recreatedOn?: Date;
  recreatedBy?: Lookup<string>;
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
  stage?: StageDefinition<PurchaseOrderStatus, PurchaseOrderAggregate>;
}

export type PurchaseOrderPageItem = {
  id?: string;
  supplier?: string;
  status?: PurchaseOrderStatus;
  createdBy?: string;
  createdOn?: Date;
  submittedBy?: string;
  submittedOn?: Date;
  paymentBy?: string;
  paymentOn?: Date;
  total?: string;
}

export type PurchaseOrderItem = {
  id?: string;
  product?: Lookup<string>;
  unitOfMeasures?: UnitOfMeasure[];
  quantity?: Measure;
  standard?: Measure;
  unitCostAmount?: number;
  totalCostAmount?: number;
}

export type PurchaseOrderPayment = {
  id?: string;
  purchaseOrderId?: string;
  paymentBy?: Lookup<string>;
  paymentOn?: Date;
  paymentAmount?: number;
  paymentType?: Lookup<string>;
}

export type PurchaseOrderReceipt = {
  id?: string;
  purchaseOrderId?: string;
  batchNumber?: string;
  receivedBy?: Lookup<string>;
  receivedOn?: Date;
  expiresOn?: Date;
  product?: Lookup<string>;
  quantity?: Measure;
  standard?: Measure;
}

export type PurchaseOrderReceivable = {
  purchaseOrderId?: string;
  product?: Lookup<string>;
  unitOfMeasures?: UnitOfMeasure[];
  orderedQuantity?: number;
  receivedQuantity?: number;
  receivableQuantity?: number;
  standard?: Measure;
  receiving: PurchaseOrderReceiving;
}

export type PurchaseOrderReceiving = {
  batchNumber?: string;
  receivedBy?: Lookup<string>;
  receivedOn?: Date;
  expiresOn?: Date;
  quantity?: Measure;
  standard?: Measure;
}

export type PurchaseOrderPayable = {
  id?: string;
  purchaseOrderId?: string;
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

export type Voucher = {
  id?: string;
  supplierName?: string;
  referenceNumber?: string;
  voucherNumber?: string;
  approvedByName?: string;
  approvedOn?: Date;
  paymentTypeName?: string;
  taxAmount?: number;
  shippingFeeAmount?: number;
  subTotalAmount?: number;
  totalAmount?: number;
  items: VoucherItem[];
}

export type VoucherItem = {
  id?: string;
  product?: Lookup<string>;
  quantity?: Measure;
  unitCostAmount?: number;
  totalCostAmount?: number;
}

export type PurchaseOrderReportPageItem = {
  id?: string;
  createdOn?: Date;
  branchName?: string;
  supplierName?: string;
  voucherNumber?: string;
  createdByName?: string;
  approvedByName?: string;
  status?: PurchaseOrderStatus;
  totalAmount?: number;
  paidAmount?: number;
  balanceAmount?: number;
}
