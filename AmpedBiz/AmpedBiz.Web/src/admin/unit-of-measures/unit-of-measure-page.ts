import { autoinject } from 'aurelia-framework';
import { DialogService } from 'aurelia-dialog';
import { UnitOfMeasureCreate } from './unit-of-measure-create';
import { UnitOfMeasure, UnitOfMeasurePageItem } from '../../common/models/unit-of-measure';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../../common/models/paging';

@autoinject
export class UnitOfMeasurePage {
  private _api: ServiceApi;
  private _dialog: DialogService;
  private _notification: NotificationService;

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<UnitOfMeasurePageItem>;

  constructor(api: ServiceApi, dialog: DialogService, notification: NotificationService, filter: Filter, sorter: Sorter, pager: Pager<UnitOfMeasurePageItem>) {
    this._api = api;
    this._dialog = dialog;
    this._notification = notification;

    this.filter = filter;
    this.filter["code"] = '';
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getPage();

    this.sorter = sorter;
    this.sorter["code"] = SortDirection.None;
    this.sorter["name"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = pager;
    this.pager.onPage = () => this.getPage();
  }

  activate() {
    this.getPage();
  }

  getPage() {
    this._api.unitOfMeasures
      .getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<UnitOfMeasurePageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }

  create() {
    this._dialog.open({ viewModel: UnitOfMeasureCreate, model: null })
      .whenClosed(response => { if (!response.wasCancelled) this.getPage(); });
  }

  edit(item: UnitOfMeasure) {
    this._dialog.open({ viewModel: UnitOfMeasureCreate, model: item })
      .whenClosed(response => { if (!response.wasCancelled) this.getPage(); });
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