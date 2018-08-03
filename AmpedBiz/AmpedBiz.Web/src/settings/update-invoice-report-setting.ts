import { Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { InvoiceReportSetting } from '../common/models/invoice-report-setting';
import { ActionResult } from '../common/controls/notification';

@autoinject
export class UpdateInvoiceReportSetting {

  public header: string = 'Invoice Report Setting';
  public canSave: boolean = true;
  public settings: InvoiceReportSetting;

  constructor(  
    private readonly _api: ServiceApi,
    private readonly _router: Router,
    private readonly _notification: NotificationService,
  ) { }

  public async activate(): Promise<void> {
    try {
      this.settings = await this._api.settings.getInvoiceReportSetting();
    }
    catch (error) {
      this._notification.warning(error);
    }
  }

  public back(): void {
    this._router.navigateBack();
  }

  public async save(): Promise<void> {
    try {
      let result = await this._notification.confirm('Do you want to save?').whenClosed();
      if (result.output === ActionResult.Yes) {
        this.settings = await this._api.settings.updateInvoiceReportSetting(this.settings);
        this._notification.success("Settings has been saved.");
      }
    }
    catch (error) {
      this._notification.warning(error);
    }
  }
}