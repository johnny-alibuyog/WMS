import { Address } from './address';
import { Contact } from './contact'

export type Customer = {
  id?: string;
  code?: string;
  name?: string;
  contactPerson?: string;
  contact?: Contact;
  billingAddress?: Address;
  officeAddress?: Address;
}

export type CustomerPageItem = {
  id?: string;
  code?: string;
  name?: string;
  description?: string;
  address?: Address;
}

export type CustomerReportPageItem = {
  id?: string;
  code?: string;
  name?: string;
  contactPerson?: string;
  creditLimitAmount?: number;
  contact?: Contact;
  officeAddress?: Address;
  billingAddress?: Address;
}

export type CustomerSalesReportPageItem = {
  paymentOn?: Date;
  branchName?: string;
  customerName?: string;
  invoiceNumber?: string;
  status?: string;
  totalAmount?: number;
  paidAmount?: number;
  balanceAmount?: number;
}

export type CustomerPaymentsReportPageItem = {
  paymentOn?: Date;
  invoiceNumber?: string;
  branchName?: string;
  customerName?: string;
  paymentTypeName?: string;
  totalAmount?: number;
  paidAmount?: number;
  balanceAmount?: number;
}

export type CustomerOrderDeliveryReportPageItem = {
  id?: string;
  shippedOn?: Date;
  branchName?: string;
  customerName?: string;
  invoiceNumber?: string
  pricingName?: string;
  discountAmount?: number;
  totalAmount?: number;
  subTotalAmount?: number;
}
