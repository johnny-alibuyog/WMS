import { autoinject } from 'aurelia-framework';
import { PaymentType } from '../../common/models/payment-type';
import { PaymentTypeService } from '../../services/payment-type-service';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';
import { Router } from 'aurelia-router';

@autoinject
export class PaymentTypeCreate {

  public header: string = 'Create Payment Type';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public paymentType: PaymentType;

  constructor(
    private readonly _router: Router,
    private readonly _service: PaymentTypeService,
    private readonly _notification: NotificationService,
  ) { }

  public async activate(paymentType: PaymentType): Promise<void> {
    try {
      this.isEdit = (paymentType && paymentType.id) ? true : false;
      this.header = (this.isEdit) ? "Edit Payment Type" : "Create Payment Type";
      this.paymentType = (this.isEdit) ? await this._service.get(paymentType.id) : <PaymentType>{};
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
        this.paymentType = (this.isEdit)
          ? await this._service.update(this.paymentType)
          : await this._service.create(this.paymentType);
        await this._notification.success("Payment Type has been saved.").whenClosed();
      }
    }
    catch (error) {
      this._notification.warning(error)
    }
  }
}
