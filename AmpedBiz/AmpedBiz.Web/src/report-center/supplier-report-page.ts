import { autoinject } from 'aurelia-framework';
import { SupplierReport, SupplierReportModel, SupplierReportModelItem } from './supplier-report';
import { SupplierReportPageItem } from '../common/models/supplier';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class SupplierReportPage {

  public header: string = ' Supplier Report';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<SupplierReportPageItem> = new Pager<SupplierReportPageItem>();

  constructor(
    private readonly _api: ServiceApi,
    private readonly _report: SupplierReport,
    private readonly _notification: NotificationService
  ) {
    this.filter.onFilter = () => this.getPage();
    this.sorter["supplierName"] = SortDirection.Ascending;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    await this.getPage();
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.suppliers.getSupplierReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });
      let response = <PagerResponse<SupplierReportPageItem>>data;
      let reportModel = <SupplierReportModel>{
        items: response.items.map(x => <SupplierReportModelItem>{
          id: x.id,
          name: x.name,
          contactPerson: x.contactPerson,
          contact: x.contact,
          address: x.address,
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
      let data = await this._api.suppliers.getSupplierReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      var response = <PagerResponse<SupplierReportPageItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}