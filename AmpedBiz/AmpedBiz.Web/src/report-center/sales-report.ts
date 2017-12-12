import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { ReportBuilder, Report, DocumentDefinition } from '../services/report-builder';

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
export class SalesReport implements Report<SalesReportModel> {
  private readonly _reportBuilder: ReportBuilder;

  constructor(reportBuilder: ReportBuilder) {
    this._reportBuilder = reportBuilder;
  }

  public show(data: SalesReportModel): void {
    var document = this.buildDocument(data);
    this._reportBuilder.build({ title: 'Products', document: document });
  }

  private buildDocument(data: SalesReportModel): DocumentDefinition {
    var document = <DocumentDefinition>{
      content:
        [
          {
            text: 'Sales On ' + formatDate(data.date),
            style: 'title'
          },
          ...this.buildItems(data)
        ],
      styles:
        {
          title:
            {
              fontSize: 28,
              bold: true,
              margin: [0, 0, 0, 10]
            },
          header:
            {
              fontSize: 18,
              bold: true,
              margin: [0, 10, 0, 10]
            },
          itemHeaderLabel:
            {
              fontSize: 10,
              bold: true
            },
          itemHeaderInfo:
            {
              fontSize: 12,
              bold: true,
              italics: true
            },
          label:
            {
              fontSize: 10,
              alignment: 'right',
            },
          value:
            {
              fontSize: 10,
              color: 'gray',
              alignment: 'left',
            },
          tablePlain:
            {
              alignment: 'right',
              margin: [0, 0, 0, 0]
            },
          tableHeader: {
            bold: true,
            fontSize: 10,
            color: 'black'
          },
          tableData: {
            fontSize: 10,
            color: 'gray'
          }
        },
    };

    return document;
  }

  private buildItems(data: SalesReportModel): any[] {
    var items = data.items.map(x => {
      return [
        {
          text: '\n\n'
        },
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