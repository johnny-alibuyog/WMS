import { Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { Customer } from '../common/models/customer';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';

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

  public activate(customer: Customer): void {
    if (customer && customer.id) {
      this.header = "Edit Customer";
      this.isEdit = true;
      this._api.customers.get(customer.id)
        .then(data => this.customer = <Customer>data)
        .catch(error => this._notification.warning(error));
    }
    else {
      this.header = "Create Customer";
      this.isEdit = false;
      this.customer = <Customer>{};
    }
  }

  public back(): void {
    return this._router.navigateBack();
  }

  public save(): void {
    if (this.isEdit) {
      this._api.customers.update(this.customer)
        .then(data => this.resetAndNoify(data, "Customer has been saved."))
        .catch(error => this._notification.warning(error));
    }
    else {
      this._api.customers.create(this.customer)
        .then(data => this.resetAndNoify(data, "Customer has been saved."))
        .catch(error => this._notification.warning(error));
    }
  }

  private resetAndNoify(customer: Customer, notificationMessage: string) {
    this.customer = customer;

    if (notificationMessage) {
      this._notification.success(notificationMessage);
    }
  }
}