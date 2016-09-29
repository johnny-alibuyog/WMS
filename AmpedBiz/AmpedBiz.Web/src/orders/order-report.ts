import {autoinject} from 'aurelia-framework';
import {formatDate, formatNumber, emptyIfNull} from '../services/formaters';
import {ReportBuilder, Report, DocumentDefinition} from '../services/report-builder';
import {OrderStatus} from '../common/models/order';

export interface OrderReportModel {
  branchName?: string;
  customerName?: string;
  pricingSchemeName?: string;
  fromDate?: Date;
  toDate?: Date;
  items: OrderReportModelItem[];
}

export interface OrderReportModelItem {
  id?: string;
  branchName?: string;
  customerName?: string;
  pricingSchemeName?: string;
  orderedOn?: Date;
  orderedByName?: string;
  status?: OrderStatus;
  totalAmount?: number;
}

@autoinject
export class OrderReport implements Report<OrderReportModel> {
  private readonly _reportBuilder: ReportBuilder;

  constructor(reportBuilder: ReportBuilder) {
    this._reportBuilder = reportBuilder;
  }

  public show(data: OrderReportModel): void {
    var document = this.buildDocument(data);
    this._reportBuilder.build({ title: 'Orders', document: document });
  }

  private buildDocument(data: OrderReportModel): DocumentDefinition {
    let orderTableBody: any[] = [
      [
        { text: 'Customer', style: 'tableHeader' },
        { text: 'Branch', style: 'tableHeader' },
        //{ text: 'Pricing', style: 'tableHeader' },
        { text: 'Staff', style: 'tableHeader' },
        { text: 'Date', style: 'tableHeader' },
        { text: 'Status', style: 'tableHeader' },
        { text: 'Total', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        orderTableBody.push([
          { text: emptyIfNull(x.customerName), style: 'tableData' },
          { text: emptyIfNull(x.branchName), style: 'tableData' },
          //{ text: emptyIfNull(x.pricingSchemeName), style: 'tableData' },
          { text: emptyIfNull(x.orderedByName), style: 'tableData' },
          { text: formatDate(x.orderedOn), style: 'tableData' },
          { text: OrderStatus[x.status], style: 'tableData' },
          { text: formatNumber(x.totalAmount), style: 'tableData', alignment: 'right' },
        ])
      );
    }

    return <DocumentDefinition>{
      content:
      [
        {
          text: 'Orders',
          style: 'title'
        },
        {
          table:
          {
            body:
            [
              [
                { text: 'Branch Name: ', style: 'label' },
                { text: emptyIfNull(data.branchName), style: 'value' }
              ],
              [
                { text: 'Customer: ', style: 'label' },
                { text: emptyIfNull(data.customerName), style: 'value' }
              ],
              [
                { text: 'Pricing: ', style: 'label' },
                { text: emptyIfNull(data.pricingSchemeName), style: 'value' }
              ],
              [
                { text: 'From Date: ', style: 'label' },
                { text: formatDate(data.fromDate), style: 'value' }
              ],
              [
                { text: 'To Date: ', style: 'label' },
                { text: formatDate(data.toDate), style: 'value' }
              ],
            ],
          },
          style: 'tablePlain',
          layout: 'noBorders',
        },
        {
          table: {
            headerRows: 1,
            //widths: ['*', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto'],
            widths: ['*', 'auto', 'auto', 'auto', 'auto', 'auto'],
            body: orderTableBody
          },
          layout: 'lightHorizontalLines',
          style: 'tableExample',
        },
      ],
      styles:
      {
        title:
        {
          fontSize: 28,
          bold: true,
          margin: [0, 0, 0, 10]
        },
        header:
        {
          fontSize: 18,
          bold: true,
          margin: [0, 10, 0, 10]
        },
        label:
        {
          fontSize: 10,
          alignment: 'right',
        },
        value:
        {
          fontSize: 10,
          color: 'gray',
          alignment: 'left',
        },
        tablePlain:
        {
          alignment: 'right',
          margin: [0, 0, 0, 0]
        },
        tableHeader: {
          bold: true,
          fontSize: 10,
          color: 'black'
        },
        tableData: {
          fontSize: 10,
          color: 'gray'
        }
      },
    };
  }
}