import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {PaymentTypeCreate} from './payment-type-create';
import {PaymentType} from './common/models/payment-type';
import {PaymentTypeService} from '../services/payment-type-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class PaymentTypeList {
  private _notification: NotificationService;
  private _service: PaymentTypeService;
  private _dialog: DialogService;

  public paymentTypes: PaymentType[];
  public filterText: string = '';

  constructor(dialog: DialogService, service: PaymentTypeService, notification: NotificationService) {
    this._dialog = dialog;
    this._service = service;
    this._notification = notification;
  }

  activate() {
    this.refreshList();
  }

  refreshList() {
    this.filterText = '';
    this.filter();
  }

  filter() {
    this._service.getLists(this.filterText)
      .then(data => {
        this.paymentTypes = <PaymentType[]>data
        if (!this.paymentTypes || this.paymentTypes.length == 0) {
          this._notification.warning("No items found!");
        }
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  create() {
    this._dialog
      .open({ viewModel: PaymentTypeCreate, model: null })
      .then(response => {
        if (!response.wasCancelled) {
          this.refreshList();
        }
      });
  }

  edit(item: PaymentType) {
    this._dialog
      .open({ viewModel: PaymentTypeCreate, model: item })
      .then(response => {
        if (!response.wasCancelled) {
          this.refreshList();
        }
      });
  }

  delete(item: any) {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}