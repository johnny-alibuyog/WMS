import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { ReportBuilder, Report, DocumentDefinition } from '../services/report-builder';

export interface ProductReportModel {
  productName?: string;
  categoryName?: string;
  supplierName?: string;
  items: ProductReportModelItem[];
}

export interface ProductReportModelItem {
  id?: string;
  productName?: string;
  categoryName?: string;
  supplierName?: string;
  onHandValue?: number;
  basePriceAmount?: number;
  distributorPriceAmount?: number;
  listPriceAmount?: number;
  totalBasePriceAmount?: number;
  totalDistributorPriceAmount?: number;
  totalListPriceAmount?: number;
}

@autoinject
export class ProductReport implements Report<ProductReportModel> {
  private readonly _reportBuilder: ReportBuilder;

  constructor(reportBuilder: ReportBuilder) {
    this._reportBuilder = reportBuilder;
  }

  public show(data: ProductReportModel): void {
    var document = this.buildDocument(data);
    this._reportBuilder.build({ title: 'Products', document: document });
  }

  private buildDocument(data: ProductReportModel): DocumentDefinition {
    // header
    let orderTableBody: any[] = [
      [
        { text: 'Product', style: 'tableHeader' },
        //{ text: 'Category', style: 'tableHeader' },
        //{ text: 'Supplier', style: 'tableHeader' },
        { text: 'On-Hand', style: 'tableHeader', alignment: 'right' },
        { text: 'Base', style: 'tableHeader', alignment: 'right' },
        { text: 'Retail', style: 'tableHeader', alignment: 'right' },
        { text: 'Wholesale', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    // items
    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        orderTableBody.push([
          { text: emptyIfNull(x.productName), style: 'tableData' },
          //{ text: emptyIfNull(x.categoryName), style: 'tableData' },
          //{ text: emptyIfNull(x.supplierName), style: 'tableData' },
          { text: formatNumber(x.onHandValue, "0"), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.basePriceAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.distributorPriceAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.listPriceAmount), style: 'tableData', alignment: 'right' },
        ])
      );
    }

    var sum = (items: number[]) => items.reduce((x, y) => (x || 0) + (y || 0), 0);

    var total = {
      get basePriceAmount(): number {
        return sum(data.items.map(x => x.totalBasePriceAmount));
      },
      get distributorPriceAmount(): number {
        return sum(data.items.map(x => x.totalDistributorPriceAmount));
      },
      get listPriceAmount(): number {
        return sum(data.items.map(x => x.totalListPriceAmount));
      }
    };

    // total
    orderTableBody.push([
      { text: '', style: 'tableData' },
      //{ text: '', style: 'tableData' },
      //{ text: '', style: 'tableData' },
      { text: 'Total:', style: 'label' },
      { text: formatNumber(total.basePriceAmount), style: 'tableData', alignment: 'right' },
      { text: formatNumber(total.distributorPriceAmount), style: 'tableData', alignment: 'right' },
      { text: formatNumber(total.listPriceAmount), style: 'tableData', alignment: 'right' },
    ])

    return <DocumentDefinition>{
      content:
      [
        {
          text: 'Products',
          style: 'title'
        },
        {
          table:
          {
            body:
            [
              [
                { text: 'Branch Name: ', style: 'label' },
                { text: emptyIfNull(data.productName), style: 'value' }
              ],
              [
                { text: 'Customer: ', style: 'label' },
                { text: emptyIfNull(data.categoryName), style: 'value' }
              ],
              [
                { text: 'Pricing: ', style: 'label' },
                { text: emptyIfNull(data.supplierName), style: 'value' }
              ],
            ],
          },
          style: 'tablePlain',
          layout: 'noBorders',
        },
        {
          table: {
            headerRows: 1,
            //widths: ['*', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto'],
            widths: ['*', 'auto', 'auto', 'auto', 'auto'],
            body: orderTableBody
          },
          layout: 'lightHorizontalLines',
          style: 'tableExample',
        },
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
  }
}