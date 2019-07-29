import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull, contactAsString, addressAsTring } from '../services/formaters';
import { Report, Content } from './report';
import { Address } from '../common/models/address';
import { Contact } from '../common/models/contact';

export interface SupplierReportModel {
  items: SupplierReportModelItem[];
}

export interface SupplierReportModelItem {
  id?: string;
  name?: string;
  contactPerson?: string;
  contact?: Contact;
  address?: Address;
}

@autoinject
export class SupplierReport extends Report<SupplierReportModel> {

  protected buildBody(data: SupplierReportModel): Promise<any[] | Content[]> {

    let supplierTableBody: any[] = [

      [
        { text: 'Supplier Name', style: 'tableHeader' },
        { text: 'Contact Person', style: 'tableHeader' },
        { text: 'Contact Number', style: 'tableHeader' },
        { text: 'Address', style: 'tableHeader' },
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      data.items.forEach(x =>
        supplierTableBody.push([
          { text: emptyIfNull(x.name), style: 'tableData' },
          { text: emptyIfNull(x.contactPerson), style: 'tableData' },
          { text: contactAsString(x.contact), style: 'tableData' },
          { text: addressAsTring(x.address), style: 'tableData' },
        ])
      );
    }

    let body = <Content[]>[
      {
        text: 'Suppliers',
        style: 'title'
      },
      {
        table: {
          headerRows: 1,
          widths: ['*', 'auto', 'auto', 'auto'],
          body: supplierTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return  Promise.resolve(body);
  }
}
