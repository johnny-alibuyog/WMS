import { autoinject } from 'aurelia-framework';
import { UnitOfMeasureReport, UnitOfMeasureReportModel, UnitOfMeasureReportModelItem } from './unit-of-measure-report';
import { UnitOfMeasure } from '../common/models/unit-of-measure';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class UnitOfMeasureReportPage {

  public header: string = ' Unit Of Measure Report';
  public filter: Filter = new Filter();
  public sorter: Sorter = new Sorter();
  public pager: Pager<UnitOfMeasure> = new Pager<UnitOfMeasure>();

  constructor(
    private readonly _api: ServiceApi,
    private readonly _report: UnitOfMeasureReport,
    private readonly _notification: NotificationService
  ) {
    this.filter["UnitOfMeasureId"] = null;
    this.filter.onFilter = () => this.getPage();
    this.sorter["UnitOfMeasureName"] = SortDirection.Ascending;
    this.sorter.onSort = () => this.getPage();
    this.pager.onPage = () => this.getPage();
  }

  public async activate(): Promise<void> {
    await this.getPage();
  }

  public async generateReport(): Promise<void> {
    try {
      let data = await this._api.unitOfMeasures.getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      });
      let response = <PagerResponse<UnitOfMeasure>>data;
      let reportModel = <UnitOfMeasureReportModel>{
        items: <UnitOfMeasureReportModelItem[]>response.items
      };
      await this._report.show(reportModel);
    }
    catch (error) {
      this._notification.error("Error encountered during report generation!");
    }
  }

  private async getPage(): Promise<void> {
    try {
      let data = await this._api.unitOfMeasures.getPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      });
      let response = <PagerResponse<UnitOfMeasureReportModelItem>>data;
      this.pager.count = response.count;
      this.pager.items = response.items;
    }
    catch (error) {
      this._notification.error("Error encountered during search!");
    }
  }
}