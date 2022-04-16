import { autoinject } from 'aurelia-framework';
import { formatNumber, emptyIfNull } from '../services/formaters';
import Enumerable from 'linq';
import { Report, Content } from '../report-center/report';

export interface ReturnByCustomerReportModel {
  branchName?: string;
  customerName?: string;
  items: ReturnByCustomerReportItem[];
}

export interface ReturnByCustomerReportItem {
  id?: string;
  branchName?: string;
  customerName?: string;
  returnedAmount?: number;
}

@autoinject
export class ReturnByCustomerReport extends Report<ReturnByCustomerReportModel> {

  protected buildBody(data: ReturnByCustomerReportModel): Promise<any[] | Content[]> {
    // table header
    let orderTableBody: any[] = [
      [
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Customer', style: 'tableHeader' },
        { text: 'Returned Amount', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    // table data
    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        orderTableBody.push([
          { text: emptyIfNull(x.branchName), style: 'tableData' },
          { text: emptyIfNull(x.customerName), style: 'tableData' },
          { text: formatNumber(x.returnedAmount), style: 'tableData', alignment: 'right' },
        ])
      );
    }

    let totalRetunedAmount = Enumerable.from(data.items).sum(x => x.returnedAmount);

    // total
    orderTableBody.push([
      { text: '', style: 'tableData' },
      { text: 'Total:', style: 'label' },
      { text: formatNumber(totalRetunedAmount), style: 'tableData', alignment: 'right' },
    ])

    let body = [
      {
        text: 'Returns By Customer',
        style: 'title'
      },
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
        table: {
          headerRows: 1,
          widths: ['*', 'auto', 'auto'],
          // widths: ['*', 'auto', 'auto', 'auto', 'auto'],
          body: orderTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      }
    ];

    return  Promise.resolve(body);
  }
}

