import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {Customer} from './common/models/customer';
import {CustomerService} from '../services/customer-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class CustomerCreate {
  private _controller: DialogController;
  private _service: CustomerService;

  public header: string = 'Create Customer';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public customer: Customer;
  public notificaton: NotificationService;

  constructor(notification: NotificationService, controller: DialogController, service: CustomerService) {
    this.notificaton = notification;
    this._controller = controller;
    this._service = service;
  }

  activate(customer: Customer) {
    if (customer) {
      this.header = "Edit Customer";
      this.isEdit = true;
      this._service.getCustomer(customer.id, {
        success: (data) => {
          this.customer = <Customer>data;
        },
        error: (error) => {
          this.notificaton.warning(error);
        }
      });
    }
    else {
      this.header = "Create Customer";
      this.isEdit = false;
      this.customer = <Customer>{};
    }
  }

  cancel() {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {
    if (this.isEdit) {

      this._service.updateCustomer(this.customer, {
        success: (data) => {
          this.notificaton.success("Customer  has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <Customer>data }));
        },
        error: (error) => {
          this.notificaton.warning(error)
        }
      })
    }
    else {

      this._service.createCustomer(this.customer, {
        success: (data) => {
          this.notificaton.success("Customer has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <Customer>data }));
        },
        error: (error) => {
          this.notificaton.warning(error)
        }
      })

    }
  }
}