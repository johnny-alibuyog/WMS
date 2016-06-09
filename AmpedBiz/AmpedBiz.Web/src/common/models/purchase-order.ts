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
  orderDate?: string;
  creationDate?: string;
  expectedDate?: string;
  approvedDate?: string;
  rejectedDate?: string;
  paymentDate?: string;
  submittedDate?: string;
  closedDate?: string;
  paymentTypeName?: string;
  tax?: string;
  shippingFee?: string;
  payment?: string;
  subTotal?: string;
  total?: string;
  statusName?: string;
  createdByEmployeeName?: string;
  submittedByEmployeeName?: string;
  approvedByEmployeeName?: string;
  rejectedByEmployeeName?: string;
  completedByEmployeeName?: string;
  supplierName?: string;
  reason?: string;
}