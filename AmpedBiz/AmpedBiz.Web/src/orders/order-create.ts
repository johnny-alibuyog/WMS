import {Router} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {ServiceApi} from '../services/service-api';
import {Lookup} from '../common/custom_types/lookup';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class OrderCreate {
  private _api: ServiceApi;
  private _router: Router;
  private _dialog: DialogService;
  private _notification: NotificationService;

  public header: string = 'Purchase Order';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public products: Lookup<string>[] = [];
  public suppliers: Lookup<string>[] = [];

  constructor(api: ServiceApi, router: Router, dialog: DialogService, notification: NotificationService) {
    this._api = api;
    this._router = router;
    this._dialog = dialog;
    this._notification = notification;

    this._api.suppliers.getLookups()
      .then(data => this.suppliers = data);
  }
}