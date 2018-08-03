import { emptyIfNull, formatDate, formatNumber } from '../services/formaters';
import { Report, Content } from '../report-center/report';
import { Voucher } from '../common/models/purchase-order';
import { autoinject } from 'aurelia-framework';

@autoinject
export class VoucherReport extends Report<Voucher> {
  protected buildBody(data: Voucher): Promise<any[] | Content[]> {

    let productTableBody: any[] = [
      [
        { text: 'Product', style: 'tableHeader' },
        { text: 'Unit', style: 'tableHeader', alignment: 'right' },
        { text: 'Quantity', style: 'tableHeader', alignment: 'right' },
        { text: 'Unit Cost', style: 'tableHeader', alignment: 'right' },
        { text: 'Total Cost', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      productTableBody.push(...data.items.map(x => [
        { text: x.product && x.product.name || '', style: 'tableData' },
        { text: x.quantity && x.quantity.unit && x.quantity.unit.name || '', style: 'tableData' },
        { text: formatNumber(x.quantity.value, "0"), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.unitCostAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.totalCostAmount), style: 'tableData', alignment: 'right' },
      ]));
    }

    let body = [
      {
        columns:
          [
            {
              style: 'tablePlain',
              layout: 'noBorders',
              table:
              {
                body:
                  [
                    [
                      { text: 'Supplier: ', style: 'label' },
                      { text: emptyIfNull(data.supplierName), style: 'value' }
                    ],
                    [
                      { text: 'Voucher Number: ', style: 'label' },
                      { text: emptyIfNull(data.voucherNumber), style: 'value' }
                    ],
                    [
                      { text: 'Reference Number: ', style: 'label' },
                      { text: emptyIfNull(data.referenceNumber), style: 'value' }
                    ],
                  ],
              }
            },
            {
              style: 'tablePlain',
              layout: 'noBorders',
              table:
              {
                body:
                  [
                    [
                      { text: 'Approved On: ', style: 'label' },
                      { text: formatDate(data.approvedOn), style: 'value' }
                    ],
                    [
                      { text: 'Approved By: ', style: 'label' },
                      { text: emptyIfNull(data.approvedByName), style: 'value' }
                    ],
                  ],
              }
            },
          ]
      },
      {
        text: ' ',
        style: 'spacer'
      },
      {
        style: 'tableExample',
        table: {
          headerRows: 1,
          widths: ['*', 'auto', 'auto', 'auto', 'auto'],
          body: productTableBody
        },
        layout: 'lightHorizontalLines'
      },
      {
        columns:
          [
            { text: "" },
            { text: "" },
            { text: "" },
            {
              style: 'tablePlain',
              layout: 'noBorders',
              table:
              {
                body:
                  [
                    /*
                    [
                      { text: 'Tax: ', style: 'label' },
                      { text: formatNumber(data.taxAmount), style: 'value', alignment: 'right' }
                    ],
                    */
                    [
                      { text: 'Shipping Fee: ', style: 'label' },
                      { text: formatNumber(data.shippingFeeAmount), style: 'value', alignment: 'right' }
                    ],
                    [
                      { text: 'Sub Total: ', style: 'label' },
                      { text: formatNumber(data.subTotalAmount), style: 'value', alignment: 'right' }
                    ],
                    [
                      { text: 'Grand Total: ', style: 'label' },
                      { text: formatNumber(data.totalAmount), style: 'value', alignment: 'right' }
                    ],
                  ],
              }
            },
          ]
      },
    ];

    return Promise.resolve(body);
  }
}