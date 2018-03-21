import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { Report, Content } from './report';
import { OrderStatus } from '../common/models/order';

export interface ProductListingReportModel {
  branchName?: string;
  supplierName?: string;
  categoryName?: string;
  productName?: string;
  items: ProductListingReportItemModel[];
}

export interface ProductListingReportItemModel {
  branchName?: string;
  supplierName?: string;
  categoryName?: string;
  productName?: string;
  quantityUnit?: string;
  onHandValue?: number;
  availableValue?: number;
  basePriceAmount?: number;
  wholeSalePriceAmount?: number;
  retailPriceAmount?: number;
  suggestedRetailPriceAmount?: number;
}

@autoinject
export class ProductListingReport extends Report<ProductListingReportModel> {

  protected async buildBody(data: ProductListingReportModel): Promise<any[] | Content[]> {
    let orderTableBody: any[] = [
      [
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Supplier', style: 'tableHeader' },
        { text: 'Category', style: 'tableHeader' },
        { text: 'Product', style: 'tableHeader' },
        { text: 'Unit', style: 'tableHeader' },
        { text: 'On Hand', style: 'tableHeader', alignment: 'right' },
        { text: 'Available', style: 'tableHeader', alignment: 'right' },
        { text: 'Base Price', style: 'tableHeader', alignment: 'right' },
        { text: 'Wholesale Price', style: 'tableHeader', alignment: 'right' },
        { text: 'Retail Price', style: 'tableHeader', alignment: 'right' },
        { text: 'SRP', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        orderTableBody.push([
          { text: emptyIfNull(x.branchName), style: 'tableData' },
          { text: emptyIfNull(x.supplierName), style: 'tableData' },
          { text: emptyIfNull(x.categoryName), style: 'tableData' },
          { text: emptyIfNull(x.productName), style: 'tableData' },
          { text: emptyIfNull(x.quantityUnit), style: 'tableData' },
          { text: formatNumber(x.onHandValue), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.availableValue), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.basePriceAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.wholeSalePriceAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.retailPriceAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.suggestedRetailPriceAmount), style: 'tableData', alignment: 'right' },
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
          widths: ['auto', 'auto', 'auto', '*', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', ],
          body: orderTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return Promise.resolve(body);
  }
}