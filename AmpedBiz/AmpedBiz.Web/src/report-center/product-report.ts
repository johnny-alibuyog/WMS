import { autoinject } from 'aurelia-framework';
import { formatNumber, emptyIfNull } from '../services/formaters';
import { Report, Content } from './report';

export interface ProductReportModel {
  productName?: string;
  categoryName?: string;
  measureType?: string;
  supplierName?: string;
  items: ProductReportModelItem[];
}

export interface ProductReportModelItem {
  id?: string;
  productName?: string;
  categoryName?: string;
  supplierName?: string;
  onHandUnit?: string;
  onHandValue?: number;
  basePriceAmount?: number;
  wholesalePriceAmount?: number;
  retailPriceAmount?: number;
  totalBasePriceAmount?: number;
  totalWholesalePriceAmount?: number;
  totalRetailPriceAmount?: number;
}

@autoinject
export class ProductReport extends Report<ProductReportModel> {

  protected buildBody(data: ProductReportModel): Promise<any[] | Content[]> {
    // table header
    let orderTableBody: any[] = [
      [
        { text: 'Product', style: 'tableHeader' },
        // { text: 'Category', style: 'tableHeader' },
        // { text: 'Supplier', style: 'tableHeader' },
        { text: 'On-Hand', style: 'tableHeader', alignment: 'right' },
        // { text: 'Base Price', style: 'tableHeader', alignment: 'right' },
        // { text: 'Retail Price', style: 'tableHeader', alignment: 'right' },
        // { text: 'Wholesale Price', style: 'tableHeader', alignment: 'right' },
        { text: 'Base Total', style: 'tableHeader', alignment: 'right' },
        { text: 'Retail Total', style: 'tableHeader', alignment: 'right' },
        { text: 'Wholesale Total', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    // table data
    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        orderTableBody.push([
          { text: emptyIfNull(x.productName), style: 'tableData' },
          // { text: emptyIfNull(x.categoryName), style: 'tableData' },
          // { text: emptyIfNull(x.supplierName), style: 'tableData' },
          { text: formatNumber(x.onHandValue) + ' ' +  emptyIfNull(x.onHandUnit), style: 'tableData', alignment: 'right' },
          // { text: formatNumber(x.basePriceAmount), style: 'tableData', alignment: 'right' },
          // { text: formatNumber(x.wholesalePriceAmount), style: 'tableData', alignment: 'right' },
          // { text: formatNumber(x.retailPriceAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.totalBasePriceAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.totalWholesalePriceAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.totalRetailPriceAmount), style: 'tableData', alignment: 'right' },
        ])
      );
    }

    var sum = (items: number[]) => items.reduce((x, y) => (x || 0) + (y || 0), 0);

    var total = {
      get basePriceAmount(): number {
        return sum(data.items.map(x => x.totalBasePriceAmount));
      },
      get wholesalePriceAmount(): number {
        return sum(data.items.map(x => x.totalWholesalePriceAmount));
      },
      get retailPriceAmount(): number {
        return sum(data.items.map(x => x.totalRetailPriceAmount));
      }
    };

    // total
    orderTableBody.push([
      // { text: '', style: 'tableData' },
      // { text: '', style: 'tableData' },
      { text: '', style: 'tableData' },
      { text: 'Total:', style: 'label' },
      { text: formatNumber(total.basePriceAmount), style: 'tableData', alignment: 'right' },
      { text: formatNumber(total.wholesalePriceAmount), style: 'tableData', alignment: 'right' },
      { text: formatNumber(total.retailPriceAmount), style: 'tableData', alignment: 'right' },
    ])

    let body = [
      {
        text: 'Product Listings',
        style: 'title'
      },
      {
        table:
        {
          body:
          [
            // [
            //   { text: 'Product: ', style: 'label' },
            //   { text: emptyIfNull(data.productName), style: 'value' }
            // ],
            [
              { text: 'Category: ', style: 'label' },
              { text: emptyIfNull(data.categoryName), style: 'value' }
            ],
            [
              { text: 'Measure Type: ', style: 'label' },
              { text: emptyIfNull(data.measureType), style: 'value' }
            ],
            [
              { text: 'Supplier: ', style: 'label' },
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
          widths: ['*', 'auto', 'auto', 'auto', 'auto'],
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

