import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {UserCreate} from './user-create';
import {User} from './common/models/user';
import {UserService} from '../services/user-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class BranchList {
  private _notification: NotificationService;
  private _service: UserService;
  private _dialog: DialogService;
  
  public users: User[];
  public filterText: string = '';

  constructor(dialog: DialogService, service: UserService, notification: NotificationService) {
    this._dialog = dialog;
    this._service = service;
    this._notification = notification;
  }

  activate() {
    this.refreshList();
  }

  refreshList() {
    this.filterText = '';
    this.filter();
  }
  
  filter() {
    this._service.getUsers(this.filterText, {
      success: (data) => {
        this.users = <User[]>data
        if (!this.users || this.users.length == 0){
          this._notification.warning("No items found!");
        }
      },
      error: (error) => {
        this._notification.error("Error encountered during search!");
      }
    });
  }

  create() {
    this._dialog
      .open({ viewModel: UserCreate, model: null})
      .then(response => {
        if (!response.wasCancelled) {
          this.refreshList();
        }
      });
  }

  edit(item: User) {
    this._dialog
      .open({ viewModel: UserCreate, model: item })
      .then(response => {
        if (!response.wasCancelled) {
          this.refreshList();
        }
      });
  }

  delete(item: any) {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}