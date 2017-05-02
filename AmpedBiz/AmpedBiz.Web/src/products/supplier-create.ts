import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { Supplier } from '../common/models/supplier';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';

@autoinject
export class SupplierCreate {
  private _api: ServiceApi;
  private _controller: DialogController;
  private _notification: NotificationService;

  public header: string = 'Create Product Category';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public supplier: Supplier;

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;
  }

  activate(supplier: Supplier) {
    if (supplier) {
      this.header = "Edit Supplier";
      this.isEdit = true;
      this._api.suppliers.get(supplier.id)
        .then(data => this.supplier = <Supplier>data)
        .catch(error => this._notification.warning(error));
    }
    else {
      this.header = "Create Supplier";
      this.isEdit = false;
      this.supplier = <Supplier>{};
    }
  }

  cancel() {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {

    if (this.isEdit) {

      this._api.suppliers.update(this.supplier)
        .then(data => {
          this._notification.success("Supplier has been saved.")
            .whenClosed((data) => this._controller.ok({ wasCancelled: true, output: <Supplier>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
    else {

      this._api.suppliers.create(this.supplier)
        .then(data => {
          this._notification.success("Supplier has been saved.")
            .whenClosed((data) => this._controller.ok({ wasCancelled: true, output: <Supplier>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
  }
}