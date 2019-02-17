import { autoinject } from 'aurelia-framework';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { ActionResult } from '../../common/controls/notification';
import { ReturnReason } from '../../common/models/return-reason';
import { Router } from 'aurelia-router';

@autoinject
export class ReturnReasonCreate {

  public header: string = 'Create Return Reason';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public returnReason: ReturnReason;

  constructor(
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService
  ) { }

  public async activate(returnReason: ReturnReason): Promise<void> {
    try {
      this.isEdit = (returnReason && returnReason.id) ? true : false;
      this.header = (this.isEdit) ? "Edit Return Reason" : "Create Return Reason";
      this.returnReason = (this.isEdit) ? await this._api.returnReasons.get(returnReason.id) : <ReturnReason>{};
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
        this.returnReason = (this.isEdit)
          ? await this._api.returnReasons.update(this.returnReason)
          : await this._api.returnReasons.create(this.returnReason);
        await this._notification.success("Return Reason has been saved.").whenClosed();
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }
}
