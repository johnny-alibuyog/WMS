import { autoinject } from 'aurelia-framework';
import { SupplierReport, SupplierReportModel, SupplierReportModelItem } from './supplier-report';
import { SupplierReportPageItem } from '../common/models/supplier';
import { ServiceApi } from '../services/service-api';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class SupplierReportPage {
  private _api: ServiceApi;
  private _report: SupplierReport;
  private _notification: NotificationService;

  public header: string = ' Supplier Report';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<SupplierReportPageItem>;

  constructor(api: ServiceApi, report: SupplierReport, notification: NotificationService) {
    this._api = api;
    this._report = report;
    this._notification = notification;

    this.filter = new Filter();
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["supplierName"] = SortDirection.Ascending;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<SupplierReportPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  public activate(): void {
    this.getPage();
  }

  public generateReport() {
    this._api.suppliers
      .getSupplierReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      })
      .then(data => {
        var response = <PagerResponse<SupplierReportPageItem>>data;
        var reportModel = <SupplierReportModel>{
          items: response.items.map(x => <SupplierReportModelItem>{
            id: x.id,
            name: x.name,
            contactPerson: x.contactPerson,
            contact: x.contact,
            address: x.address,
          })
        };

        this._report.show(reportModel)
      })
      .catch(error => {
        this._notification.error("Error encountered during report generation!");
      });
  }

  private getPage(): void {
    this._api.suppliers
      .getSupplierReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<SupplierReportPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }
}