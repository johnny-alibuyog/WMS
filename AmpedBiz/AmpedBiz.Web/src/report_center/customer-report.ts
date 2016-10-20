import {autoinject} from 'aurelia-framework';
import {formatDate, formatNumber, emptyIfNull, contactAsString, addressAsTring} from '../services/formaters';
import {ReportBuilder, Report, DocumentDefinition} from '../services/report-builder';
import {Address} from '../common/models/address';
import {Contact} from '../common/models/contact';

export interface CustomerReportModel {
  items: CustomerReportModelItem[];
}

export interface CustomerReportModelItem {
  id?: string;
  name?: string;
  creditLimitAmount?: number;
  contact?: Contact;
  officeAddress?: Address;
  billingAddress?: Address;
}

@autoinject
export class CustomerReport implements Report<CustomerReportModel> {
  private readonly _reportBuilder: ReportBuilder;

  constructor(reportBuilder: ReportBuilder) {
    this._reportBuilder = reportBuilder;
  }

  public show(data: CustomerReportModel): void {
    var document = this.buildDocument(data);
    this._reportBuilder.build({ title: 'Customers', document: document });
  }

  private buildDocument(data: CustomerReportModel): DocumentDefinition {
    let orderTableBody: any[] = [
      [
        { text: 'Customer', style: 'tableHeader' },
        { text: 'Credit Limit', style: 'tableHeader', alignment: 'right' },
        { text: 'Contacts', style: 'tableHeader' },
        { text: 'Office Address', style: 'tableHeader' },
        { text: 'Billing Address', style: 'tableHeader' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        orderTableBody.push([
          { text: emptyIfNull(x.name), style: 'tableData' },
          { text: formatNumber(x.creditLimitAmount), style: 'tableData', alignment: 'right' },
          { text: contactAsString(x.contact), style: 'tableData' },
          { text: addressAsTring(x.officeAddress), style: 'tableData' },
          { text: addressAsTring(x.billingAddress), style: 'tableData' },
        ])
      );
    }

    return <DocumentDefinition>{
      content:
      [
        {
          text: 'Customers',
          style: 'title'
        },
        {
          table: {
            headerRows: 1,
            widths: ['*', 'auto', 'auto', 'auto', 'auto'],
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