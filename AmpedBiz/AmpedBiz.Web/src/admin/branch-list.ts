import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {BranchCreate} from './branch-create';
import {Branch} from './common/models/branch';
import {BranchService} from '../services/branch-service';
import {NotificationService} from '../common/controls/notification-service';

@autoinject
export class BranchList {
  private _notification: NotificationService;
  private _service: BranchService;
  private _dialog: DialogService;
  
  public branches: Branch[];
  public filterText: string = '';

  constructor(dialog: DialogService, service: BranchService, notification: NotificationService) {
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
    this._service.getBranches(this.filterText, {
      success: (data) => {
        this.branches = <Branch[]>data
        if (!this.branches || this.branches.length == 0){
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
      .open({ viewModel: BranchCreate, model: null })
      .then(response => {
        if (!response.wasCancelled) {
          this.refreshList();
        }
      });
  }

  edit(item: Branch) {
    this._dialog
      .open({ viewModel: BranchCreate, model: item })
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