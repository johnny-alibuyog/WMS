import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {Customer} from '../common/models/customer';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class CustomerCreate {
  private readonly _api: ServiceApi;
  private readonly _controller: DialogController;
  private readonly _notification: NotificationService;

  public header: string = 'Create Customer';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public customer: Customer;

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;
  }

  public activate(customer: Customer): void {
    if (customer) {
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

  public cancel(): void {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  public save(): void {
    if (this.isEdit) {

      this._api.customers.update(this.customer)
        .then(data => {
          this._notification.success("Customer has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <Customer>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
    else {

      this._api.customers.create(this.customer)
        .then(data => {
          this._notification.success("Customer has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <Customer>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
  }
}