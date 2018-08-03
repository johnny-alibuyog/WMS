import { autoinject } from 'aurelia-framework';
import { ProductReportPageItem } from '../common/models/product';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, SortDirection } from '../common/models/paging';
import { SalesReport, SalesReportModel, SalesReportModelItem } from './sales-report';

@autoinject
export class SalesReportPage {

  public header: string = ' Customer Sales';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ProductReportPageItem> = new Pager<ProductReportPageItem>();
  public products: Lookup<string>[];
  public suppliers?: Lookup<string>[];
  public categories?: Lookup<string>[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _report: SalesReport,
    private readonly _notification: NotificationService
  ) {
    this.filter["productId"] = '';
    this.filter["date"] = new Date();
    this.filter.onFilter = () => this.getPage();
    this.sorter["productName"] = SortDirection.Ascending;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async signalProductChanged(productId: string): Promise<void> {
    this.filter["productId"] = productId;
    await this.getPage();
  }

  public async activate(): Promise<void> {
    this.products = await this._api.products.getLookups();
    await this.getPage();
  }

  public async generateReport(): Promise<void> {
    try {
      let self = this;
      let selected = {
        get productName(): string {
          let selectedProduct = self.products.find(x => x.id == self.filter['productId']);
          return selectedProduct && selectedProduct.name || 'All Products';
        },
        get date(): Date {
          return self.filter['date'];
        }
      };
      let data = await this._api.orders.getSalesReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });
      let reportModel = <SalesReportModel>{
        productName: selected.productName,
        date: selected.date,
        items: <SalesReportModelItem>data.items
      };
      await this._report.show(reportModel)
    }
    catch (error) {
      this._notification.error("Error encountered during report generation!");
    }
  }

  private async getPage(): Promise<void> {
    try {
      let response = await this._api.orders.getSalesReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}