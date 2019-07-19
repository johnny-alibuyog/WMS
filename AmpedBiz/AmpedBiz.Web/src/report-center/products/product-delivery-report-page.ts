import { autoinject } from 'aurelia-framework';
import { ServiceApi } from '../../services/service-api';
import { Lookup } from '../../common/custom_types/lookup';
import { NotificationService } from '../../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../../common/models/paging';
import { ProductDeliveryReport, ProductDeliveryReportModel, ProductDeliveryReportItemModel } from './product-delivery-report';
import { ProductDeliveryReportPageItem } from '../../common/models/product';

@autoinject
export class ProductsDeliveredReportPage {

  public header: string = ' Product Delivery';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<ProductDeliveryReportPageItem> = new Pager<ProductDeliveryReportPageItem>();
  public branches: Lookup<string>[];
  public categories: Lookup<string>[];
  public products: Lookup<string>[];

  constructor(
    private readonly _api: ServiceApi,
    private readonly _report: ProductDeliveryReport,
    private readonly _notification: NotificationService,
  ) {
    this.filter["branchId"] = null;
    this.filter["categoryId"] = null;
    this.filter["productId"] = null;
    this.filter["fromDate"] = null;
    this.filter["toDate"] = null;
    this.filter.onFilter = () => this.getPage();
    this.sorter["branchName"] = SortDirection.None;
    this.sorter["categoryName"] = SortDirection.None;
    this.sorter["productName"] = SortDirection.Ascending;
    this.sorter["quantityUnit"] = SortDirection.None;
    this.sorter["unitPriceAmount"] = SortDirection.None;
    this.sorter["discountAmount"] = SortDirection.None;
    this.sorter["extendedPriceAmount"] = SortDirection.None;
    this.sorter["totalPriceAmount"] = SortDirection.None;
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
      let data = await this._api.products.getProductDeliveryReportPage({
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
        product: this.products.find(x => x.id == this.filter["productId"]),
        fromDate: this.filter["fromDate"],
        toDate: this.filter["toDate"],
      };

      let reportModel = <ProductDeliveryReportModel>{
        branchName: header.branch && header.branch.name || "All Branches",
        categoryName: header.category && header.category.name || "All Category",
        productName: header.product && header.product.name || "All Product",
        fromDate: header.fromDate,
        toDate: header.toDate,
        items: data.items.map(x => <ProductDeliveryReportItemModel>{
          shippedOn: x.shippedOn,
          branchName: x.branchName,
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
      await this._report.show(reportModel)
    }
    catch (error) {
      this._notification.error("Error encountered during report generation!");
    }
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.products.getProductDeliveryReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });

      let response = <PagerResponse<ProductDeliveryReportPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}
