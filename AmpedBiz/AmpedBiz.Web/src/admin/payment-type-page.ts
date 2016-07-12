import {autoinject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {PaymentTypeCreate} from './payment-type-create';
import {PaymentType, PaymentTypePageItem} from '../common/models/payment-type';
import {ServiceApi} from '../services/service-api';
import {NotificationService} from '../common/controls/notification-service';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class PaymentTypePage {
  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<PaymentTypePageItem>;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, filter: Filter, sorter: Sorter, pager: Pager<PaymentTypePageItem>) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;

    this.filter = filter;
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getPage();

    this.sorter = sorter;
    this.sorter["code"] = SortDirection.None;
    this.sorter["name"] = SortDirection.Ascending;
    this.sorter.onSort = () => this.getPage();

    this.pager = pager;
    this.pager.onPage = () => this.getPage();
  }

  activate() {
    this.getPage();
  }

  getPage() {
    this._api.paymentTypes
      .getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<PaymentTypePageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  create() {
    this._dialog
      .open({
        viewModel: PaymentTypeCreate,
        model: null
      })
      .then(response => {
        if (!response.wasCancelled) {
          this.getPage();
        }
      });
  }

  edit(item: PaymentTypePageItem) {
    this._dialog
      .open({
        viewModel: PaymentTypeCreate,
        model: <PaymentType>{ id: item.id }
      })
      .then(response => {
        if (!response.wasCancelled) {
          this.getPage();
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