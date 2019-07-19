import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull, contactAsString, addressAsTring } from '../../services/formaters';
import { Report, Content } from '../report';
import { Address } from '../../common/models/address';
import { Contact } from '../../common/models/contact';

export interface CustomerReportModel {
  items: CustomerReportModelItem[];
}

export interface CustomerReportModelItem {
  id?: string;
  name?: string;
  contactPerson?: string;
  creditLimitAmount?: number;
  contact?: Contact;
  officeAddress?: Address;
  billingAddress?: Address;
}

@autoinject
export class CustomerReport extends Report<CustomerReportModel> {
  protected async buildBody(data: CustomerReportModel): Promise<any[] | Content[]> {
    let orderTableBody: any[] = [
      [
        { text: 'Customer', style: 'tableHeader' },
        { text: 'Contact Person', style: 'tableHeader' },
        { text: 'Credit Limit', style: 'tableHeader', alignment: 'right' },
        { text: 'Contacts', style: 'tableHeader' },
        { text: 'Address', style: 'tableHeader' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        orderTableBody.push([
          { text: emptyIfNull(x.name), style: 'tableData' },
          { text: emptyIfNull(x.contactPerson), style: 'tableData' },
          { text: formatNumber(x.creditLimitAmount), style: 'tableData', alignment: 'right' },
          { text: contactAsString(x.contact), style: 'tableData' },
          { text: addressAsTring(x.officeAddress), style: 'tableData' },
        ])
      );
    }

    let body = [
      {
        text: 'Customer List',
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
    ];

    return Promise.resolve(body);
  }
}
