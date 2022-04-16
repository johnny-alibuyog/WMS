import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { Report, Content } from './report';
import { PurchaseOrderStatus } from '../common/models/purchase-order';
import Enumerable from 'linq';

export interface PurchaseOrderReportModel {
  branchName?: string;
  supplierName?: string;
  voucherNumber?: string;
  fromDate?: Date;
  toDate?: Date;
  status?: string,
  items: PurchaseOrderReportItemModel[];
}

export interface PurchaseOrderReportItemModel {
  id?: string;
  createdOn?: Date;
  branchName?: string;
  supplierName?: string;
  voucherNumber?: string;
  createdByName?: string;
  approvedByName?: string;
  status?: PurchaseOrderStatus;
  totalAmount?: number;
  paidAmount?: number;
  balanceAmount?: number;
}

@autoinject
export class PurchaseOrderReport extends Report<PurchaseOrderReportModel> {

  public constructor() {
    super();
    this.option.pageOrientation = 'landscape';
  }

  protected async buildBody(data: PurchaseOrderReportModel): Promise<any[] | Content[]> {
    let orderTableBody: any[] = [
      [
        { text: 'Date', style: 'tableHeader' },
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Supplier', style: 'tableHeader' },
        { text: 'Voucher', style: 'tableHeader' },
        { text: 'Creator', style: 'tableHeader' },
        { text: 'Approver', style: 'tableHeader' },
        { text: 'Status', style: 'tableHeader' },
        { text: 'Total', style: 'tableHeader', alignment: 'right' },
        { text: 'Paid', style: 'tableHeader', alignment: 'right' },
        { text: 'Balance', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      // table content
      orderTableBody.push(...data.items.map(x => [
        { text: formatDate(x.createdOn), style: 'tableData' },
        { text: emptyIfNull(x.branchName), style: 'tableData' },
        { text: emptyIfNull(x.supplierName), style: 'tableData' },
        { text: emptyIfNull(x.voucherNumber), style: 'tableData' },
        { text: emptyIfNull(x.createdByName), style: 'tableData' },
        { text: emptyIfNull(x.approvedByName), style: 'tableData' },
        { text: PurchaseOrderStatus[x.status], style: 'tableData' },
        { text: formatNumber(x.totalAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.paidAmount), style: 'tableData', alignment: 'right' },
        { text: formatNumber(x.balanceAmount), style: 'tableData', alignment: 'right' }
      ]));

      // table footer
      orderTableBody.push([
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "Grand Total", style: "tableHeader" },
        { text: formatNumber(Enumerable.from(data.items).sum(o => o.totalAmount)), style: "tableData", alignment: "right" },
        { text: formatNumber(Enumerable.from(data.items).sum(o => o.paidAmount)), style: "tableData", alignment: "right" },
        { text: formatNumber(Enumerable.from(data.items).sum(o => o.balanceAmount)), style: "tableData", alignment: "right" },
      ]);
    }

    let body = [
      {
        text: 'Purchase Orders',
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
                      { text: 'Branch Name: ', style: 'label' },
                      { text: emptyIfNull(data.branchName), style: 'value' }
                    ],
                    [
                      { text: 'Customer: ', style: 'label' },
                      { text: emptyIfNull(data.supplierName), style: 'value' }
                    ],
                    [
                      { text: 'Created On: ', style: 'label' },
                      { text: formatDate(data.fromDate) + ' - ' + formatDate(data.toDate), style: 'value' }
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
          //widths: ['*', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto'],
          widths: ['auto', 'auto', '*', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto'],
          body: orderTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return Promise.resolve(body);
  }
}