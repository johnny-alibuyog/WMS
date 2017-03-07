import { autoinject } from 'aurelia-framework';
import { UnitOfMeasureReport, UnitOfMeasureReportModel, UnitOfMeasureReportModelItem } from './unit-of-measure-report';
import { UnitOfMeasure } from '../common/models/unit-of-measure';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class UnitOfMeasureReportPage {
  private _api: ServiceApi;
  private _report: UnitOfMeasureReport;
  private _notification: NotificationService;

  public header: string = ' Unit Of Measure Report';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<UnitOfMeasure>;

  constructor(api: ServiceApi, report: UnitOfMeasureReport, notification: NotificationService) {
    this._api = api;
    this._report = report;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["UnitOfMeasureId"] = null;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["UnitOfMeasureName"] = SortDirection.Ascending;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<UnitOfMeasure>();
    this.pager.onPage = () => this.getPage();
  }

  public activate(): void {
    this.getPage();
  }

  public generateReport(): void {
    this._api.unitOfMeasures
      .getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      })
      .then(data => {
        var response = <PagerResponse<UnitOfMeasure>>data;
        var reportModel = <UnitOfMeasureReportModel>{
          items: <UnitOfMeasureReportModelItem[]>response.items
        };

        this._report.show(reportModel)
      })
      .catch(error => {
        this._notification.error("Error encountered during report generation!");
      });
  }

  private getPage(): void {
    this._api.unitOfMeasures
      .getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<UnitOfMeasureReportModelItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }
}