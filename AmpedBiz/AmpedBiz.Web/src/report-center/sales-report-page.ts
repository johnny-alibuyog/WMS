import { autoinject } from 'aurelia-framework';
import { ProductReportPageItem } from '../common/models/product';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { SalesReport, SalesReportModel, SalesReportModelItem } from './sales-report';
import { LookupIdToNameValueConverter } from '../common/converters/lookup-id-to-name-value-converter';

@autoinject
export class SalesReportPage {
  private readonly _api: ServiceApi;
  private readonly _report: SalesReport;
  private readonly _notification: NotificationService;

  public header: string = ' Sales Report';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<ProductReportPageItem>;

  public products: Lookup<string>[];
  public suppliers?: Lookup<string>[];
  public categories?: Lookup<string>[];

  constructor(api: ServiceApi, report: SalesReport, notification: NotificationService) {
    this._api = api;
    this._report = report;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["productId"] = '';
    this.filter["date"] = new Date();
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["productName"] = SortDirection.Ascending;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<ProductReportPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  public signalProductChanged(productId: string){
    this.filter["productId"] = productId;
    this.getPage();
  }

  public activate(): void {

    let requests: [Promise<Lookup<string>[]>] = [
      this._api.products.getLookups(),
    ];

    Promise.all(requests).then((responses: [Lookup<string>[]]) => {

      this.products = responses[0];

      this.getPage();
    });
  }

  public generateReport(): void {
    var self = this;
    var selected = {
      get productName(): string {
        var selectedProduct = self.products.find(x => x.id == self.filter['productId']);
        return selectedProduct && selectedProduct.name || 'All Products';
      },
      get date(): Date {
        return self.filter['date'];
      }
    };

    this._api.orders
      .getSalesReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      })
      .then(response => {
        var reportModel = <SalesReportModel>{
          productName: selected.productName,
          date: selected.date,
          items: <SalesReportModelItem>response.items
        };

        this._report.show(reportModel)
      })
      .catch(error => {
        this._notification.error("Error encountered during report generation!");
      });
  }

  private getPage(): void {
    this._api.orders
      .getSalesReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(response => {
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }
}