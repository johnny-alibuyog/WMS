import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { Report, Content } from './report';

export interface CustomerOrderDeliveryReportModel {
  branchName?: string;
  fromDate?: Date;
  toDate?: Date;
  items: CustomerOrderDeliveryReportItemModel[];
}

export interface CustomerOrderDeliveryReportItemModel {
  id?: string;
  shippedOn?: Date;
  branchName?: string;
  customerName?: string;
  invoiceNumber?: string
  pricingName?: string;
  discountAmount?: number;
  totalAmount?: number;
  subTotalAmount?: number;
}

@autoinject
export class CustomerOrderDeliveryReport extends Report<CustomerOrderDeliveryReportModel> {

  public constructor() {
    super();
    //this.option.pageOrientation = 'landscape';
  }
  protected async buildBody(data: CustomerOrderDeliveryReportModel): Promise<any[] | Content[]> {
    let orderTableBody: any[] = [
      [
        { text: 'Date', style: 'tableHeader' },
        { text: 'Invoice', style: 'tableHeader' },
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Customer', style: 'tableHeader' },
        { text: 'Pricing', style: 'tableHeader' },
        { text: 'Sub Total', style: 'tableHeader', alignment: 'right' },
        { text: 'Discount', style: 'tableHeader', alignment: 'right' },
        { text: 'Total', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      // table content
      orderTableBody.push(...data.items.map(x => [
        { text: formatDate(x.shippedOn), style: 'tableData' },
        { text: emptyIfNull(x.invoiceNumber), style: 'tableData' },
        { text: emptyIfNull(x.branchName), style: 'tableData' },
        { text: emptyIfNull(x.customerName), style: 'tableData' },
        { text: emptyIfNull(x.pricingName), style: 'tableData' },
        { text: formatNumber(x.subTotalAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.discountAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.totalAmount), style: 'tableData', alignment: 'right' },
      ]));

      // table footer
      orderTableBody.push([
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "Grand Total", style: "tableHeader" },
        { text: formatNumber(data.items.map(o => o.subTotalAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
        { text: formatNumber(data.items.map(o => o.discountAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
        { text: formatNumber(data.items.map(o => o.totalAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
      ]);
    }

    let body = [
      {
        text: 'Customer Order Delivery',
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
                      { text: 'Order Date: ', style: 'label' },
                      { text: formatDate(data.fromDate) + ' - ' + formatDate(data.toDate), style: 'value' }
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
          widths: ['auto', 'auto', 'auto', '*', 'auto', 'auto', 'auto', 'auto'],
          body: orderTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return Promise.resolve(body);
  }
}