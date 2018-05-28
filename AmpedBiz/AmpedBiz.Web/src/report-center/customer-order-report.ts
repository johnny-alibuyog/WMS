import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { Report, Content } from './report';
import { OrderStatus } from '../common/models/order';

export interface CustomerOrderReportModel {
  branchName?: string;
  customerName?: string;
  pricingName?: string;
  fromDate?: Date;
  toDate?: Date;
  status?: string,
  items: CustomerOrderReportItemModel[];
}

export interface CustomerOrderReportItemModel {
  id?: string;
  branchName?: string;
  customerName?: string;
  invoiceNumber?: string
  pricingName?: string;
  orderedOn?: Date;
  orderedByName?: string;
  status?: OrderStatus;
  totalAmount?: number;
  paidAmount?: number;
  balanceAmount?: number;
}

@autoinject
export class CustomerOrderReport extends Report<CustomerOrderReportModel> {

  public constructor() {
    super();
    this.option.pageOrientation = 'landscape';
  }

  protected async buildBody(data: CustomerOrderReportModel): Promise<any[] | Content[]> {
    let orderTableBody: any[] = [
      [
        { text: 'Date', style: 'tableHeader' },
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Customer', style: 'tableHeader' },
        { text: 'Invoice', style: 'tableHeader' },
        { text: 'Pricing', style: 'tableHeader' },
        { text: 'Staff', style: 'tableHeader' },
        { text: 'Status', style: 'tableHeader' },
        { text: 'Total', style: 'tableHeader', alignment: 'right' },
        { text: 'Balance', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      // table content
      orderTableBody.push(...data.items.map(x => [
        { text: formatDate(x.orderedOn), style: 'tableData' },
        { text: emptyIfNull(x.branchName), style: 'tableData' },
        { text: emptyIfNull(x.customerName), style: 'tableData' },
        { text: emptyIfNull(x.invoiceNumber), style: 'tableData' },
        { text: emptyIfNull(x.pricingName), style: 'tableData' },
        { text: emptyIfNull(x.orderedByName), style: 'tableData' },
        { text: OrderStatus[x.status], style: 'tableData' },
        { text: formatNumber(x.totalAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.balanceAmount), style: 'tableData', alignment: 'right' }
      ]));

      // table footer
      orderTableBody.push([
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "Grand Total", style: "tableHeader" },
        { text: formatNumber(data.items.map(o => o.totalAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
        { text: formatNumber(data.items.map(o => o.balanceAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
      ]);

      // data.items.forEach(x =>
      //   orderTableBody.push([
      //     { text: formatDate(x.orderedOn), style: 'tableData' },
      //     { text: emptyIfNull(x.branchName), style: 'tableData' },
      //     { text: emptyIfNull(x.customerName), style: 'tableData' },
      //     { text: emptyIfNull(x.invoiceNumber), style: 'tableData' },
      //     { text: emptyIfNull(x.pricingName), style: 'tableData' },
      //     { text: emptyIfNull(x.orderedByName), style: 'tableData' },
      //     { text: OrderStatus[x.status], style: 'tableData' },
      //     { text: formatNumber(x.totalAmount), style: 'tableData', alignment: 'right' },
      //     { text: formatNumber(x.balanceAmount), style: 'tableData', alignment: 'right' },
      //   ])
      // );
    }

    let body = [
      {
        text: 'Customer Orders',
        style: 'title'
      },
      {
        columns: [
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
                      { text: 'Order Date: ', style: 'label' },
                      { text: formatDate(data.fromDate) + ' - ' + formatDate(data.toDate), style: 'value' }
                    ],
                    // [
                    //   { text: 'From Date: ', style: 'label' },
                    //   { text: formatDate(data.fromDate), style: 'value' }
                    // ],
                    // [
                    //   { text: 'To Date: ', style: 'label' },
                    //   { text: formatDate(data.toDate), style: 'value' }
                    // ],
                  ],
              },
            style: 'tablePlain',
            layout: 'noBorders',
          },
          {
            table:
              {
                body:
                  [
                    [
                      { text: 'Pricing: ', style: 'label' },
                      { text: emptyIfNull(data.pricingName), style: 'value' }
                    ],
                    [
                      { text: 'Status: ', style: 'label' },
                      { text: emptyIfNull(data.status), style: 'value' }
                    ],
                  ],
              },
            style: 'tablePlain',
            layout: 'noBorders',
          },
        ]
      },
      {
        text: ' ',
        style: 'separator'
      },
      {
        table: {
          headerRows: 1,
          //widths: ['*', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto'],
          widths: ['auto', 'auto', '*', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto'],
          body: orderTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return Promise.resolve(body);
  }
}