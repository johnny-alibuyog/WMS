import { autoinject } from 'aurelia-framework';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';
import { ProductsDeliveredReport, ProductsDeliveredReportModel, ProductsDeliveredReportItemModel } from './products-delivered-report';
import { ProductsDeliveredReportPageItem } from '../common/models/product';

@autoinject
export class ProductsDeliveredReportPage {
  private _api: ServiceApi;
  private _report: ProductsDeliveredReport;
  private _notification: NotificationService;

  public header: string = ' Products Delivered';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<ProductsDeliveredReportPageItem>;

  public branches: Lookup<string>[];
  public categories: Lookup<string>[];
  public suppliers: Lookup<string>[];
  public products: Lookup<string>[];

  constructor(api: ServiceApi, report: ProductsDeliveredReport, notification: NotificationService) {
    this._api = api;
    this._report = report;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["branchId"] = null;
    this.filter["categoryId"] = null;
    this.filter["supplierId"] = null;
    this.filter["productId"] = null;
    this.filter["fromDate"] = null;
    this.filter["toDate"] = null;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["completedOn"] = SortDirection.Ascending;
    this.sorter["branchName"] = SortDirection.None;
    this.sorter["supplierName"] = SortDirection.None;
    this.sorter["categoryName"] = SortDirection.None;
    this.sorter["productName"] = SortDirection.None;
    this.sorter["quantityUnit"] = SortDirection.None;
    this.sorter["unitPriceAmount"] = SortDirection.None;
    this.sorter["discountAmount"] = SortDirection.None;
    this.sorter["extendedPriceAmount"] = SortDirection.None;
    this.sorter["totalPriceAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<ProductsDeliveredReportPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    [
      this.branches,
      this.categories,
      this.suppliers,
      this.products
    ] = await Promise.all([
      this._api.branches.getLookups(),
      this._api.productCategories.getLookups(),
      this._api.suppliers.getLookups(),
      this._api.products.getLookups(),
    ]);

    this.getPage();
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.products.getProductSalesReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });

      let header = {
        branch: this.branches.find(x => x.id == this.filter["branchId"]),
        category: this.categories.find(x => x.id == this.filter["customerId"]),
        supplier: this.suppliers.find(x => x.id == this.filter["supplierId"]),
        product: this.products.find(x => x.id == this.filter["productId"]),
        fromDate: this.filter["fromDate"],
        toDate: this.filter["toDate"],
      };

      let reportModel = <ProductsDeliveredReportModel>{
        branchName: header.branch && header.branch.name || "All Branches",
        supplierName: header.supplier && header.supplier.name || "All Supplier",
        categoryName: header.category && header.category.name || "All Category",
        productName: header.product && header.product.name || "All Product",
        fromDate: header.fromDate,
        toDate: header.toDate,
        items: data.items.map(x => <ProductsDeliveredReportItemModel>{
          shippedOn: x.shippedOn,
          branchName: x.branchName,
          supplierName: x.supplierName,
          categoryName: x.categoryName,
          productName: x.productName,
          quantityUnit: x.quantityUnit,
          quantityValue: x.quantityValue,
          discountAmount: x.discountAmount,
          unitPriceAmount: x.unitPriceAmount,
          extendedPriceAmount: x.extendedPriceAmount,
          totalPriceAmount: x.totalPriceAmount,
        })
      };
      this._report.show(reportModel)
    }
    catch (error) {
      this._notification.error("Error encountered during report generation!");
    }
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.products.getProductSalesReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });

      let response = <PagerResponse<ProductsDeliveredReportPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}