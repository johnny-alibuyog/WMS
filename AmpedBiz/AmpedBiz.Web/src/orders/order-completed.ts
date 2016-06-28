import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';

@autoinject
export class OrderCompleted {
  completedOrders = [];
  dialog: DialogService;

  constructor(dialog: DialogService) {
    this.completedOrders = [
      { 
        'orderDate': 'June 10, 2016',
        'user': 'John Doe',
        'customer': 'SM Market',
        'status': 'Completed',
        'paymentDate' : 'June 20, 2016',
        'taxes': '',
        total : 700
    },
    ];

    this.dialog = dialog;
  }
}