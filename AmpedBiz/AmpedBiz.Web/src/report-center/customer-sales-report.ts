import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { Report, Content } from './report';
import { OrderStatus } from '../common/models/order';

export interface CustomerSalesReportModel {
  branchName?: string;
  customerName?: string;
  fromDate?: Date;
  toDate?: Date;
  items: CustomerSalesReportItemModel[];
}

export interface CustomerSalesReportItemModel {
  completedOn?: Date;
  branchName?: string;
  customerName?: string;
  invoiceNumber?: string;
  totalAmount?: number;
  paidAmount?: number;
  balanceAmount?: number;
}

@autoinject
export class CustomerSalesReport extends Report<CustomerSalesReportModel> {

  protected async buildBody(data: CustomerSalesReportModel): Promise<any[] | Content[]> {
    let orderTableBody: any[] = [
      [
        { text: 'Date', style: 'tableHeader' },
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Customer', style: 'tableHeader' },
        { text: 'Invoice', style: 'tableHeader' },
        { text: 'Total', style: 'tableHeader', alignment: 'right' },
        { text: 'Balance', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        orderTableBody.push([
          { text: formatDate(x.completedOn), style: 'tableData' },
          { text: emptyIfNull(x.branchName), style: 'tableData' },
          { text: emptyIfNull(x.customerName), style: 'tableData' },
          { text: emptyIfNull(x.invoiceNumber), style: 'tableData' },
          { text: formatNumber(x.totalAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.balanceAmount), style: 'tableData', alignment: 'right' },
        ])
      );
    }

    let body = [
      {
        text: 'Customer Orders',
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
          widths: ['auto', 'auto', '*', 'auto', 'auto', 'auto'],
          body: orderTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return Promise.resolve(body);
  }
}