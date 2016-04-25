import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {BranchCreate} from './branch-create';
import {Branch, BranchPageItem} from './common/models/branch';
import {BranchService} from '../services/branch-service';
import {NotificationService} from '../common/controls/notification-service';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class BranchList {
  private _dialog: DialogService;
  private _service: BranchService;
  private _notification: NotificationService;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<Branch>;

  constructor(dialog: DialogService, service: BranchService, notification: NotificationService, filter: Filter, sorter: Sorter, pager: Pager<Branch>) {
    this._dialog = dialog;
    this._service = service;
    this._notification = notification;

    this.filter = filter;
    this.filter.onFilter = () => this.getList();
    this.filter["name"] = '';

    this.sorter = sorter;
    this.sorter.onSort = () => this.getList();
    this.sorter["code"] = SortDirection.Ascending;
    this.sorter["name"] = SortDirection.None;
    this.sorter["descirption"] = SortDirection.None;

    this.pager = pager;
    this.pager.onPage = () => this.getList();
  }

  activate() {
    this.getList();
  }

  getList(): void {
    this._service
      .getPages({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<BranchPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;

        if (this.pager.count === 0) {
          this._notification.warning("No items found!");
        }
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  create() {
    this._dialog
      .open({ viewModel: BranchCreate, model: null })
      .then(response => {
        if (!response.wasCancelled) {
          this.getList();
        }
      });
  }

  edit(item: Branch) {
    this._dialog
      .open({ viewModel: BranchCreate, model: item })
      .then(response => {
        if (!response.wasCancelled) {
          this.getList();
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