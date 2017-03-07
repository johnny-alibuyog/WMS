import { autoinject } from 'aurelia-framework';
import { OrderReport, OrderReportModel, OrderReportModelItem } from './order-report';
import { OrderStatus, OrderReportPageItem } from '../common/models/order';
import { ServiceApi } from '../services/service-api';
import { Lookup } from '../common/custom_types/lookup';
import { NotificationService } from '../common/controls/notification-service';
import { Filter, Sorter, Pager, PagerRequest, PagerResponse, SortDirection } from '../common/models/paging';

@autoinject
export class OrderReportPage {
  private _api: ServiceApi;
  private _report: OrderReport;
  private _notification: NotificationService;

  public header: string = ' Order Report';

  public filter: Filter;
  public sorter: Sorter;
  public pager: Pager<OrderReportPageItem>;

  public pricings: Lookup<string>[];
  public customers: Lookup<string>[];
  public statuses: Lookup<OrderStatus>[];
  public branches: Lookup<string>[];

  constructor(api: ServiceApi, report: OrderReport, notification: NotificationService) {
    this._api = api;
    this._report = report;
    this._notification = notification;

    this.filter = new Filter();
    this.filter["customerId"] = null;
    this.filter["branchId"] = null;
    this.filter["pricingShemeId"] = null;
    this.filter["fromDate"] = null;
    this.filter["toDate"] = null;
    this.filter["status"] = null;
    this.filter.onFilter = () => this.getPage();

    this.sorter = new Sorter();
    this.sorter["branchName"] = SortDirection.None;
    this.sorter["customerName"] = SortDirection.None;
    this.sorter["pricingName"] = SortDirection.None;
    this.sorter["status"] = SortDirection.None;
    this.sorter["orderedOn"] = SortDirection.Ascending;
    this.sorter["orderedByName"] = SortDirection.None;
    this.sorter["totalAmount"] = SortDirection.None;
    this.sorter.onSort = () => this.getPage();

    this.pager = new Pager<OrderReportPageItem>();
    this.pager.onPage = () => this.getPage();
  }

  public activate(): void {

    let requests: [
      Promise<Lookup<string>[]>,
      Promise<Lookup<string>[]>,
      Promise<Lookup<OrderStatus>[]>,
      Promise<Lookup<string>[]>] = [

        this._api.pricings.getLookups(),
        this._api.customers.getLookups(),
        this._api.orders.getStatusLookup(),
        this._api.branches.getLookups()
      ];

    Promise.all(requests).then((responses: [
      Lookup<string>[],
      Lookup<string>[],
      Lookup<OrderStatus>[],
      Lookup<string>[]]) => {

      this.pricings = responses[0];
      this.customers = responses[1];
      this.statuses = responses[2];
      this.branches = responses[3];

      this.getPage();
    });
  }

  public generateReport() {
    this._api.orders
      .getOrderReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>{
          offset: 0,
          size: 0
        }
      })
      .then(data => {
        var response = <PagerResponse<OrderReportPageItem>>data;
        var reportModel = <OrderReportModel>{
          items: response.items.map(x => <OrderReportModelItem>{
            id: x.id,
            branchName: x.branchName,
            customerName: x.customerName,
            pricingName: x.pricingName,
            orderedOn: x.orderedOn,
            orderedByName: x.orderedByName,
            status: x.status,
            totalAmount: x.totalAmount
          })
        };

        this._report.show(reportModel)
      })
      .catch(error => {
        this._notification.error("Error encountered during report generation!");
      });
  }

  private getPage(): void {
    this._api.orders
      .getOrderReportPage({
        filter: this.filter,
        sorter: this.sorter,
        pager: <PagerRequest>this.pager
      })
      .then(data => {
        var response = <PagerResponse<OrderReportPageItem>>data;
        this.pager.count = response.count;
        this.pager.items = response.items;
      })
      .catch(error => {
        this._notification.error("Error encountered during search!");
      });
  }
}