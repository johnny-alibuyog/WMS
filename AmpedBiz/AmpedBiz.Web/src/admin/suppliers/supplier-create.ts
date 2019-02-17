import { autoinject } from 'aurelia-framework';
import { Supplier } from '../../common/models/supplier';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';
import { Router } from 'aurelia-router';

@autoinject
export class SupplierCreate {

  public header: string = 'Create Product Category';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public supplier: Supplier;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService
  ) { }
  
  public async activate(supplier: Supplier): Promise<void> {
    try {
      this.isEdit = (supplier && supplier.id) ? true : false;
      this.header = (this.isEdit) ? "Edit Supplier" : "Create Supplier";
      this.supplier = (this.isEdit) ? await this._api.suppliers.get(supplier.id) : <Supplier>{};
    } 
    catch (error) {
      this._notification.warning(error);
    }
  }

  public back(): void {
    return this._router.navigateBack();
  }

  public async save(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to save?').whenClosed();
      if (result.output === ActionResult.Yes) {
        this.supplier = (this.isEdit)
          ? await this._api.suppliers.update(this.supplier)
          : await this._api.suppliers.create(this.supplier);
        await this._notification.success("Supplier has been saved.").whenClosed();
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }
}
