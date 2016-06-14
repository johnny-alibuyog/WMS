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
  orderDate?: Date;
  creationDate?: Date;
  expectedDate?: Date;
  approvedDate?: Date;
  rejectedDate?: Date;
  paymentDate?: Date;
  submittedDate?: Date;
  closedDate?: Date;
  paymentTypeId?: string;
  taxAmount?: number;
  shippingFeeAmount?: number;
  paymentAmount?: number;
  subTotalAmount?: number;
  totalAmount?: number;
  status?: number;
  createdByEmployeeId?: string;
  submittedByEmployeeId?: string;
  approvedByEmployeeId?: string;
  rejectedByEmployeeId?: string;
  completedByEmployeeId?: string;
  supplierId?: string;
  reason?: string;
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