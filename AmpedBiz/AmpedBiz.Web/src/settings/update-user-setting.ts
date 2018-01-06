import { Router } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { ActionResult } from '../common/controls/notification';
import { UserSetting } from '../common/models/user-setting';

@autoinject
export class UpdateUserSetting {
  private readonly _api: ServiceApi;
  private readonly _router: Router;
  private readonly _notification: NotificationService;

  public header: string = 'User Setting';
  public canSave: boolean = true;
  public settings: UserSetting;

  constructor(api: ServiceApi, router: Router, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._notification = notification;
  }

  public activate(): void {
    this._api.settings.getUserSetting()
      .then(data => this.settings = data)
      .catch(error => this._notification.warning(error));
  }

  public back(): void {
    return this._router.navigateBack();
  }

  public save(): void {
    this._notification.confirm('Do you want to save?').whenClosed(result => {
      if (result.output === ActionResult.Yes) {

        this._api.settings.updateUserSetting(this.settings)
          .then(data => this.resetAndNoify(data, "Settings has been saved."))
          .catch(error => this._notification.warning(error));
      }
    });
  }

  private resetAndNoify(settings: UserSetting, notificationMessage: string) {
    this.settings = settings;

    if (notificationMessage) {
      this._notification.success(notificationMessage);
    }
  }
}