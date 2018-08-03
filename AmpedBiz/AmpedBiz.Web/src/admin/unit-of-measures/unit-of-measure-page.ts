import { autoinject } from 'aurelia-framework';
import { DialogService } from 'aurelia-dialog';
import { UnitOfMeasureCreate } from './unit-of-measure-create';
import { UnitOfMeasure, UnitOfMeasurePageItem } from '../../common/models/unit-of-measure';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../../common/models/paging';

@autoinject
export class UnitOfMeasurePage {

  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<UnitOfMeasurePageItem> = new Pager<UnitOfMeasurePageItem>();

  constructor(
    private readonly _api: ServiceApi,
    private readonly _dialog: DialogService,
    private readonly _notification: NotificationService,
  ) {
    this.filter["code"] = '';
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getPage();

    this.sorter["code"] = SortDirection.None;
    this.sorter["name"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    await this.getPage();
  }

  public async getPage(): Promise<void> {
    try {
      let data = await this._api.unitOfMeasures.getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      var response = <PagerResponse<UnitOfMeasurePageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }

  public async create(): Promise<void> {
    let settings = { viewModel: UnitOfMeasureCreate, model: null };
    let response = await this._dialog.open(settings).whenClosed();
    if (!response.wasCancelled) await this.getPage();
  }

  public async edit(item: UnitOfMeasure): Promise<void> {
    let settings = { viewModel: UnitOfMeasureCreate, model: item };
    let response = await this._dialog.open(settings).whenClosed(); 
    if (!response.wasCancelled) await this.getPage();
  }

  public delete(item: any): void {
    /*
    var index = this.mockData.indexOf(item);
    if (index > -1) {
      this.mockData.splice(index, 1);
    }
    this.filter();
    */
  }
}