import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {SupplierCreate} from './supplier-create';
import {Supplier} from './common/models/supplier';
import {SupplierService} from '../services/supplier-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class SupplierList {
  private _notification: NotificationService;
  private _service: SupplierService;
  private _dialog: DialogService;

  public suppliers: Supplier[];
  public filterText: string = '';

  constructor(dialog: DialogService, service: SupplierService, notification: NotificationService) {
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
        this.suppliers = <Supplier[]>data
        if (!this.suppliers || this.suppliers.length == 0) {
          this._notification.warning("No items found!");
        }
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  create() {
    this._dialog
      .open({ viewModel: SupplierCreate, model: null })
      .then(response => {
        if (!response.wasCancelled) {
          this.refreshList();
        }
      });
  }

  edit(item: Supplier) {
    this._dialog
      .open({ viewModel: SupplierCreate, model: item })
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