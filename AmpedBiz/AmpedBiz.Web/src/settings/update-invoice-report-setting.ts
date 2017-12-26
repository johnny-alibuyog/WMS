import { Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { InvoiceReportSetting } from '../common/models/invoice-report-setting';
import { ActionResult } from '../common/controls/notification';

@autoinject
export class UpdateInvoiceReportSetting {
  private readonly _api: ServiceApi;
  private readonly _router: Router;
  private readonly _notification: NotificationService;

  public header: string = 'Invoice Report Setting';
  public canSave: boolean = true;
  public settings: InvoiceReportSetting;

  constructor(api: ServiceApi, router: Router, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._notification = notification;
  }

  public activate(): void {
    this._api.settings.getInvoiceReportSetting()
      .then(data => this.settings = <InvoiceReportSetting>data)
      .catch(error => this._notification.warning(error));
  }

  public back(): void {
    return this._router.navigateBack();
  }

  public save(): void {
    this._notification.confirm('Do you want to save?').whenClosed(result => {
      if (result.output === ActionResult.Yes) {

        this._api.settings.updateInvoiceReportSetting(this.settings)
          .then(data => this.resetAndNoify(data, "Settings has been saved."))
          .catch(error => this._notification.warning(error));
      }
    });
  }

  private resetAndNoify(settings: InvoiceReportSetting, notificationMessage: string) {
    this.settings = settings;

    if (notificationMessage) {
      this._notification.success(notificationMessage);
    }
  }
}