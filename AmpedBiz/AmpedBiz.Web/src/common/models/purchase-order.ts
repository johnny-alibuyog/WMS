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
  supplierId?: string;
  status?: PurchaseOrderStatus;
  createdBy?: string;
  createdOn?: Date;
  submittedBy?: string;
  submittedOn?: Date;
  closedBy?: string;
  closedOn?: Date;
  purchaseOrderDetails?: PurchaseOrderDetail[];
  recievingDetails?: RecievingDetail[];
  paymentDetail?: PaymentDetail[];
}

export interface PurchaseOrderDetail {
  id?: string;
  productId?: string;
  available?: number;
  quantity?: number;
  unitPrice?: number;
  totalPrice?: number;
}

export interface RecievingDetail {

}

export interface PaymentDetail {

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