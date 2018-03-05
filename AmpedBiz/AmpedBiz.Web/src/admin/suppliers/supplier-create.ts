import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { Supplier } from '../../common/models/supplier';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';

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

  public async activate(supplier: Supplier): Promise<void> {
    try {
      if (supplier) {
        this.header = "Edit Supplier";
        this.isEdit = true;
        this.supplier = await this._api.suppliers.get(supplier.id);
      }
      else {
        this.header = "Create Supplier";
        this.isEdit = false;
        this.supplier = <Supplier>{};
      }
    } catch (error) {
      this._notification.warning(error);
    }
  }

  public cancel(): void {
    this._controller.cancel();
  }

  public async save(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to save?').whenClosed();
      if (result.output === ActionResult.Yes) {
        let data = (this.isEdit)
          ? await this._api.suppliers.update(this.supplier)
          : await this._api.suppliers.create(this.supplier);
        await this._notification.success("Supplier has been saved.").whenClosed();
        this._controller.ok(<Supplier>data);
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }
}