import {autoinject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {User} from './common/models/user';
import {Branch} from './common/models/branch';
import {UserService} from '../services/user-service';
import {BranchService} from '../services/branch-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class UserCreate {
  private _controller: DialogController;
  private _service: UserService;
  private _branchService: BranchService;

  public header: string = 'Create User';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public user: User;
  public branches: Branch[];
  public notificaton: NotificationService;

  constructor(notification: NotificationService, controller: DialogController, service: UserService, branchService: BranchService) {
    this._branchService = branchService;
    this.notificaton = notification;
    this._controller = controller;
    this._service = service;
  }

  activate(user: User) {
    this._branchService.getBranches(null, {
        success: (data) => {
          this.branches = <Branch[]>data;
        },
        error: (error) => {
          this.notificaton.warning(error);
        }
      });
    
    if (user) {
      this.header = "Edit User";
      this.isEdit = true;
      this._service.getUser(user.id, {
        success: (data) => {
          this.user = <User>data;
        },
        error: (error) => {
          this.notificaton.warning(error);
        }
      });
    }
    else {
      this.header = "Create User";
      this.isEdit = false;
      this._service.getInitialUser(null, {
        success: (data) => {
          this.user = <User>data;
        },
        error: (error) => {
          this.notificaton.warning(error);
        }
      });
    }
  }

  cancel() {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  save() {
    if (this.isEdit) {

      this._service.updateUser(this.user, {
        success: (data) => {
          this.notificaton.success("User  has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <User>data }));
        },
        error: (error) => {
          this.notificaton.warning(error)
        }
      })
    }
    else {

      this._service.createUser(this.user, {
        success: (data) => {
          this.notificaton.success("User has been saved.")
            .then((data) => this._controller.ok({ wasCancelled: true, output: <User>data }));
        },
        error: (error) => {
          this.notificaton.warning(error)
        }
      })

    }
  }
}