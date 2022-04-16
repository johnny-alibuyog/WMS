import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../../services/formaters';
import { Report, Content } from '../report';
import Enumerable from 'linq';

export interface CustomerPaymentsReportModel {
  branchName?: string;
  fromDate?: Date;
  toDate?: Date;
  items: CustomerPaymentsReportItemModel[];
}

export interface CustomerPaymentsReportItemModel {
  paymentOn?: Date;
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

  public constructor() {
    super();
    this.option.pageOrientation = 'landscape';
  }

  protected async buildBody(data: CustomerPaymentsReportModel): Promise<any[] | Content[]> {
    let tableBody: any[] = [
      [
        { text: 'Date', style: 'tableHeader' },
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Customer', style: 'tableHeader' },
        { text: 'Invoice', style: 'tableHeader' },
        { text: 'Amount', style: 'tableHeader', alignment: 'right' },
        { text: 'Payment', style: 'tableHeader', alignment: 'right' },
        { text: 'Type', style: 'tableHeader' },
        { text: 'Balance', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      // table content
      tableBody.push(...data.items.map(x => [
        { text: formatDate(x.paymentOn), style: 'tableData' },
        { text: emptyIfNull(x.branchName), style: 'tableData' },
        { text: emptyIfNull(x.customerName), style: 'tableData' },
        { text: emptyIfNull(x.invoiceNumber), style: 'tableData' },
        { text: formatNumber(x.totalAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.paidAmount), style: 'tableData', alignment: 'right' },
        { text: emptyIfNull(x.paymentTypeName), style: 'tableData' },
        { text: formatNumber(x.balanceAmount), style: 'tableData', alignment: 'right' },
      ]));

      let grandTotal = {
        totalAmount: Enumerable
          .from(data.items)
          .groupBy(x => x.invoiceNumber)
          .select(x => x.first().totalAmount)
          .sum(x => x),
        paidAmount:  Enumerable
          .from(data.items)
          .sum(x => x.paidAmount),
        balanceAmount: Enumerable
          .from(data.items)
          .groupBy(x => x.invoiceNumber)
          .select(x => x.min(o => o.balanceAmount))
          .sum(x => x),
      };

      // table footer
      tableBody.push([
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "Grand Total", style: "tableHeader" },
        { text: formatNumber(grandTotal.paidAmount), style: "tableData", alignment: "right" },
        { text: formatNumber(grandTotal.paidAmount), style: "tableData", alignment: "right" },
        { text: "", style: "tableData" },
        { text: formatNumber(grandTotal.balanceAmount), style: "tableData", alignment: "right" },
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
          widths: ['auto', 'auto','auto', '*', 'auto', 'auto', 'auto', 'auto'],
          body: tableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return Promise.resolve(body);
  }
}
