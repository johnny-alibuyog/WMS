import { autoinject } from 'aurelia-framework';
import { ServiceApi } from '../../services/service-api';
import { Lookup } from '../../common/custom_types/lookup';
import { NotificationService } from '../../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../../common/models/paging';
import { ProductListingReportPageItem } from '../../common/models/product';
import { ProductListingReport, ProductListingReportModel, ProductListingReportItemModel } from './product-listing-report';

@autoinject
export class ProductListingReportPage {

  public header: string = ' Product Listing';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ProductListingReportPageItem> = new Pager<ProductListingReportPageItem>();
  public branches: Lookup<string>[];
  public categories: Lookup<string>[];
  public products: Lookup<string>[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _report: ProductListingReport,
    private readonly _notification: NotificationService,
  ) {
    this.filter["customerId"] = null;
    this.filter["categoryId"] = null;
    this.filter["productId"] = null;
    this.filter["fromDate"] = null;
    this.filter["toDate"] = null;
    this.filter.onFilter = () => this.getPage();
    this.sorter["branchName"] = SortDirection.None;
    this.sorter["categoryName"] = SortDirection.None;
    this.sorter["productName"] = SortDirection.Ascending;
    this.sorter["unitOfMeasure"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    [
      this.branches,
      this.categories,
      this.products
    ] = await Promise.all([
      this._api.branches.getLookups(),
      this._api.productCategories.getLookups(),
      this._api.products.getLookups(),
    ]);
    await this.getPage();
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.products.getProductListingReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });

      let header = {
        branch: this.branches.find(x => x.id == this.filter["branchId"]),
        category: this.categories.find(x => x.id == this.filter["categoryId"]),
        product: this.products.find(x => x.id == this.filter["productId"]),
      };

      let reportModel = <ProductListingReportModel>{
        branchName: header.branch && header.branch.name || "All Branches",
        categoryName: header.category && header.category.name || "All Categories",
        productName: header.product && header.product.name || "All Products",
        items: data.items.map(x => <ProductListingReportItemModel>{
          branchName: x.branchName,
          categoryName: x.categoryName,
          productName: x.productName,
          quantityUnit: x.quantityUnit,
          onHandValue: x.onHandValue,
          availableValue: x.availableValue,
          basePriceAmount: x.basePriceAmount,
          wholesalePriceAmount: x.wholesalePriceAmount,
          retailPriceAmount: x.retailPriceAmount,
          suggestedRetailPriceAmount: x.suggestedRetailPriceAmount,
        })
      };
      await this._report.show(reportModel)
    }
    catch (error) {
      this._notification.error("Error encountered during report generation!");
    }
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.products.getProductListingReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<ProductListingReportPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}
