import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { Supplier } from '../../common/models/supplier';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';

@autoinject
export class SupplierCreate {

  public header: string = 'Create Product Category';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public supplier: Supplier;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _controller: DialogController,
    private readonly _notification: NotificationService
  ) { }
  
  public async activate(supplier: Supplier): Promise<void> {
    try {
      this.isEdit = (supplier) ? true : false;
      this.header = (this.isEdit) ? "Edit Supplier" : "Create Supplier";
      this.supplier = (this.isEdit) ? await this._api.suppliers.get(supplier.id) : <Supplier>{};
    } 
    catch (error) {
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
        await this._controller.ok(<Supplier>data);
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }
}