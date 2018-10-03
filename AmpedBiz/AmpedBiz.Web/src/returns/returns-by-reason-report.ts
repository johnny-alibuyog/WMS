import { autoinject } from 'aurelia-framework';
import { formatNumber, emptyIfNull } from '../services/formaters';
import * as Enumerable from 'linq';
import { Report, Content } from '../report-center/report';

export interface ReturnsByReasonReportModel {
  branchName?: string;
  reasonName?: string;
  items: ReturnsByReasonReportItem[];
}

export interface ReturnsByReasonReportItem {
  id?: string;
  branchName?: string;
  reasonName?: string;
  returnedAmount?: number;
}

@autoinject
export class ReturnsByReasonReport extends Report<ReturnsByReasonReportModel> {

  protected buildBody(data: ReturnsByReasonReportModel): Promise<any[] | Content[]> {
    // table header
    let orderTableBody: any[] = [
      [
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Reason', style: 'tableHeader' },
        { text: 'Returned Amount', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    // table data
    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        orderTableBody.push([
          { text: emptyIfNull(x.branchName), style: 'tableData' },
          { text: emptyIfNull(x.reasonName), style: 'tableData' },
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
        text: 'Returns By Reason',
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
                { text: 'Product: ', style: 'label' },
                { text: emptyIfNull(data.reasonName), style: 'value' }
              ],
            ],
        },
        style: 'tablePlain',
        layout: 'noBorders',
      },
      {
        table: {
          headerRows: 1,
          widths: ['auto', '*', 'auto'],
          body: orderTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      }
    ];

    return Promise.resolve(body);
  }
}
