import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {OrderCreate} from './order-create';

@autoinject
export class OrderActive {
  activeOrders = [];
  dialog: DialogService;

  constructor(dialog: DialogService) {
    this.activeOrders = [
      { 
        'orderDate': 'June 10, 2016',
        'employee': 'John Doe',
        'customer': 'SM Market',
        'status': 'New',
        'paymentDate' : 'June 20, 2016',
        'taxes': '',
        total : 700
    },
    ];

    this.dialog = dialog;
  }

  create() {
    this.dialog
      .open({ viewModel: OrderCreate, model: null })
      .then(response => {
        if (!response.wasCancelled) {
          //this.insert(response.output.output);
        }
      });
  }
}