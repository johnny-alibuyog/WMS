import { Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { Customer } from '../../common/models/customer';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';

@autoinject
export class CustomerCreate {
  private readonly _api: ServiceApi;
  private readonly _router: Router;
  private readonly _notification: NotificationService;

  public header: string = 'Create Customer';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public customer: Customer;

  constructor(api: ServiceApi, router: Router, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._notification = notification;
  }

  public async activate(customer: Customer): Promise<void> {
    try {
      if (customer && customer.id) {
        this.header = "Edit Customer";
        this.isEdit = true;
        this.customer = await this._api.customers.get(customer.id);
      }
      else {
        this.header = "Create Customer";
        this.isEdit = false;
        this.customer = <Customer>{};
      }
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
        let data = (this.isEdit)
          ? await this._api.customers.update(this.customer)
          : await this._api.customers.create(this.customer);
        this.resetAndNoify(data, "Customer has been saved.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  private resetAndNoify(customer: Customer, notificationMessage: string) {
    this.customer = customer;

    if (notificationMessage) {
      this._notification.success(notificationMessage);
    }
  }
}