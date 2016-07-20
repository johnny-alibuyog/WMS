import {ProductInventory} from './product';
import {Lookup} from '../custom_types/lookup';
import {Dictionary} from '../custom_types/dictionary';
import {PurchaseOrder, PurchaseOrderStatus, PurchaseOrderItem, PurchaseOrderPayment, PurchaseOrderReceipt} from './purchase-order';


export interface PurchaseOrderEvent {
  id?: string;
  purchaseOrderId?: string;
  transitionDescription?: string;
}

export interface PurchaseOrderNewlyCreatedEvent extends PurchaseOrderEvent {
  createdBy?: Lookup<string>;
  createdOn?: Date;
  expectedOn?: Date;
  paymentType?: Lookup<string>;
  shipper?: Lookup<string>;
  shippingFeeAmount?: number;
  taxAmount?: number;
  supplier?: Lookup<string>;
  items?: PurchaseOrderItem[];
}

export interface PurchaseOrderSubmittedEvent extends PurchaseOrderEvent {
  submittedBy?: Lookup<string>;
  submittedOn?: Date;
}

export interface PurchaseOrderApprovedEvent extends PurchaseOrderEvent {
  approvedBy?: Lookup<string>;
  approvedOn?: Date;
}

export interface PurchaseOrderPaidEvent extends PurchaseOrderEvent {
  payments?: PurchaseOrderPayment[],
}

export interface PurchaseOrderReceivedEvent extends PurchaseOrderEvent {
  receipts?: PurchaseOrderReceipt[];
}

export interface PurchaseOrderCompletedEvent extends PurchaseOrderEvent {
  completedBy?: Lookup<string>;
  completedOn?: Date;
}

export interface PurchaseOrderCancelledEvent extends PurchaseOrderEvent {
  cancelledBy?: Lookup<string>;
  cancelledOn?: Date;
  cancellationReason?: string;
}