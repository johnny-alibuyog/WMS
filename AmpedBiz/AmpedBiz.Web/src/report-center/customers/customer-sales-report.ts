import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../../services/formaters';
import { Report, Content } from '../report';

export interface CustomerSalesReportModel {
  branchName?: string;
  customerName?: string;
  fromDate?: Date;
  toDate?: Date;
  items: CustomerSalesReportItemModel[];
}

export interface CustomerSalesReportItemModel {
  paymentOn?: Date;
  branchName?: string;
  customerName?: string;
  invoiceNumber?: string;
  status?: string;
  totalAmount?: number;
  paidAmount?: number;
  balanceAmount?: number;
}

@autoinject()
export class CustomerSalesReport extends Report<CustomerSalesReportModel> {

  protected async buildBody(data: CustomerSalesReportModel): Promise<any[] | Content[]> {
    let salesTableBody: any[] = [
      [
        { text: 'Date', style: 'tableHeader' },
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Customer', style: 'tableHeader' },
        { text: 'Invoice', style: 'tableHeader' },
        { text: 'Status', style: 'tableHeader' },
        { text: 'Total', style: 'tableHeader', alignment: 'right' },
        { text: 'Balance', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      // table content
      salesTableBody.push(...data.items.map(x => [
        { text: formatDate(x.paymentOn), style: 'tableData' },
        { text: emptyIfNull(x.branchName), style: 'tableData' },
        { text: emptyIfNull(x.customerName), style: 'tableData' },
        { text: emptyIfNull(x.invoiceNumber), style: 'tableData' },
        { text: emptyIfNull(x.status), style: 'tableData' },
        { text: formatNumber(x.paidAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.balanceAmount), style: 'tableData', alignment: 'right' },
      ]));

      // table footer
      salesTableBody.push([
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "Grand Total", style: "tableHeader" },
        { text: formatNumber(data.items.map(o => o.paidAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
        { text: formatNumber(data.items.map(o => o.balanceAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
      ]);
    }

    let body = [
      {
        text: 'Customer Sales',
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
                  { text: 'Sales Date: ', style: 'label' },
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
          widths: ['auto', 'auto', '*', 'auto', 'auto', 'auto', 'auto'],
          body: salesTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return Promise.resolve(body);
  }
}
