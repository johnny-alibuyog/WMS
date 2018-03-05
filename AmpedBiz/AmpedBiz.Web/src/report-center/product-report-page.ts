import { autoinject } from 'aurelia-framework';
import { ProductReport, ProductReportModel, ProductReportModelItem } from './product-report';
import { ProductReportPageItem } from '../common/models/product';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { LookupIdToNameValueConverter } from '../common/converters/lookup-id-to-name-value-converter';

@autoinject
export class ProductReportPage {
  private _api: ServiceApi;
  private _report: ProductReport;
  private _notification: NotificationService;

  public header: string = ' Product Report';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<ProductReportPageItem>;

  public products: Lookup<string>[];
  public suppliers?: Lookup<string>[];
  public categories?: Lookup<string>[];
  public measureTypes?: Lookup<string>[];

  constructor(api: ServiceApi, report: ProductReport, notification: NotificationService) {
    this._api = api;
    this._report = report;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["productId"] = null;
    this.filter["categoryId"] = null;
    this.filter["supplierId"] = null;
    this.filter["mesureTypes"] = null;
    this.filter.onFilter = () => {
      // if filter is changed, reset the page to first
      this.pager.offset = 1;
      this.getPage();
    }

    this.sorter = new Sorter();
    this.sorter["productName"] = SortDirection.Ascending;
    this.sorter["categoryName"] = SortDirection.None;
    this.sorter["supplierName"] = SortDirection.None;
    this.sorter["onHandValue"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<ProductReportPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  public activate(): void {

    let requests: [
      Promise<Lookup<string>[]>,
      Promise<Lookup<string>[]>,
      Promise<Lookup<string>[]>] = [

        this._api.products.getLookups(),
        this._api.productCategories.getLookups(),
        this._api.suppliers.getLookups(),
      ];

    Promise.all(requests).then((responses: [
      Lookup<string>[],
      Lookup<string>[],
      Lookup<string>[]]) => {

      this.products = responses[0];
      this.categories = responses[1];
      this.suppliers = responses[2];
      this.measureTypes = [
        { id: "default", name: "Default" },
        { id: "standard", name: "Standard" },
      ];
      this.filter["measureType"] = this.measureTypes[0];

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
      get categoryName(): string {
        var selectedCategory = self.categories.find(x => x.id == self.filter['categoryId']);
        return selectedCategory && selectedCategory.name || 'All Categories';
      },
      get supplierName(): string {
        var selectedSupplier = self.suppliers.find(x => x.id == self.filter['supplierId']);
        return selectedSupplier && selectedSupplier.name || 'All Suppliers';
      },
    };

    this._api.products
      .getProductReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      })
      .then(data => {
        var response = <PagerResponse<ProductReportPageItem>>data;
        var reportModel = <ProductReportModel>{
          productName: selected.productName,
          categoryName: selected.categoryName,
          supplierName: selected.supplierName,
          items: response.items.map(x => <ProductReportModelItem>{
            id: x.id,
            productName: x.productName,
            categoryName: x.categoryName,
            supplierName: x.supplierName,
            onHandUnit: x.onHandUnit,
            onHandValue: x.onHandValue,
            basePriceAmount: x.basePriceAmount,
            wholesalePriceAmount: x.wholesalePriceAmount,
            retailPriceAmount: x.retailPriceAmount,
            totalBasePriceAmount: x.totalBasePriceAmount,
            totalWholesalePriceAmount: x.totalWholesalePriceAmount,
            totalRetailPriceAmount: x.totalRetailPriceAmount,
          })
        };

        this._report.show(reportModel)
      })
      .catch(error => {
        this._notification.error("Error encountered during report generation!");
      });
  }

  private getPage(): void {
    this._api.products
      .getProductReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<ProductReportPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }
}