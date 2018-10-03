import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';
import { ReturnReason } from '../../common/models/return-reason';

@autoinject
export class ReturnReasonCreate {

  public header: string = 'Create Return Reason';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public returnReason: ReturnReason;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _controller: DialogController,
    private readonly _notification: NotificationService
  ) { }

  public async activate(returnReason: ReturnReason): Promise<void> {
    try {
      if (returnReason) {
        this.header = "Edit Return Reason";
        this.isEdit = true;
        this.returnReason = await this._api.returnReasons.get(returnReason.id);
      }
      else {
        this.header = "Create Return Reason";
        this.isEdit = false;
        this.returnReason = <ReturnReason>{};
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
          ? await this._api.returnReasons.update(this.returnReason)
          : await this._api.returnReasons.create(this.returnReason);
        await this._notification.success("Return Reason has been saved.").whenClosed();
        await this._controller.ok(<ReturnReason>data);
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }
}