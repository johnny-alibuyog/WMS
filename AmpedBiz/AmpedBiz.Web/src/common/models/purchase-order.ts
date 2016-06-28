import {ProductInventory} from 'product';
//import {autoinject, BindingEngine} from 'aurelia-framework';

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
  status?: PurchaseOrderStatus;
  expectedOn?: Date;
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
  quantityValue?: number;
  unitPriceAmount?: number;
}

export interface RecievingDetail {

}

export interface PaymentDetail {

}

/*
@autoinject
export class DetailsFactory {
  private _bindingEngine: BindingEngine;

  constructor(bindingEngine: BindingEngine) {
    this._bindingEngine = bindingEngine;
  }

  create(type: string, instance?: any): any {

    switch (type) {

      case 'PurchaseOrderDetail':
        var item = <PurchaseOrderDetail>{
          available: 0,
          quantity: 0,
          price: 0,
          discount: 0,
        };

        var computeTotal = () => item.quantity * (item.price - item.discount);

        this._bindingEngine.propertyObserver(item, 'quantity')
          .subscribe((newValue: any, oldValue: any) => item.totalPrice = computeTotal());

        this._bindingEngine.propertyObserver(item, 'price')
          .subscribe((newValue: any, oldValue: any) => item.totalPrice = computeTotal());

        this._bindingEngine.propertyObserver(item, 'discount')
          .subscribe((newValue: any, oldValue: any) => item.totalPrice = computeTotal());

        return item;

      case 'RecievingDetail':
        return null;

      case 'PaymentDetail':
        return null;

      default:
        throw new Error('Type ${type} is not defined');
    }
  }
}
*/

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
