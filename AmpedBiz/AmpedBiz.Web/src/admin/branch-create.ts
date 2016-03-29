import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {Branch} from './common/models/branch';
import {BranchService} from '../services/branch-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class BranchCreate {
  private _controller: DialogController;
  private _service: BranchService;

  public header: string = 'Create Type Product';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public branch: Branch;
  public notificaton: NotificationService;

  constructor(notification: NotificationService, controller: DialogController, service: BranchService) {
    this.notificaton = notification;
    this._controller = controller;
    this._service = service;
  }

  activate(branch: Branch) {
    if (branch) {
      this.header = "Edit Branch";
      this.isEdit = true;
      this._service.getBranch(branch.id, {
        success: (data) => {
          this.branch = <Branch>data;
        },
        error: (error) => {
          this.notificaton.warning(error);
        }
      });
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

      this._service.updateBranch(this.branch, {
        success: (data) => {
          this.notificaton.success("Branch  has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <Branch>data }));
        },
        error: (error) => {
          this.notificaton.warning(error)
        }
      })
    }
    else {

      this._service.createBranch(this.branch, {
        success: (data) => {
          this.notificaton.success("Branch has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <Branch>data }));
        },
        error: (error) => {
          this.notificaton.warning(error)
        }
      })

    }
  }
}