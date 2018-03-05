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

  public async activate(paymentType: PaymentType): Promise<void> {
    try {
      if (paymentType) {
        this.header = "Edit Payment Type";
        this.isEdit = true;
        this.paymentType = await this._service.get(paymentType.id);
      }
      else {
        this.header = "Create Payment Type";
        this.isEdit = false;
        this.paymentType = <PaymentType>{};
      }
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
          ? await this._service.update(this.paymentType)
          : await this._service.create(this.paymentType);
        await this._notification.success("Payment Type has been saved.").whenClosed();
        this._controller.ok(<PaymentType>data)
      }
    }
    catch (error) {
      this._notification.warning(error)
    }
  }
}