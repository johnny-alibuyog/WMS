import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {Branch} from '../common/models/branch';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';



@autoinject
export class BranchCreate {
  private _api: ServiceApi;
  private _controller: DialogController;
  private _notification: NotificationService;

  public header: string = 'Create Branch';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public branch: Branch;

  constructor(api: ServiceApi, controller: DialogController, notification: NotificationService) {
    this._api = api;
    this._controller = controller;
    this._notification = notification;
  }

  activate(branch: Branch) {
    if (branch) {
      this.header = "Edit Branch";
      this.isEdit = true;
      this._api.branches.get(branch.id)
        .then(data => this.branch = <Branch>data)
        .catch(error => this._notification.warning(error));
    }
    else {
      this.header = "Create Branch";
      this.isEdit = false;
      this.branch = <Branch>{};
    }
  }

  cancel() {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {
    if (this.isEdit) {

      this._api.branches.update(this.branch)
        .then(data => {
          this._notification.success("Branch  has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <Branch>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
    else {

      this._api.branches.create(this.branch)
        .then(data => {
          this._notification.success("Branch has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <Branch>data }));
        })
        .catch(error => {
          this._notification.warning(error)
        });
    }
  }
}