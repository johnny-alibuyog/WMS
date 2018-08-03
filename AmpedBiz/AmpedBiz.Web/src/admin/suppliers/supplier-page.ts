import { autoinject } from 'aurelia-framework';
import { DialogService } from 'aurelia-dialog';
import { SupplierCreate } from './supplier-create';
import { Supplier, SupplierPageItem } from '../../common/models/supplier';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../../common/models/paging';

@autoinject
export class SupplierPage {

  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<SupplierPageItem> = new Pager<SupplierPageItem>();

  constructor(
    private readonly _api: ServiceApi,
    private readonly _dialog: DialogService,
    private readonly _notification: NotificationService
  ) {
    this.filter["name"] = '';
    this.filter.onFilter = () => this.getPage();
    this.sorter["code"] = SortDirection.None;
    this.sorter["name"] = SortDirection.Ascending;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    await this.getPage();
  }

  public async getPage(): Promise<void> {
    try {
      let data = await this._api.suppliers.getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      var response = <PagerResponse<SupplierPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    } 
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }

  public async create(): Promise<void> {
    let settings = { viewModel: SupplierCreate, model: null };
    let response = await this._dialog.open(settings).whenClosed();
    if (!response.wasCancelled) await this.getPage();
  }

  public async edit(item: SupplierPageItem) : Promise<void> {
    let settings = { viewModel: SupplierCreate, model: <Supplier>{ id: item.id } };
    let response = await this._dialog.open(settings).whenClosed();
    if (!response.wasCancelled) await this.getPage();
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