import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {POCreate} from './po-create';

@autoinject
export class POActiveList {
  activePurchases = [];
  dialog: DialogService;

  constructor(dialog: DialogService) {
    this.activePurchases = [
      { 'code': 'YYY', 'creationDate': 'March 15, 2016', 'user': 'J.Co', 'status': 'pending', 'supplier' : 'San Miguel', 'paymentDate' : 'March 20, 2016', total : 700 },
    ];

    this.dialog = dialog;
  }

  create() {
    this.dialog
      .open({ viewModel: POCreate, model: null })
      .then(response => {
        if (!response.wasCancelled) {
          //this.insert(response.output.output);
        }
      });
  }
}