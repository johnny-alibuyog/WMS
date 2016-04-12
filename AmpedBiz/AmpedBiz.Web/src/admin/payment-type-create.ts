import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {PaymentType} from './common/models/payment-type';
import {PaymentTypeService} from '../services/payment-type-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class PaymentTypeCreate {
  private _controller: DialogController;
  private _service: PaymentTypeService;

  public header: string = 'Create Type Product';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public paymentType: PaymentType;
  public notificaton: NotificationService;

  constructor(notification: NotificationService, controller: DialogController, service: PaymentTypeService) {
    this.notificaton = notification;
    this._controller = controller;
    this._service = service;
  }

  activate(paymentType: PaymentType) {
    if (paymentType) {
      this.header = "Edit Product";
      this.isEdit = true;
      this._service.getPaymentType(paymentType.id)
        .then(data => this.paymentType = <PaymentType>data)
        .catch(error => this.notificaton.warning(error));
    }
    else {
      this.header = "Create Product";
      this.isEdit = false;
      this.paymentType = <PaymentType>{};
    }
  }

  cancel() {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {

    if (this.isEdit) {

      this._service.updatePaymentType(this.paymentType)
        .then(data => {
          this.notificaton.success("Product Type has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <PaymentType>data }));
        })
        .catch(error => {
          this.notificaton.warning(error)
        });
    }
    else {

      this._service.createPaymentType(this.paymentType)
        .then(data => {
          this.notificaton.success("Product Type has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <PaymentType>data }));
        })
        .catch(error => {
          this.notificaton.warning(error)
        });
    }
  }
}