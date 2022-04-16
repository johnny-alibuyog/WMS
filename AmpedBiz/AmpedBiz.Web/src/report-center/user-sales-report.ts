import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { Report, Content } from './report';
import Enumerable from 'linq';

export type UserSalesReportModel = {
  fromDate?: Date;
  toDate?: Date;
  totalSales: () => number;
  items: UserSalesReportModelItem[];
}

export type UserSalesReportModelItem = {
  userFullname?: string;
  salesAmount?: number;
  returnsAmount?: number;
  totalSalesAmount?: number;
}

@autoinject
export class UserSalesReport extends Report<UserSalesReportModel> {

  protected buildBody(data: UserSalesReportModel): Promise<any[] | Content[]> {
    data.totalSales = () => {
      return Enumerable
        .from(data.items)
        .sum(x => x.totalSalesAmount);
    };

    // header
    let uomTableBody: any[] = [
      [
        { text: 'User', style: 'tableHeader' },
        { text: 'Sales', style: 'tableHeader', alignment: 'right' },
        { text: 'Returns', style: 'tableHeader', alignment: 'right' },
        { text: 'Total Sales', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    // items
    if (data && data.items && data.items.length > 0) {
      data.items.forEach(item => {
        uomTableBody.push([
          { text: emptyIfNull(item.userFullname), style: 'tableData' },
          { text: formatNumber(item.salesAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(item.returnsAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(item.totalSalesAmount), style: 'tableData', alignment: 'right' },
        ]);
      });
    }

    let body = <Content[]>[
      {
        text: 'User Sales Report',
        style: 'title'
      },
      {
        table:
        {
          body:
            [
              [
                { text: 'From Date: ', style: 'label' },
                { text: formatDate(data.fromDate), style: 'value' }
              ],
              [
                { text: 'To Date: ', style: 'label' },
                { text: formatDate(data.toDate), style: 'value' }
              ],
              [
                { text: 'Total Sales: ', style: 'label' },
                { text: formatNumber(data.totalSales()), style: 'value' }
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
          body: uomTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return Promise.resolve(body);
  }
}
