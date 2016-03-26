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
      this._service.getPaymentType(paymentType.id, {
        success: (data) => {
          this.paymentType = <PaymentType>data
        },
        error: (error) => {
          this.notificaton.warning(error)
        }
      });
    }
    else {
      this.header = "Create Product";
      this.paymentType = <PaymentType>{};
    }
  }

  cancel() {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {
    this._service.createPaymentType(this.paymentType, {
      success: (data) => {
        this.notificaton.success("Product Type has been saved.")
          .then((data) => this._controller.ok({ wasCancelled: true, output: <PaymentType>data }));
      },
      error: (error) => {
        this.notificaton.warning(error)
      }
    })
  }
}