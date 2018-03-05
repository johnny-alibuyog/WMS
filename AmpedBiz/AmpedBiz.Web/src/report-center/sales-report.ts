import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { Report, Content } from './report';

export interface SalesReportModel {
  productName?: string;
  date?: Date;
  items?: SalesReportModelItem[];
}

export interface SalesReportModelItem {
  productId?: string;
  productName?: string;
  totalSoldItems?: string;
  totalSoldPrice?: string;
  details?: SalesReportPageDetailModelItem[];
}

export interface SalesReportPageDetailModelItem {
  customerName?: string;
  invoiceNumber?: string;
  soldItems?: string;
  soldPrice?: string;
}

@autoinject
export class SalesReport extends Report<SalesReportModel> {

  protected buildBody(data: SalesReportModel): Promise<any[] | Content[]> {
    
    var body = <Content[]>[
      {
        text: 'Customer Sales On ' + formatDate(data.date),
        style: 'title'
      },
      {
        text: ' ',
        style: 'spacer'
      },
      ...this.buildItems(data)
    ];

    return Promise.resolve(body);
  }

  private buildItems(data: SalesReportModel): any[] {
    var items = data.items.map(x => {
      return [
        {
          text: [
            { text: 'Product: ', style: 'itemHeaderLabel' },
            { text: x.productName, style: 'itemHeaderInfo' },
          ]
        },
        this.buildDetails(x)
      ];
    })

    return items;
  }

  private buildDetails(data: SalesReportModelItem): any {
    // table heading
    let detailsTableBody: any[] = [
      [
        { text: 'Customer', style: 'tableHeader' },
        { text: 'Invoice', style: 'tableHeader' },
        { text: 'Items', style: 'tableHeader', alignment: 'right' },
        { text: 'Price', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    // items
    if (data && data.details && data.details.length > 0) {
      data.details.forEach(x =>
        detailsTableBody.push([
          { text: emptyIfNull(x.customerName), style: 'tableData' },
          { text: emptyIfNull(x.invoiceNumber), style: 'tableData' },
          { text: emptyIfNull(x.soldItems), style: 'tableData' },
          { text: emptyIfNull(x.soldPrice), style: 'tableData', alignment: 'right' },
        ])
      );
    }

    // summary
    detailsTableBody.push([
      { text: '', style: 'tableData' },
      { text: 'Total:', style: 'label' },
      { text: data.totalSoldItems, style: 'tableData', alignment: 'right' },
      { text: data.totalSoldPrice, style: 'tableData', alignment: 'right' },
    ]);

    return {
      table: {
        headerRows: 1,
        widths: ['*', 'auto', 'auto', 'auto'],
        body: detailsTableBody
      },
      layout: 'lightHorizontalLines',
      style: 'tableExample',
    };
  }
}