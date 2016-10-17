import {autoinject} from 'aurelia-framework';
import {UnitOfMeasureClassReport, UnitOfMeasureClassReportModel, UnitOfMeasureClassReportModelItem} from './unit-of-measure-class-report';
import {UnitOfMeasureClass} from '../common/models/unit-of-measure-class';
import {UnitOfMeasure} from '../common/models/unit-of-measure';
import {ServiceApi} from '../services/service-api';
import {Lookup} from '../common/custom_types/lookup';
import {NotificationService} from '../common/controls/notification-service';
import {Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection} from '../common/models/paging';

@autoinject
export class UnitOfMeasureClassReportPage {
  private _api: ServiceApi;
  private _report: UnitOfMeasureClassReport;
  private _notification: NotificationService;

  public header: string = ' Unit Of Measure Report';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<UnitOfMeasureClass>;

  constructor(api: ServiceApi, report: UnitOfMeasureClassReport, notification: NotificationService) {
    this._api = api;
    this._report = report;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["unitOfMeasureClassId"] = null;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["unitOfMeasureClassName"] = SortDirection.Ascending;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<UnitOfMeasureClass>();
    this.pager.onPage = () => this.getPage();
  }

  public activate(): void {
      this.getPage();
  }

  public generateReport(): void {
    this._api.unitOfMeasureClasses
      .getUnitOfMeasureClassReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      })
      .then(data => {
        var response = <PagerResponse<UnitOfMeasureClass>>data;
        var reportModel = <UnitOfMeasureClassReportModel>{
          items: response.items.map(classItem => <UnitOfMeasureClassReportModelItem>{
            id: classItem.id,
            name: classItem.name,
            units: classItem.units.map(unitItem => <UnitOfMeasure>{
              id: unitItem.id,
              name: unitItem.name,
              isBaseUnit: unitItem.isBaseUnit,
              conversionFactor: unitItem.conversionFactor,
              unitOfMeasureClassId: unitItem.unitOfMeasureClassId
            })
          })
        };

        this._report.show(reportModel)
      })
      .catch(error => {
        this._notification.error("Error encountered during report generation!");
      });
  }

  private getPage(): void {
    this._api.unitOfMeasureClasses
      .getUnitOfMeasureClassReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<UnitOfMeasureClassReportModelItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }
}