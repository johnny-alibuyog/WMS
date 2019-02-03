import { PointOfSaleDetail } from '../common/models/point-of-sale';
import { DocumentDefinition, ReportViewer, getReportHeaderData, } from '../report-center/report';
import { emptyIfNull, formatDate, formatNumber } from '../services/formaters';

import { autoinject } from 'aurelia-framework';
import { getName } from '../common/custom_types/lookup';
import { getUnitId, getValue } from '../common/models/measure';
import * as linq from 'linq';

//https://github.com/bpampuch/pdfmake/issues/1194

declare var pdfMake: any;
declare var vfsfont: any;

@autoinject
export class ReceiptGenerator {

  public async show(data: PointOfSaleDetail): Promise<void> {
    let viewer = new ReportViewer();
    let document = this.buildDocument(data);
    let pdfUrl = await this.buildPdfUrl(document);
    viewer.show(pdfUrl);
  }

  public print(data: PointOfSaleDetail): void {
    let document = this.buildDocument(data);
    this.printPdf(document);
  }

  private buildPdfUrl(document: DocumentDefinition): Promise<any> {
    return new Promise<any>(async (resolve, reject) => {
      pdfMake.createPdf(document).getDataUrl(dataUrl => resolve(dataUrl));
    });
  }

  private printPdf(document: DocumentDefinition): void {
    pdfMake.createPdf(document).print();
  }

  private buildHeader(): any[] {
    let data = getReportHeaderData();

    return [
      {
        text: data.title,
        style: "headerTitle"
      },
      {
        text: data.address,
        style: "header3"
      },
      {
        text: `TEL NO. ${data.telephoneNumber}`,
        style: "header3"
      },
      {
        text: `TIN ${data.tinNumber}`,
        style: "header3"
      },
      {
        text: " ",
        style: "spacer"
      },
    ];

  }

  private buildStyles(): any {
    return {
      title: {
        fontSize: 16,
        bold: true,
        alignment: "center",
        margin: [0, 10, 0, 10]
      },
      headerTitle: {
        fontSize: 28,
        bold: true,
      },
      header: {
        fontSize: 18,
        bold: true,
        margin: [0, 10, 0, 10]
      },
      header3: {
        bold: true,
        fontSize: 8,
        alignment: "left",
      },
      label: {
        fontSize: 8,
      },
      spacer: {
        margin: [0, 0, 0, 2]
      },
      value: {
        fontSize: 8,
        color: "gray",
        alignment: "left",
      },
      itemHeaderLabel: {
        fontSize: 8,
        bold: true
      },
      itemHeaderInfo: {
        fontSize: 12,
        bold: true,
        italics: true
      },
      tablePlain: {
        alignment: "right",
        margin: [0, 0, 0, 0]
      },
      tableHeader: {
        bold: true,
        fontSize: 8,
        color: "black"
      },
      tableData: {
        fontSize: 8,
        color: "gray"
      },
      footer: {
        color: "gray",
        fontSize: 8,
        alignment: "right",
        margin: [40, 0]
      },
    };
  }

  private buildDocument(data: PointOfSaleDetail): DocumentDefinition {
    let tableBody = [];

    // table header
    tableBody.push([
      { text: "Product", style: "tableHeader" },
      { text: "Price", style: "tableHeader", alignment: "right" }
    ]);

    // table content
    tableBody.push(...data.items.map(x => [
      { text: `${getName(x.product)} (${getValue(x.quantity)} ${getUnitId(x.quantity)})`, style: "tableData" },
      { text: formatNumber(x.extendedPriceAmount), style: "tableData", alignment: "right" },
    ]));

    // table footer
    tableBody.push(...[
      [
        { text: "Sub Total: ", style: "label" },
        { text: formatNumber(data.subTotalAmount), style: "value", alignment: "right" }
      ],
      [
        { text: "Discount: ", style: "label" },
        { text: `(${formatNumber(data.discountAmount)})`, style: "value", alignment: "right" }
      ],
      [
        { text: "Total Amount: ", style: "label" },
        { text: formatNumber(data.totalAmount), style: "value", alignment: "right" }
      ],
      [
        { text: "Received Amount: ", style: "label" },
        { text: formatNumber(data.receivedAmount), style: "value", alignment: "right" }
      ],
      [
        { text: "Change Amount: ", style: "label" },
        { text: formatNumber(data.changeAmount), style: "value", alignment: "right" }
      ],
      [
        { text: "Paid Amount: ", style: "label" },
        { text: formatNumber(data.paidAmount), style: "value", alignment: "right" }
      ],
    ]);

    let body = [
      {
        style: "tablePlain",
        layout: "noBorders",
        table:
        {
          body:
            [
              [
                { text: "Branch Name: ", style: "label", alignment: "right" },
                { text: data.branchName || "", style: "value" }
              ],
              [
                { text: "Customer: ", style: "label", alignment: "right" },
                { text: emptyIfNull(data.customerName), style: "value" }
              ],
              [
                { text: "Invoice Number: ", style: "label", alignment: "right" },
                { text: data.invoiceNumber || "", style: "value" }
              ],
              [
                { text: "Tendered On: ", style: "label", alignment: "right" },
                { text: formatDate(data.tenderedOn), style: "value" }
              ],
              [
                { text: "Tendered By: ", style: "label", alignment: "right" },
                { text: data.tenderedByName || "", style: "value" }
              ],
            ],
        }
      },
      {
        text: " ",
        style: "spacer"
      },
      {
        style: "tableExample",
        table: {
          headerRows: 1,
          widths: ["*", "auto"],
          body: tableBody
        },
        layout: "noBorders"
      },
      {
        text: "----------------------------------------------------------------------------------------",
        style: "value"
      },
      {
        text: "THIS INVOICE/RECEIPT SHALL BE VALID FOR FIVE (5) YEARS FROM THE DATE OF THE PERMIT TO USE",
        style: "value"
      },
      {
        text: " ",
        style: "spacer"
      },
    ];

    let styles = this.buildStyles();
    let header = this.buildHeader();

    return <DocumentDefinition>{
      pageMargins: [15, 15, 15, 15],
      pageSize: {
        width: 230,
        height: 'auto'
      },
      content: [
        ...header,
        ...body
      ],
      styles: styles
    }
  }
}
