import { autoinject } from 'aurelia-framework';
import { formatNumber, emptyIfNull, formatDate } from '../services/formaters';
import * as Enumerable from 'linq';
import { Report, Content } from './report';

export interface ReturnByCustomerReportModel {
  branchName?: string;
  customerName?: string;
  productName?: string;
  fromDate?: Date;
  toDate?: Date;
  items: ReturnByCustomerReportItem[];
}

export interface ReturnByCustomerReportItem {
  id?: string;
  branchName?: string;
  customerName?: string;
  productName?: string;
  reasonName?: string;
  returnedByName?: string;
  quantiyValue?: number;
  quantiyUnitId?: string;
  returnedOn?: Date;
  returnedAmount?: number;
}

@autoinject
export class ReturnByCustomerReport extends Report<ReturnByCustomerReportModel> {

  public constructor() {
    super();
    this.option.pageOrientation = 'landscape';
  }

  protected buildBody(data: ReturnByCustomerReportModel): Promise<any[] | Content[]> {
    // table header
    let returnTableBody: any[] = [
      [
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Date', style: 'tableHeader' },
        { text: 'ReceivedBy', style: 'tableHeader' },
        { text: 'Customer', style: 'tableHeader' },
        { text: 'Product', style: 'tableHeader' },
        { text: 'Reason', style: 'tableHeader' },
        { text: 'Quantity', style: 'tableHeader', alignment: 'right' },
        { text: 'Amount', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    // table data
    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        returnTableBody.push([
          { text: emptyIfNull(x.branchName), style: 'tableData' },
          { text: formatDate(x.returnedOn), style: 'tableData' },
          { text: emptyIfNull(x.returnedByName), style: 'tableData' },
          { text: emptyIfNull(x.customerName), style: 'tableData' },
          { text: emptyIfNull(x.productName), style: 'tableData' },
          { text: emptyIfNull(x.reasonName), style: 'tableData' },
          { text: `${formatNumber(x.quantiyValue, "0")} ${emptyIfNull(x.quantiyUnitId)}`, style: 'tableData' },
          { text: formatNumber(x.returnedAmount), style: 'tableData', alignment: 'right' },
        ])
      );
    }

    let totalRetunedAmount = Enumerable.from(data.items).sum(x => x.returnedAmount);

    // total
    returnTableBody.push([
      { text: '', style: 'tableData' },
      { text: '', style: 'tableData' },
      { text: '', style: 'tableData' },
      { text: '', style: 'tableData' },
      { text: '', style: 'tableData' },
      { text: '', style: 'tableData' },
      { text: 'Total:', style: 'label' },
      { text: formatNumber(totalRetunedAmount), style: 'tableData', alignment: 'right' },
    ]);

    let body = [
      {
        text: 'Returns Details',
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
                    { text: 'Branch: ', style: 'label' },
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
            table:
            {
              body:
                [
                  [
                    { text: 'Product: ', style: 'label' },
                    { text: emptyIfNull(data.productName), style: 'value' }
                  ],
                  [
                    { text: 'Date: ', style: 'label' },
                    { text: formatDate(data.fromDate) + ' - ' + formatDate(data.toDate), style: 'value' }
                  ],
                ],
            },
            style: 'tablePlain',
            layout: 'noBorders',
          }
        ]
      },
      {
        table: {
          headerRows: 1,
          widths: ['auto', 'auto', 'auto', 'auto', '*', 'auto', 'auto', 'auto'],
          body: returnTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      }
    ];

    return Promise.resolve(body);
  }
}

