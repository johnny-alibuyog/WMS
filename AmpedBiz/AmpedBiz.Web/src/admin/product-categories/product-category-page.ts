import { autoinject } from 'aurelia-framework';
import { DialogService } from 'aurelia-dialog';
import { ProductCategoryCreate } from './product-category-create';
import { ProductCategory, ProductCategoryPageItem } from '../../common/models/product-category';
import { ServiceApi } from '../../services/service-api';
import { NotificationService } from '../../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../../common/models/paging';

@autoinject
export class ProductCategoryPage {

  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ProductCategoryPageItem> = new Pager<ProductCategoryPageItem>();

  constructor(
    private readonly _api: ServiceApi,
    private readonly _dialog: DialogService,
    private readonly _notification: NotificationService,
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
      let data = await this._api.productCategories.getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<ProductCategoryPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }

  public async create(): Promise<void> {
    let settings = { viewModel: ProductCategoryCreate, model: null };
    let response = await this._dialog.open(settings).whenClosed();
    if (!response.wasCancelled) await this.getPage();
  }

  public async edit(item: ProductCategoryPageItem): Promise<void> {
    let settings = { viewModel: ProductCategoryCreate, model: <ProductCategory>{ id: item.id } };
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