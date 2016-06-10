import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';

@autoinject
export class OrderInvoiced {
  invoicedOrders = [];
  dialog: DialogService;

  constructor(dialog: DialogService) {
    this.invoicedOrders = [
      { 
        'orderDate': 'June 10, 2016',
        'employee': 'John Doe',
        'customer': 'SM Market',
        'status': 'Invoiced',
        'paymentDate' : 'June 20, 2016',
        'taxes': '',
        total : 700
    },
    ];

    this.dialog = dialog;
  }
}