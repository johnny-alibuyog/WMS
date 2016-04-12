import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {CustomerCreate} from './customer-create';
import {Customer} from './common/models/customer';
import {CustomerService} from '../services/customer-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class BranchList {
  private _notification: NotificationService;
  private _service: CustomerService;
  private _dialog: DialogService;

  public customers: Customer[];
  public filterText: string = '';

  constructor(dialog: DialogService, service: CustomerService, notification: NotificationService) {
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
    this._service.getCustomers(this.filterText)
      .then(data => {
        this.customers = <Customer[]>data
        if (!this.customers || this.customers.length == 0) {
          this._notification.warning("No items found!");
        }
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  create() {
    this._dialog
      .open({ viewModel: CustomerCreate, model: null })
      .then(response => {
        if (!response.wasCancelled) {
          this.refreshList();
        }
      });
  }

  edit(item: Customer) {
    this._dialog
      .open({ viewModel: CustomerCreate, model: item })
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