import {ProductInventory} from './product';
import {Lookup} from '../custom_types/lookup';
import {Dictionary} from '../custom_types/dictionary';

export enum PurchaseOrderStatus {
  new = 1,
  submitted = 2,
  approved = 3,
  payed = 4,
  received = 5,
  completed = 6,
  cancelled = 7
}

export interface PurchaseOrder {
  id?: string;
  userId?: string;
  supplierId?: string;
  paymentTypeId?: string;
  taxAmount?: number;
  shippingFeeAmount?: number;
  paymentAmount?: number;
  subTotalAmount?: number;
  totalAmount?: number;
  status?: PurchaseOrderStatus;
  expectedOn?: Date;
  createdBy?: string;
  createdOn?: Date;
  submittedBy?: string;
  submittedOn?: Date;
  closedBy?: string;
  closedOn?: Date;
  cancelledBy?: string,
  cancelledOn?: Date,
  cancellationReason?: string,
  items?: PurchaseOrderItem[];
  payments?: PurchaseOrderPayment[];
  reciepts?: PurchaseOrderReciept[];
  allowedTransitions?: Dictionary<string>;
}

export interface PurchaseOrderItem {
  id?: string;
  product?: Lookup<string>;
  quantityValue?: number;
  unitPriceAmount?: number;
}

export interface PurchaseOrderReciept {

}

export interface PurchaseOrderPayment {

}

export interface PurchaseOrderPageItem {
  id?: string;
  supplier?: string;
  status?: PurchaseOrderStatus;
  createdBy?: string;
  createdOn?: Date;
  submittedBy?: string;
  submittedOn?: Date;
  payedBy?: string;
  payedOn?: Date;
  total?: string;
}
