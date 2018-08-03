import { autoinject } from 'aurelia-framework';
import { ProductReport, ProductReportModel, ProductReportModelItem } from './product-report';
import { ProductReportPageItem } from '../common/models/product';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class ProductReportPage {

  public header: string = ' Product Report';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ProductReportPageItem> = new Pager<ProductReportPageItem>();
  public products: Lookup<string>[];
  public suppliers?: Lookup<string>[];
  public categories?: Lookup<string>[];
  public measureTypes?: Lookup<string>[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _report: ProductReport,
    private readonly _notification: NotificationService
  ) {

    this.filter["productId"] = null;
    this.filter["categoryId"] = null;
    this.filter["supplierId"] = null;
    this.filter["mesureTypes"] = null;
    this.filter.onFilter = () => {
      // if filter is changed, reset the page to first
      this.pager.offset = 1;
      this.getPage();
    }
    this.sorter["productName"] = SortDirection.Ascending;
    this.sorter["categoryName"] = SortDirection.None;
    this.sorter["supplierName"] = SortDirection.None;
    this.sorter["onHandValue"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    [this.products, this.categories, this.suppliers] = await Promise.all([
      this._api.products.getLookups(),
      this._api.productCategories.getLookups(),
      this._api.suppliers.getLookups(),
    ]);
    this.measureTypes = [
      { id: "default", name: "Default" },
      { id: "standard", name: "Standard" },
    ];
    this.filter["measureType"] = this.measureTypes[0];
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
        get categoryName(): string {
          let selectedCategory = self.categories.find(x => x.id == self.filter['categoryId']);
          return selectedCategory && selectedCategory.name || 'All Categories';
        },
        get supplierName(): string {
          let selectedSupplier = self.suppliers.find(x => x.id == self.filter['supplierId']);
          return selectedSupplier && selectedSupplier.name || 'All Suppliers';
        },
      };
      let data = await this._api.products.getProductReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });
      let response = <PagerResponse<ProductReportPageItem>>data;
      let reportModel = <ProductReportModel>{
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
      await this._report.show(reportModel);
    }
    catch (error) {
      this._notification.error("Error encountered during report generation!");
    }
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.products.getProductReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<ProductReportPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}