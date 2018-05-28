import { Address } from './address';
import { Contact } from './contact'

export interface Customer {
  id?: string;
  code?: string;
  name?: string;
  contactPerson?: string;
  contact?: Contact;
  billingAddress?: Address;
  officeAddress?: Address;
}

export interface CustomerPageItem {
  id?: string;
  code?: string;
  name?: string;
  description?: string;
  address?: Address;
}

export interface CustomerReportPageItem {
  id?: string;
  code?: string;
  name?: string;
  contactPerson?: string;
  creditLimitAmount?: number;
  contact?: Contact;
  officeAddress?: Address;
  billingAddress?: Address;
}

export class CustomerSalesReportPageItem
{
  paidOn?: Date;
  branchName?: string;
  customerName?: string;
  invoiceNumber?: string;
  totalAmount?: number;
  paidAmount?: number;
  balanceAmount?: number;
}

export interface CustomerPaymentsReportPageItem {
  paidOn?: Date;
  invoiceNumber?: string;
  branchName?: string;
  customerName?: string;
  paymentTypeName?: string;
  totalAmount?: number;
  paidAmount?: number;
  balanceAmount?: number;
}

export interface CustomerOrderDeliveryReportPageItem {
  id?: string;
  shippedOn?: Date;
  branchName?: string;
  customerName?: string;
  invoiceNumber?: string
  pricingName?: string;
  discountAmount?: number;
  totalAmount?: number;
}
