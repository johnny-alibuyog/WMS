import { autoinject } from 'aurelia-framework';
import { DialogService } from 'aurelia-dialog';
import { PaymentTypeCreate } from './payment-type-create';
import { PaymentType, PaymentTypePageItem } from '../../common/models/payment-type';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../../common/models/paging';

@autoinject
export class PaymentTypePage {

  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<PaymentTypePageItem> = new Pager<PaymentTypePageItem>();

  constructor(
    private _api: ServiceApi,
    private _dialog: DialogService,
    private _notification: NotificationService,
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
      let data = await this._api.paymentTypes.getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<PaymentTypePageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }

  public async create(): Promise<void> {
    let settings = { viewModel: PaymentTypeCreate, model: null };
    let response = await this._dialog.open(settings).whenClosed();
    if (!response.wasCancelled) await this.getPage();
  }

  public async edit(item: PaymentTypePageItem): Promise<void> {
    let settings = { viewModel: PaymentTypeCreate, model: <PaymentType>{ id: item.id } };
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