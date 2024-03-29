import { autoinject } from 'aurelia-framework';
import { formatNumber, emptyIfNull } from '../../services/formaters';
import { Report, Content } from '../report';
import Enumerable from 'linq';

export interface ProductListingReportModel {
  branchName?: string;
  categoryName?: string;
  productName?: string;
  items: ProductListingReportItemModel[];
}

export interface ProductListingReportItemModel {
  branchName?: string;
  categoryName?: string;
  productName?: string;
  quantityUnit?: string;
  onHandValue?: number;
  availableValue?: number;
  basePriceAmount?: number;
  wholesalePriceAmount?: number;
  retailPriceAmount?: number;
  suggestedRetailPriceAmount?: number;
}

@autoinject
export class ProductListingReport extends Report<ProductListingReportModel> {

  public constructor() {
    super();

    this.option.pageOrientation = 'landscape';
  }

  protected async buildBody(data: ProductListingReportModel): Promise<any[] | Content[]> {
    let productTableBody: any[] = [
      [
        { text: 'Branch', style: 'tableHeader' },
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
      productTableBody.push(...data.items.map(x => [
        { text: emptyIfNull(x.branchName), style: 'tableData' },
        { text: emptyIfNull(x.categoryName), style: 'tableData' },
        { text: emptyIfNull(x.productName), style: 'tableData' },
        { text: emptyIfNull(x.quantityUnit), style: 'tableData' },
        { text: formatNumber(x.onHandValue), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.availableValue), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.basePriceAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.wholesalePriceAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.retailPriceAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.suggestedRetailPriceAmount), style: 'tableData', alignment: 'right' },
      ]));
    }

    let total = {
      basePriceAmount: Enumerable.from(data.items).select(x => x.onHandValue * x.basePriceAmount).sum(),
      wholesalePriceAmount: Enumerable.from(data.items).select(x => x.onHandValue * x.wholesalePriceAmount).sum(),
      retailPriceAmount: Enumerable.from(data.items).select(x => x.onHandValue * x.wholesalePriceAmount).sum(),
      suggestedRetailPriceAmount: Enumerable.from(data.items).select(x => x.onHandValue * x.suggestedRetailPriceAmount).sum(),
    };

    // total
    productTableBody.push([
      { text: '', style: 'tableData' },
      { text: '', style: 'tableData' },
      { text: '', style: 'tableData' },
      { text: '', style: 'tableData' },
      { text: '', style: 'tableData' },
      { text: 'Total:', style: 'label' },
      { text: formatNumber(total.basePriceAmount), style: 'tableData', alignment: 'right' },
      { text: formatNumber(total.wholesalePriceAmount), style: 'tableData', alignment: 'right' },
      { text: formatNumber(total.retailPriceAmount), style: 'tableData', alignment: 'right' },
      { text: formatNumber(total.suggestedRetailPriceAmount), style: 'tableData', alignment: 'right' },
    ]);

    let body = [
      {
        text: 'Product Listing',
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
                      { text: 'Categoy: ', style: 'label' },
                      { text: emptyIfNull(data.categoryName), style: 'value' }
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
          widths: ['auto', 'auto', '*', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto',],
          body: productTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return Promise.resolve(body);
  }
}
