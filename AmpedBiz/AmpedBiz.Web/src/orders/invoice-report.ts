import { autoinject } from 'aurelia-framework';
import { ReportBuilder, Report, DocumentDefinition } from '../services/report-builder';
import { Lookup } from '../common/custom_types/lookup';
import { Address } from '../common/models/Address';
import { OrderItem } from '../common/models/order';
import { OrderInvoiceDetail } from '../common/models/order';
import { AuthService } from '../services/auth-service';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import * as moment from 'moment';

@autoinject
export class InvoiceReport implements Report<OrderInvoiceDetail> {
  private readonly _authService: AuthService;
  private readonly _reportBuilder: ReportBuilder;

  constructor(authService: AuthService, reportBuilder: ReportBuilder) {
    this._authService = authService;
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

    let branch = this._authService.user.branch;
    let branchAddress = branch && branch.address && `${branch.address.barangay || ''}, ${branch.address.city || ''}, ${branch.address.province || ''}`;
    let telephoneNumber = branch && branch.contact && branch.contact.landline || '';
    let tinNumber = branch && branch.taxpayerIdentificationNumber || '';

    let pagedTables = [];

    let pageSize = 5;
    let total = data.items.length;

    for (let i = 0; i < total; i += pageSize) {
      let pageItems = data.items.slice(i, i + pageSize);
      let tableBody = [];

      // table header
      tableBody.push([
        { text: 'Product', style: 'tableHeader' },
        { text: 'Quantity', style: 'tableHeader', alignment: 'right' },
        { text: 'Unit Price', style: 'tableHeader', alignment: 'right' },
        { text: 'Discount', style: 'tableHeader', alignment: 'right' },
        { text: 'Price', style: 'tableHeader', alignment: 'right' }
      ]);

      // table content
      tableBody.push(...pageItems.map(x => [
        { text: x.product && x.product.name || '', style: 'tableData' },
        { text: formatNumber(x.quantityValue, "0"), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.unitPriceAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.discountAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.totalPriceAmount), style: 'tableData', alignment: 'right' },
      ]));

      // table footer
      tableBody.push([
        { text: '', style: 'tableData' },
        { text: '', style: 'tableData' },
        { text: formatNumber(pageItems.map(o => o.unitPriceAmount).reduce((pre, cur) => pre + cur)), style: 'tableData', alignment: 'right' },
        { text: formatNumber(pageItems.map(o => o.discountAmount).reduce((pre, cur) => pre + cur)), style: 'tableData', alignment: 'right' },
        { text: formatNumber(pageItems.map(o => o.totalPriceAmount).reduce((pre, cur) => pre + cur)), style: 'tableData', alignment: 'right' },
      ]);

      pagedTables.push({
        style: 'tableExample',
        table: {
          headerRows: 1,
          widths: ['*', 'auto', 'auto', 'auto', 'auto'],
          body: tableBody
        },
        layout: 'lightHorizontalLines',
        pageBreak: 'after'
      });

    }

    return <DocumentDefinition>{
      footer: (currentPage: number, pageCount: number) => [
        {
          text: `Page ${currentPage} of ${pageCount}`,
          style: "footer"
        }
      ],
      content:
      [
        {
          text: branch.description,
          style: 'title'
        },
        {
          text: branchAddress,
          style: 'header3'
        },
        {
          text: `TEL NO. ${telephoneNumber}`,
          style: 'header3'
        },
        {
          text: `TIN ${tinNumber}`,
          style: 'header3'
        },
        {
          text: ' ',
          style: 'spacer'
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
          text: ' ',
          style: 'spacer'
        },
        // {
        //   style: 'tableExample',
        //   table: {
        //     headerRows: 1,
        //     widths: ['*', 'auto', 'auto', 'auto', 'auto'],
        //     body: productTableBody
        //   },
        //   layout: 'lightHorizontalLines',
        //   pageBreak: 'after'
        // },
        ...pagedTables,
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
                  // [
                  //   { text: 'Tax: ', style: 'label' },
                  //   { text: formatNumber(data.taxAmount), style: 'value', alignment: 'right' }
                  // ],
                  [
                    { text: 'Shipping Fee: ', style: 'label' },
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
                    { text: 'Grand Total: ', style: 'label' },
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
        },
        header:
        {
          fontSize: 18,
          bold: true,
          margin: [0, 10, 0, 10]
        },
        header3:
        {
          bold: true,
          fontSize: 10,
          alignment: 'left',
        },
        label:
        {
          fontSize: 10,
          alignment: 'right',
        },
        spacer:
        {
          margin: [0, 0, 0, 2]
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
        },
        footer:
        {
          color: 'gray',
          fontSize: 10,
          alignment: 'right',
          margin: [40, 0]
        },
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
  pricingName: 'pricing',
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


/* page break sample */
/*
var dd = {
	content: [
		{text: 'lightHorizontalLines:', fontSize: 14, bold: true, margin: [0, 20, 0, 8]},
		{
			style: 'tableExample',
			table: {
				headerRows: 1,
				body: [
					[{text: 'Header 1', style: 'tableHeader'}, {text: 'Header 2', style: 'tableHeader'}, {text: 'Header 3', style: 'tableHeader'}],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
				]
			},
			layout: 'lightHorizontalLines',
			pageBreak: 'after'
		},
		{
			style: 'tableExample',
			table: {
				headerRows: 1,
				body: [
					[{text: 'Header 1', style: 'tableHeader'}, {text: 'Header 2', style: 'tableHeader'}, {text: 'Header 3', style: 'tableHeader'}],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
				]
			},
			layout: 'lightHorizontalLines',
			pageBreak: 'after'
		},
		{
			style: 'tableExample',
			table: {
				headerRows: 1,
				body: [
					[{text: 'Header 1', style: 'tableHeader'}, {text: 'Header 2', style: 'tableHeader'}, {text: 'Header 3', style: 'tableHeader'}],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
				]
			},
			layout: 'lightHorizontalLines',
			pageBreak: 'after'
		},
		{
			style: 'tableExample',
			table: {
				headerRows: 1,
				body: [
					[{text: 'Header 1', style: 'tableHeader'}, {text: 'Header 2', style: 'tableHeader'}, {text: 'Header 3', style: 'tableHeader'}],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
				]
			},
			layout: 'lightHorizontalLines',
			pageBreak: 'after'
		},
		{
			style: 'tableExample',
			table: {
				headerRows: 1,
				body: [
					[{text: 'Header 1', style: 'tableHeader'}, {text: 'Header 2', style: 'tableHeader'}, {text: 'Header 3', style: 'tableHeader'}],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
				]
			},
			layout: 'lightHorizontalLines',
			pageBreak: 'after'
		},
	],
	styles: {
		header: {
			fontSize: 18,
			bold: true,
			margin: [0, 0, 0, 10]
		},
		subheader: {
			fontSize: 16,
			bold: true,
			margin: [0, 10, 0, 5]
		},
		tableExample: {
			margin: [0, 5, 0, 15]
		},
		tableHeader: {
			bold: true,
			fontSize: 13,
			color: 'black'
		}
	},
	defaultStyle: {
		// alignment: 'justify'
	}
	
}

*/