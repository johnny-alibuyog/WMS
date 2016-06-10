import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';

@autoinject
export class OrderOnRoute {
  onRouteOrders = [];
  dialog: DialogService;

  constructor(dialog: DialogService) {
    this.onRouteOrders = [
      { 
        'orderDate': 'June 10, 2016',
        'employee': 'John Doe',
        'customer': 'SM Market',
        'status': 'On Route',
        'paymentDate' : 'June 20, 2016',
        'taxes': '',
        total : 700
    },
    ];

    this.dialog = dialog;
  }
}