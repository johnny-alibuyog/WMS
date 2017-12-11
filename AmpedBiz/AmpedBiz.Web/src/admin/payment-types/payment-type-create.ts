import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { PaymentType } from '../../common/models/payment-type';
import { PaymentTypeService } from '../../services/payment-type-service';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';

@autoinject
export class PaymentTypeCreate {
  private _notification: NotificationService;
  private _controller: DialogController;
  private _service: PaymentTypeService;

  public header: string = 'Create Payment Type';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public paymentType: PaymentType;

  constructor(notification: NotificationService, controller: DialogController, service: PaymentTypeService) {
    this._notification = notification;
    this._controller = controller;
    this._service = service;
  }

  activate(paymentType: PaymentType) {
    if (paymentType) {
      this.header = "Edit Payment Type";
      this.isEdit = true;
      this._service.get(paymentType.id)
        .then(data => this.paymentType = <PaymentType>data)
        .catch(error => this._notification.warning(error));
    }
    else {
      this.header = "Create Payment Type";
      this.isEdit = false;
      this.paymentType = <PaymentType>{};
    }
  }

  cancel() {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {
    this._notification.confirm('Do you want to save?').whenClosed(result => {
      if (result.output === ActionResult.Yes) {

        if (this.isEdit) {
          this._service.update(this.paymentType)
            .then(data => {
              this._notification.success("Payment Type has been saved.")
                .then((data) => this._controller.ok({ wasCancelled: true, output: <PaymentType>data }));
            })
            .catch(error => {
              this._notification.warning(error)
            });
        }
        else {

          this._service.create(this.paymentType)
            .then(data => {
              this._notification.success("Payment Type has been saved.")
                .then((data) => this._controller.ok({ wasCancelled: true, output: <PaymentType>data }));
            })
            .catch(error => {
              this._notification.warning(error)
            });
        }
      }
    });
  }
}