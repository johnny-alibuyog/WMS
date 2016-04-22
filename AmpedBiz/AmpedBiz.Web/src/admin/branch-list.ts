import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {BranchCreate} from './branch-create';
import {Branch} from './common/models/branch';
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
    this.sorter = sorter;
    this.pager = pager;

    this.filter["name"] = '';
  }

  activate() {
    this.getList();
  }

  doFilter() : void {
    this.getList();
  }

  doSort(field: string) {
    if (this.sorter[field] == SortDirection.Descending)
      this.sorter[field] = SortDirection.Ascending
    else
      this.sorter[field] = SortDirection.Descending;

    this.getList();
  }

  doPage(pageNumber: number) {
    if (this.pager.offset === pageNumber)
      return;

    this.pager.offset = pageNumber;
    
    this.getList();
  }

  getList() {
    this._service
      .getPages({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<Branch>>data;
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