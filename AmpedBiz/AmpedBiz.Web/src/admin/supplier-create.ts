import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {Supplier} from './common/models/supplier';
import {SupplierService} from '../services/supplier-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class SupplierCreate {
  private _controller: DialogController;
  private _service: SupplierService;

  public header: string = 'Create Supplier';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public supplier: Supplier;
  public notificaton: NotificationService;

  constructor(notification: NotificationService, controller: DialogController, service: SupplierService) {
    this.notificaton = notification;
    this._controller = controller;
    this._service = service;
  }

  activate(supplier: Supplier) {
    if (supplier) {
      this.header = "Edit Supplier";
      this.isEdit = true;
      this._service.getSupplier(supplier.id, {
        success: (data) => {
          this.supplier = <Supplier>data;
        },
        error: (error) => {
          this.notificaton.warning(error);
        }
      });
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

      this._service.updateSupplier(this.supplier, {
        success: (data) => {
          this.notificaton.success("Supplier has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <Supplier>data }));
        },
        error: (error) => {
          this.notificaton.warning(error)
        }
      })
    }
    else {

      this._service.createSupplier(this.supplier, {
        success: (data) => {
          this.notificaton.success("Supplier has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <Supplier>data }));
        },
        error: (error) => {
          this.notificaton.warning(error)
        }
      })

    }
  }
}