import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull, contactAsString, addressAsTring } from '../services/formaters';
import { ReportBuilder, Report, DocumentDefinition } from '../services/report-builder';
import { Address } from '../common/models/address';
import { Contact } from '../common/models/contact';

export interface SupplierReportModel {
  items: SupplierReportModelItem[];
}

export interface SupplierReportModelItem {
  id?: string;
  name?: string;
  contact?: Contact;
  address?: Address;
}

@autoinject
export class SupplierReport implements Report<SupplierReportModel> {
  private readonly _reportBuilder: ReportBuilder;

  constructor(reportBuilder: ReportBuilder) {
    this._reportBuilder = reportBuilder;
  }

  public show(data: SupplierReportModel): void {
    var document = this.buildDocument(data);
    this._reportBuilder.build({ title: 'Suppliers', document: document });
  }

  private buildDocument(data: SupplierReportModel): DocumentDefinition {
    let orderTableBody: any[] = [
      [
        { text: 'Supplier', style: 'tableHeader' },
        { text: 'Contacts', style: 'tableHeader' },
        { text: 'Address', style: 'tableHeader' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        orderTableBody.push([
          { text: emptyIfNull(x.name), style: 'tableData' },
          { text: contactAsString(x.contact), style: 'tableData' },
          { text: addressAsTring(x.address), style: 'tableData' },
        ])
      );
    }

    return <DocumentDefinition>{
      content:
      [
        {
          text: 'Suppliers',
          style: 'title'
        },
        {
          table: {
            headerRows: 1,
            widths: ['*', 'auto', 'auto'],
            body: orderTableBody
          },
          layout: 'lightHorizontalLines',
          style: 'tableExample',
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