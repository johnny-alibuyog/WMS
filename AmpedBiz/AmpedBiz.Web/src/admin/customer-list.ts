import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {CustomerCreate} from './customer-create';
import {Customer} from './common/models/customer';
import {CustomerService} from '../services/customer-service';
import {NotificationService} from '../common/controls/notification-service';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class CustomerList {
  private _notification: NotificationService;
  private _service: CustomerService;
  private _dialog: DialogService;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<Customer>;

  constructor(dialog: DialogService, service: CustomerService, notification: NotificationService, filter: Filter, sorter: Sorter, pager: Pager<Customer>) {
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
        var response = <PagerResponse<Customer>>data;
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
      .open({ viewModel: CustomerCreate, model: null })
      .then(response => {
        if (!response.wasCancelled) {
          this.getList();
        }
      });
  }

  edit(item: Customer) {
    this._dialog
      .open({ viewModel: CustomerCreate, model: item })
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