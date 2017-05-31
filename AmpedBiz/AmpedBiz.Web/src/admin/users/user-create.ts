import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { User } from '../../common/models/user';
import { Branch } from '../../common/models/branch';
import { UserService } from '../../services/user-service';
import { BranchService } from '../../services/branch-service';
import { NotificationService } from '../../common/controls/notification-service';

@autoinject
export class UserCreate {
  private readonly _notificaton: NotificationService;
  private readonly _controller: DialogController;
  private readonly _service: UserService;
  private readonly _branchService: BranchService;

  public header: string = 'Create User';
  public isEdit: boolean = false;
  public canSave: boolean = true;
  public user: User;
  public branches: Branch[];

  constructor(notification: NotificationService, controller: DialogController, service: UserService, branchService: BranchService) {
    this._branchService = branchService;
    this._notificaton = notification;
    this._controller = controller;
    this._service = service;
  }

  public activate(user: User): void {
    this._branchService.getList()
      .then(data => this.branches = <Branch[]>data)
      .catch(error => this._notificaton.warning(error));

    if (user) {
      this.header = "Edit User";
      this.isEdit = true;
      this._service.get(user.id)
        .then(data => this.user = <User>data)
        .catch(error => this._notificaton.warning(error));
    }
    else {
      this.header = "Create User";
      this.isEdit = false;
      this._service.getInitialUser(null)
        .then(data => this.user = <User>data)
        .catch(error => this._notificaton.warning(error));
    }
  }

  public changeValue(): void {
    this.isEdit = !this.isEdit;
  }

  public cancel(): void {
    this._controller.cancel({ wasCancelled: true, output: null });
  }

  public save(): void {
    if (this.isEdit) {

      this._service.update(this.user)
        .then(data => {
          this._notificaton.success("User  has been saved.")
            .whenClosed((data) => this._controller.ok({ wasCancelled: true, output: <User>data }));
        })
        .catch(error => {
          this._notificaton.warning(error);
        });
    }
    else {

      this._service.create(this.user)
        .then(data => {
          this._notificaton.success("User has been saved.")
            .whenClosed((data) => this._controller.ok({ wasCancelled: true, output: <User>data }));
        })
        .catch(error => {
          this._notificaton.warning(error);
        });
    }
  }
}