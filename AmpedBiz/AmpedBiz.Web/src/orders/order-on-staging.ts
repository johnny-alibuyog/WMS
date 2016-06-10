import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';

@autoinject
export class OrderOnStaging {
  onStagingOrders = [];
  dialog: DialogService;

  constructor(dialog: DialogService) {
    this.onStagingOrders = [
      { 
        'orderDate': 'June 10, 2016',
        'employee': 'John Doe',
        'customer': 'SM Market',
        'status': 'On Staging',
        'paymentDate' : 'June 20, 2016',
        'taxes': '',
        total : 700
    },
    ];

    this.dialog = dialog;
  }
}