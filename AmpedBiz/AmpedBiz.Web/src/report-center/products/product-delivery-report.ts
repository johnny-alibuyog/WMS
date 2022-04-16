import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../../services/formaters';
import { Report, Content } from '../report';
import Enumerable from 'linq';

export type ProductDeliveryReportModel = {
  branchName?: string;
  categoryName?: string;
  productName?: string;
  fromDate?: Date;
  toDate?: Date;
  items: ProductDeliveryReportItemModel[];
}

export type  ProductDeliveryReportItemModel = {
  branchName?: string;
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
export class ProductDeliveryReport extends Report<ProductDeliveryReportModel> {

  public constructor() {
    super();
    this.option.pageOrientation = 'landscape';
  }

  protected async buildBody(data: ProductDeliveryReportModel): Promise<any[] | Content[]> {
    let deliveryTableBody: any[] = [
      [
        { text: 'Product', style: 'tableHeader' },
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Category', style: 'tableHeader' },
        { text: 'Quantity', style: 'tableHeader', alignment: 'right' },
        { text: 'Price', style: 'tableHeader', alignment: 'right' },
        { text: 'Discount', style: 'tableHeader', alignment: 'right' },
        { text: 'Total', style: 'tableHeader', alignment: 'right' },
        { text: 'SubTotal', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      // table body
      Enumerable
        .from(data.items)
        .orderBy(x => x.productName)
        .groupBy(x => x.productName)
        .forEach(group => {
          let details = group.getSource();
          // details
          deliveryTableBody.push(...Enumerable
            .from(details)
            .select((x, i) => [
              (i === 0) // header
                ? { text: emptyIfNull(x.productName), style: 'tableData', rowSpan: details.length }
                : { text: "", style: "tableData" },
              { text: emptyIfNull(x.branchName), style: 'tableData' },
              { text: emptyIfNull(x.categoryName), style: 'tableData' },
              { text: `${emptyIfNull(x.quantityUnit)} ${formatNumber(x.quantityValue, '0,0')}`, style: 'tableData', alignment: 'right' },
              { text: formatNumber(x.unitPriceAmount), style: 'tableData', alignment: 'right' },
              { text: formatNumber(x.discountAmount), style: 'tableData', alignment: 'right' },
              { text: formatNumber(x.totalPriceAmount), style: 'tableData', alignment: 'right' },
              (i === 0) // subtotal
                ? { text: formatNumber(Enumerable.from(details).sum(o => o.totalPriceAmount)), style: 'tableData', alignment: 'right', rowSpan: details.length }
                : { text: "", style: "tableData" },
            ])
            .toArray()
          );
        });

      // table footer
      deliveryTableBody.push([
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "Grand Total", style: "tableHeader" },
        { text: formatNumber(Enumerable.from(data.items).sum(o => o.totalPriceAmount)), style: "tableData", alignment: "right" },
      ]);
    }

    let body = [
      {
        text: 'Product Delivery',
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
          widths: ['*', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto'],
          body: deliveryTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return Promise.resolve(body);
  }
}
