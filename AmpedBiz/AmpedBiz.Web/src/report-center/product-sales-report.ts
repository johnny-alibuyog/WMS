import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { Report, Content } from './report';
import { OrderStatus } from '../common/models/order';
import { SupplierCreate } from '../admin/suppliers/supplier-create';

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

  public constructor() {
    super();
    this.option.pageOrientation = 'landscape';
  }

  protected async buildBody(data: ProductSalesReportModel): Promise<any[] | Content[]> {
    let orderTableBody: any[] = [
      [
        { text: 'Date', style: 'tableHeader' },
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Supplier', style: 'tableHeader' },
        { text: 'Category', style: 'tableHeader' },
        { text: 'Product', style: 'tableHeader' },
        { text: 'Quantity', style: 'tableHeader', alignment: 'right' },
        { text: 'Price', style: 'tableHeader', alignment: 'right' },
        { text: 'Discount', style: 'tableHeader', alignment: 'right' },
        { text: 'Total', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      // table body
      orderTableBody.push(...data.items.map(x => [
        { text: formatDate(x.completedOn), style: 'tableData' },
        { text: emptyIfNull(x.branchName), style: 'tableData' },
        { text: emptyIfNull(x.supplierName), style: 'tableData' },
        { text: emptyIfNull(x.categoryName), style: 'tableData' },
        { text: emptyIfNull(x.productName), style: 'tableData' },
        { text: `${emptyIfNull(x.quantityUnit)} ${formatNumber(x.quantityValue, '0,0')}`, style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.unitPriceAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.discountAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.totalPriceAmount), style: 'tableData', alignment: 'right' },
      ]));

      // table footer
      orderTableBody.push([
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "Grand Total", style: "tableHeader" },
        { text: formatNumber(data.items.map(o => o.unitPriceAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
        { text: formatNumber(data.items.map(o => o.discountAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
        { text: formatNumber(data.items.map(o => o.totalPriceAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
      ]);
    }

    let body = [
      {
        text: 'Product Sales',
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
                    [
                      { text: 'Date: ', style: 'label' },
                      { text: formatDate(data.fromDate) + ' - ' + formatDate(data.toDate), style: 'value' }
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