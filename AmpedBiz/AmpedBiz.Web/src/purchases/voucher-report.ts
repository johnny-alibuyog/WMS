import { autoinject } from 'aurelia-framework';
import { ReportBuilder, Report, DocumentDefinition } from '../services/report-builder';
import { Lookup } from '../common/custom_types/lookup';
import { Address } from '../common/models/Address';
import { Voucher } from '../common/models/purchase-order';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import * as moment from 'moment';

@autoinject
export class VoucherReport implements Report<Voucher> {
  private readonly _reportBuilder: ReportBuilder;

  constructor(reportBuilder: ReportBuilder) {
    this._reportBuilder = reportBuilder;
  }

  public show(data: Voucher): void {
    var document = this.buildDocument(data);
    this._reportBuilder.build({ title: 'Voucher', document: document });
  }

  private buildDocument(data: Voucher): DocumentDefinition {
    let productTableBody: any[] = [
      [
        { text: 'Product', style: 'tableHeader' },
        { text: 'Quantity', style: 'tableHeader', alignment: 'right' },
        { text: 'Unit Cost', style: 'tableHeader', alignment: 'right' },
        { text: 'Total Cost', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        productTableBody.push([
          { text: x.product && x.product.name || '', style: 'tableData' },
          { text: formatNumber(x.quantityValue, "0"), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.unitCostAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.totalCostAmount), style: 'tableData', alignment: 'right' },
        ])
      );
    }

    return <DocumentDefinition>{
      content:
      [
        {
          text: 'Voucher',
          style: 'title'
        },
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
                    { text: 'Payment Type: ', style: 'label' },
                    { text: emptyIfNull(data.paymentTypeName), style: 'value' }
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
          text: 'Products',
          style: 'header'
        },
        {
          style: 'tableExample',
          table: {
            headerRows: 1,
            widths: ['*', 'auto', 'auto', 'auto'],
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
                  [
                    { text: 'Tax: ', style: 'label' },
                    { text: formatNumber(data.taxAmount), style: 'value', alignment: 'right' }
                  ],
                  [
                    { text: 'Freight: ', style: 'label' },
                    { text: formatNumber(data.shippingFeeAmount), style: 'value', alignment: 'right' }
                  ],
                  [
                    { text: 'Sub Total: ', style: 'label' },
                    { text: formatNumber(data.subTotalAmount), style: 'value', alignment: 'right' }
                  ],
                  [
                    { text: 'Total: ', style: 'label' },
                    { text: formatNumber(data.totalAmount), style: 'value', alignment: 'right' }
                  ],
                ],
              }
            },
          ]
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