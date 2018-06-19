import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { Report, Content } from './report';

export interface InventoryMovementsReportModel {
  branchName?: string;
  productName?: string;
  fromDate?: Date;
  toDate?: Date;
  items: InventoryMovementsReportItemModel[];
}

export interface InventoryMovementsReportItemModel {
  date?: Date;
  branchName?: string;
  productName?: string;
  out?: string;
  in?: string;
}

@autoinject
export class InventoryMovementsReport extends Report<InventoryMovementsReportModel> {

  protected async buildBody(data: InventoryMovementsReportModel): Promise<any[] | Content[]> {
    let orderTableBody: any[] = [
      [
        { text: 'Date', style: 'tableHeader' },
        { text: 'Branch', style: 'tableHeader' },
        { text: 'Product', style: 'tableHeader' },
        { text: 'In', style: 'tableHeader', alignment: 'right' },
        { text: 'Out', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      // table content
      orderTableBody.push(...data.items.map(x => [
        { text: formatDate(x.date), style: 'tableData' },
        { text: emptyIfNull(x.branchName), style: 'tableData' },
        { text: emptyIfNull(x.productName), style: 'tableData' },
        { text: emptyIfNull(x.in), style: 'tableData', alignment: 'right' },
        { text: emptyIfNull(x.out), style: 'tableData', alignment: 'right' },
      ]));

      // table footer
      // orderTableBody.push([
      //   { text: "", style: "tableData" },
      //   { text: "", style: "tableData" },
      //   { text: "", style: "tableData" },
      //   { text: "Grand Total", style: "tableHeader" },
      //   { text: formatNumber(data.items.map(o => o.totalAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
      //   { text: formatNumber(data.items.map(o => o.balanceAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
      // ]);
    }

    let body = [
      {
        text: 'Inventory Movement',
        style: 'title'
      },
      {
        table:
        {
          body:
            [
              [
                { text: 'Date: ', style: 'label' },
                { text: formatDate(data.fromDate) + ' - ' + formatDate(data.toDate), style: 'value' }
              ],
              [
                { text: 'Branch: ', style: 'label' },
                { text: emptyIfNull(data.branchName), style: 'value' }
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
      {
        text: ' ',
        style: 'separator'
      },
      {
        table: {
          headerRows: 1,
          widths: ['auto', 'auto', '*', 'auto', 'auto'],
          body: orderTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return Promise.resolve(body);
  }
}