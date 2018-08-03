import { Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { Customer } from '../../common/models/customer';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';

@autoinject
export class CustomerCreate {

  public header: string = 'Create Customer';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public customer: Customer;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService
  ) { }

  public async activate(customer: Customer): Promise<void> {
    try {
      this.isEdit = (customer && customer.id) ? true : false;
      this.header = (this.isEdit) ? "Edit Customer" : "Create Customer";
      this.customer = (this.isEdit) ? await this._api.customers.get(customer.id) : <Customer>{};
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
        this.customer = (this.isEdit)
          ? await this._api.customers.update(this.customer)
          : await this._api.customers.create(this.customer);
        await this._notification.success("Customer has been saved.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }
}