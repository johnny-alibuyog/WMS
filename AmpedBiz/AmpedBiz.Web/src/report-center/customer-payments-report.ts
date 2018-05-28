import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { Report, Content } from './report';
import { OrderStatus } from '../common/models/order';

export interface CustomerPaymentsReportModel {
  branchName?: string;
  fromDate?: Date;
  toDate?: Date;
  items: CustomerPaymentsReportItemModel[];
}

export interface CustomerPaymentsReportItemModel {
  paidOn?: Date;
  invoiceNumber?: string;
  branchName?: string;
  customerName?: string;
  paymentTypeName?: string;
  totalAmount?: number;
  paidAmount?: number;
  balanceAmount?: number;
}

@autoinject
export class CustomerPaymentsReport extends Report<CustomerPaymentsReportModel> {

  protected async buildBody(data: CustomerPaymentsReportModel): Promise<any[] | Content[]> {
    let orderTableBody: any[] = [
      [
        { text: 'Date', style: 'tableHeader' },
        { text: 'Invoice', style: 'tableHeader' },
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Customer', style: 'tableHeader' },
        { text: 'Payment Type', style: 'tableHeader' },
        { text: 'Amount', style: 'tableHeader', alignment: 'right' },
        { text: 'Payment', style: 'tableHeader', alignment: 'right' },
        { text: 'Balance', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      // table content
      orderTableBody.push(...data.items.map(x => [
        { text: formatDate(x.paidOn), style: 'tableData' },
        { text: emptyIfNull(x.invoiceNumber), style: 'tableData' },
        { text: emptyIfNull(x.branchName), style: 'tableData' },
        { text: emptyIfNull(x.customerName), style: 'tableData' },
        { text: emptyIfNull(x.paymentTypeName), style: 'tableData' },
        { text: formatNumber(x.totalAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.paidAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.balanceAmount), style: 'tableData', alignment: 'right' },
      ]));

      // table footer
      orderTableBody.push([
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "Grand Total", style: "tableHeader" },
        { text: formatNumber(data.items.map(o => o.totalAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
        { text: formatNumber(data.items.map(o => o.paidAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
        { text: formatNumber(data.items.map(o => o.balanceAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
      ]);
    }

    let body = [
      {
        text: 'Customer Payments',
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
                  { text: 'Date: ', style: 'label' },
                  { text: formatDate(data.fromDate) + ' - ' + formatDate(data.toDate), style: 'value' }
                ],
              ],
          },
        style: 'tablePlain',
        layout: 'noBorders',
      },
      {
        text: ' ',
        style: 'separator'
      },
      {
        table: {
          headerRows: 1,
          widths: ['auto', 'auto', '*', 'auto', 'auto', 'auto', 'auto', 'auto'],
          body: orderTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return Promise.resolve(body);
  }
}