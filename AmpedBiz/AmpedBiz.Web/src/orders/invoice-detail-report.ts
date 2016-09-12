import {autoinject} from 'aurelia-framework';
import {ReportBuilder, Report} from '../services/report-builder';
import {Lookup} from '../common/custom_types/lookup';
import {Address} from '../common/models/Address';
import {OrderItem} from '../common/models/order';

@autoinject
export class InvoiceDetailReport implements Report<InvoiceDetail> {
  private readonly _reportBuilder: ReportBuilder;

  constructor(reportBuilder: ReportBuilder) {
    this._reportBuilder = reportBuilder;
  }

  public show(data: InvoiceDetail): void {
    data = {
      customerName: 'euphrates'
    };

    this._reportBuilder.build({
      content:
      [
        {
          text: 'Invoice',
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
                    { text: 'Customer: ', style: 'label' },
                    { text: data.customerName, style: 'value' }
                  ],
                  [
                    { text: 'Invoice Number: ', style: 'label' },
                    { text: data.invoiceNumber || '', style: 'value' }
                  ],
                  [
                    { text: 'Inviced On: ', style: 'label' },
                    { text: data.invoicedOn, style: 'value' }
                  ],
                  [
                    { text: 'Pricing Scheme: ', style: 'label' },
                    { text: data.pricingSchemeName, style: 'value' }
                  ],
                ],
              }
            },
            /*
            {
              style: 'tablePlain',
              layout: 'noBorders',
              table:
              {
                body:
                [
                  [
                    { text: 'Customer: ', style: 'label' },
                    { text: data.customerName, style: 'value' }
                  ],
                  [
                    { text: 'Invoice Number: ', style: 'label' },
                    { text: data.invoiceNumber, style: 'value' }
                  ],
                  [
                    { text: 'Customer: ', style: 'label' },
                    { text: data.customerName, style: 'value' }
                  ],
                  [
                    { text: 'Customer: ', style: 'label' },
                    { text: data.customerName, style: 'value' }
                  ],
                ],
              }
            },
            */
          ]
        },

      ],
      styles:
      {
        title:
        {
          fontSize: 22,
          bold: true,
          margin: [0, 0, 0, 10]
        },
        header:
        {
          fontSize: 18,
          bold: true,
          margin: [0, 0, 0, 10]
        },
        label:
        {
          fontSize: 12,
          bold: true,
        },
        value:
        {
          fontSize: 12,
          color: 'gray',
        },
        tablePlain:
        {
          margin: [0, 0, 0, 0]
        },
        tableHeader: {
          bold: true,
          fontSize: 13,
          color: 'black'
        }
      },

    });
  }
}

export interface InvoiceDetail {
  customerName?: string;
  invoiceNumber?: string;
  invoicedOn?: Date;
  invoicedByName?: string;
  pricingSchemeName?: string;
  paymentTypeName?: string;
  branchName?: string;
  orderedOn?: Date;
  orderedToName?: string;
  shippedOn?: Date;
  shippedByName?: string;
  shippingAddress?: Address;
  orderItems?: OrderItem[];
  subTotal?: number;
  taxes?: number;
  freight?: number;
  total?: number;
}