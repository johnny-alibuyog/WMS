import { autoinject } from 'aurelia-framework';
import { ReportBuilder, Report, DocumentDefinition } from '../services/report-builder';
import { Lookup } from '../common/custom_types/lookup';
import { Address } from '../common/models/Address';
import { OrderItem } from '../common/models/order';
import { OrderInvoiceDetail } from '../common/models/order';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import * as moment from 'moment';

@autoinject
export class OrderInvoiceDetailReport implements Report<OrderInvoiceDetail> {
  private readonly _reportBuilder: ReportBuilder;

  constructor(reportBuilder: ReportBuilder) {
    this._reportBuilder = reportBuilder;
  }

  public show(data: OrderInvoiceDetail): void {
    if (data == null)
      data = genDummy();

    var document = this.buildDocument(data);
    this._reportBuilder.build({ title: 'Invoice', document: document });
  }

  private buildDocument(data: OrderInvoiceDetail): DocumentDefinition {
    let productTableBody: any[] = [
      [
        { text: 'Product', style: 'tableHeader' },
        { text: 'Quantity', style: 'tableHeader', alignment: 'right' },
        { text: 'Unit Price', style: 'tableHeader', alignment: 'right' },
        { text: 'Discount', style: 'tableHeader', alignment: 'right' },
        { text: 'Price', style: 'tableHeader', alignment: 'right' }
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        productTableBody.push([
          { text: x.product && x.product.name || '', style: 'tableData' },
          { text: formatNumber(x.quantityValue, "0"), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.unitPriceAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.discountAmount), style: 'tableData', alignment: 'right' },
          { text: formatNumber(x.totalPriceAmount), style: 'tableData', alignment: 'right' },
        ])
      );
    }

    return <DocumentDefinition>{
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
                    { text: emptyIfNull(data.customerName), style: 'value' }
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
                    { text: data.invoicedByName || '', style: 'value' }
                  ],
                  [
                    { text: 'Payment Type: ', style: 'label' },
                    { text: data.paymentTypeName || '', style: 'value' }
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
                    { text: data.branchName || '', style: 'value' }
                  ],
                  [
                    { text: 'Ordered On: ', style: 'label' },
                    { text: formatDate(data.orderedOn), style: 'value' }
                  ],
                  [
                    { text: 'Staff: ', style: 'label' },
                    { text: data.orderedByName || '', style: 'value' }
                  ],
                  [
                    { text: 'Delivery Address: ', style: 'label' },
                    { text: data.shippingAddress || '', style: 'value' }
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
                    { text: 'Discount: ', style: 'label' },
                    { text: formatNumber(data.discountAmount), style: 'value', alignment: 'right' }
                  ],
                  [
                    { text: 'Sub Total: ', style: 'label' },
                    { text: formatNumber(data.subTotalAmount), style: 'value', alignment: 'right' }
                  ],
                  [
                    { text: 'Total: ', style: 'label' },
                    { text: formatNumber(data.totalAmount), style: 'value', alignment: 'right' }
                  ],
                  [
                    { text: 'Returned: ', style: 'label' },
                    { text: formatNumber(data.returnedAmount), style: 'value', alignment: 'right' }
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

let genDummy = (): OrderInvoiceDetail => <OrderInvoiceDetail>{
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
  items: [
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
    { id: '1', product: <Lookup<string>>{ id: '1', name: 'product name' }, quantityValue: 1, discountRate: 10, discountAmount: 100, unitPriceAmount: 100, extendedPriceAmount: 100, totalPriceAmount: 90 },
  ],
  subTotalAmount: 100,
  taxAmount: 1,
  shippingFeeAmount: 1,
  totalPriceAmount: 90
};