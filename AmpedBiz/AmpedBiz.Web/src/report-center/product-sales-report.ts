import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { Report, Content } from './report';
import { OrderStatus } from '../common/models/order';

export interface ProductSalesReportModel {
  branchName?: string;
  supplierName?: string;
  categoryName?: string;
  productName?: string;
  fromDate?: Date;
  toDate?: Date;
  items: ProductSalesReportItemModel[];
}

export interface ProductSalesReportItemModel {
  completedOn?: Date;
  branchName?: string;
  supplierName?: string;
  categoryName?: string;
  productName?: string;
  quantityUnit?: string;
  quantityValue?: number;
  unitPriceAmount?: number;
  discountAmount?: number;
  extendedPriceAmount?: number;
  totalPriceAmount?: number;
}

@autoinject
export class ProductSalesReport extends Report<ProductSalesReportModel> {

  protected async buildBody(data: ProductSalesReportModel): Promise<any[] | Content[]> {
    let orderTableBody: any[] = [
      [
        { text: 'Date', style: 'tableHeader' },
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Supplier', style: 'tableHeader' },
        { text: 'Category', style: 'tableHeader' },
        { text: 'Product', style: 'tableHeader' },
        { text: 'Unit', style: 'tableHeader' },
        { text: 'Price', style: 'tableHeader', alignment: 'right' },
        { text: 'Discount', style: 'tableHeader', alignment: 'right' },
        { text: 'Total', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        orderTableBody.push([
          { text: formatDate(x.completedOn), style: 'tableData' },
          { text: emptyIfNull(x.branchName), style: 'tableData' },
          { text: emptyIfNull(x.supplierName), style: 'tableData' },
          { text: emptyIfNull(x.categoryName), style: 'tableData' },
          { text: emptyIfNull(x.productName), style: 'tableData' },
          { text: emptyIfNull(x.quantityUnit), style: 'tableData' },
          { text: formatNumber(x.unitPriceAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.discountAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.totalPriceAmount), style: 'tableData', alignment: 'right' },
        ])
      );
    }

    let body = [
      {
        text: 'Customer Orders',
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
                      { text: 'Supplier: ', style: 'label' },
                      { text: emptyIfNull(data.supplierName), style: 'value' }
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
                      { text: 'Categoy: ', style: 'label' },
                      { text: emptyIfNull(data.categoryName), style: 'value' }
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

        ]
      },
      {
        text: ' ',
        style: 'separator'
      },
      {
        table: {
          headerRows: 1,
          widths: ['auto', 'auto', 'auto', 'auto', '*', 'auto', 'auto', 'auto', 'auto'],
          body: orderTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return Promise.resolve(body);
  }
}