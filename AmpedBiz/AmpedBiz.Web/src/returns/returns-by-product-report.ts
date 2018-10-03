import { autoinject } from 'aurelia-framework';
import { formatNumber, emptyIfNull } from '../services/formaters';
import * as Enumerable from 'linq';
import { Report, Content } from '../report-center/report';

export interface ReturnsByProductReportModel {
  branchName?: string;
  productName?: string;
  items: ReturnsByProductReportItem[];
}

export interface ReturnsByProductReportItem {
  id?: string;
  branchName?: string;
  productName?: string;
  productCode?: string;
  quantityUnit?: string;
  quantityValue?: number;
  returnedAmount?: number;
}

@autoinject
export class ReturnsByProductReport extends Report<ReturnsByProductReportModel> {

  protected buildBody(data: ReturnsByProductReportModel): Promise<any[] | Content[]> {
    // table header
    let orderTableBody: any[] = [
      [
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Product', style: 'tableHeader' },
        { text: 'Quantity', style: 'tableHeader', alignment: 'right' },
        { text: 'Returned Amount', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    // table data
    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        orderTableBody.push([
          { text: emptyIfNull(x.branchName), style: 'tableData' },
          { text: emptyIfNull(x.productName), style: 'tableData' },
          { text: `${formatNumber(x.quantityValue)} ${emptyIfNull(x.quantityUnit)}`, style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.returnedAmount), style: 'tableData', alignment: 'right' },
        ])
      );
    }

    let totalRetunedAmount = Enumerable.from(data.items).sum(x => x.returnedAmount);

    // total
    orderTableBody.push([
      { text: '', style: 'tableData' },
      { text: '', style: 'tableData' },
      { text: 'Total:', style: 'label' },
      { text: formatNumber(totalRetunedAmount), style: 'tableData', alignment: 'right' },
    ])

    let body = [
      {
        text: 'Returns By Product',
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
              { text: emptyIfNull(data.productName), style: 'value' }
            ],
          ],
        },
        style: 'tablePlain',
        layout: 'noBorders',
      },
      {
        table: {
          headerRows: 1,
          widths: ['*', 'auto', 'auto', 'auto'],
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

