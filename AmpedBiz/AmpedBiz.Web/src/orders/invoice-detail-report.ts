import {autoinject} from 'aurelia-framework';
import {ReportBuilder, Report} from '../services/report-builder';
import {Lookup} from '../common/custom_types/lookup';
import {Address} from '../common/models/Address';
import {OrderItem} from '../common/models/order';
import {formatDate, formatNumber} from '../services/formaters';
import * as moment from 'moment';

@autoinject
export class InvoiceDetailReport implements Report<InvoiceDetail> {
  private readonly _reportBuilder: ReportBuilder;

  constructor(reportBuilder: ReportBuilder) {
    this._reportBuilder = reportBuilder;
  }

  public show(data: InvoiceDetail): void {
    data = genDummy();

    let productTableBody: any[] = [
      [
        { text: 'Product', style: 'tableHeader' },
        { text: 'Quantity', style: 'tableHeader', alignment: 'right' },
        { text: 'Unit Price', style: 'tableHeader', alignment: 'right' },
        { text: 'Discount', style: 'tableHeader', alignment: 'right' },
        { text: 'Price', style: 'tableHeader', alignment: 'right' }
      ],
    ];

    if (data && data.orderItems && data.orderItems.length > 0) {
      data.orderItems.forEach(x =>
        productTableBody.push([
          { text: x.product && x.product.name || '', style: 'tableData' },
          { text: formatNumber(x.quantityValue, "0"), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.unitPriceAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.discountAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.total), style: 'tableData', alignment: 'right' },
        ])
      );
    }

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
                    { text: formatDate(data.invoicedOn), style: 'value' }
                  ],
                  [
                    { text: 'Invoiced By: ', style: 'label' },
                    { text: data.invoicedByName, style: 'value' }
                  ],
                  [
                    { text: 'Payment Type: ', style: 'label' },
                    { text: data.paymentTypeName, style: 'value' }
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
                    { text: 'Branch Name: ', style: 'label' },
                    { text: data.branchName, style: 'value' }
                  ],
                  [
                    { text: 'Ordered On: ', style: 'label' },
                    { text: formatDate(data.orderedOn), style: 'value' }
                  ],
                  [
                    { text: 'Staff: ', style: 'label' },
                    { text: data.orderedFromName, style: 'value' }
                  ],
                  [
                    { text: 'Delivery Address: ', style: 'label' },
                    { text: data.shippingAddress, style: 'value' }
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
            widths: ['*', 'auto', 'auto', 'auto', 'auto'],
            body: productTableBody
          },
          layout: 'lightHorizontalLines'
        },
        {
          columns:
          [
            { text: "" },
            {
              style: 'tablePlain',
              layout: 'noBorders',
              table:
              {
                body:
                [
                  [
                    { text: 'Sub Total: ', style: 'label' },
                    { text: formatNumber(data.subTotal), style: 'value', alignment: 'right' }
                  ],
                  [
                    { text: 'Tax: ', style: 'label' },
                    { text: formatNumber(data.taxes), style: 'value', alignment: 'right' }
                  ],
                  [
                    { text: 'Freight: ', style: 'label' },
                    { text: formatNumber(data.freight), style: 'value', alignment: 'right' }
                  ],
                  [
                    { text: 'Total: ', style: 'label' },
                    { text: formatNumber(data.total), style: 'value', alignment: 'right' }
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
  orderedFromName?: string;
  shippedOn?: Date;
  shippedByName?: string;
  shippingAddress?: Address;

  orderItems?: OrderItem[];

  subTotal?: number;
  taxes?: number;
  freight?: number;
  total?: number;
}

let genDummy = (): InvoiceDetail => <InvoiceDetail>{
  customerName: 'customer name',
  invoiceNumber: 'invoice #',
  invoicedOn: moment(new Date())
    .subtract(1, 'day')
    .toDate(),
  invoicedByName: 'invoiced by',
  pricingSchemeName: 'pricing scheme',
  paymentTypeName: 'payment type',
  branchName: 'branch name',
  orderedOn: moment(new Date())
    .subtract(2, 'day')
    .toDate(),
  orderedFromName: 'ordered from',
  shippedOn: new Date(),
  shippedByName: 'shipped by',
  shippingAddress: 'shipping address',
  orderItems: [
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, total: 90 },
  ],
  subTotal: 100,
  taxes: 1,
  freight: 1,
  total: 90
};