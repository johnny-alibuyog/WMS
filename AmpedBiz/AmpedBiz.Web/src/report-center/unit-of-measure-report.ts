import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { Report, Content } from './report';
import { UnitOfMeasure } from '../common/models/unit-of-measure';

export interface UnitOfMeasureReportModel {
  items: UnitOfMeasureReportModelItem[];
}

export interface UnitOfMeasureReportModelItem extends UnitOfMeasure {
}

@autoinject
export class UnitOfMeasureReport extends Report<UnitOfMeasureReportModel> {

  protected buildBody(data: UnitOfMeasureReportModel): Promise<any[] | Content[]> {
    // header
    let uomTableBody: any[] = [
      [
        { text: 'Code', style: 'tableHeader' },
        { text: 'Name', style: 'tableHeader' },
      ],
    ];

    // items
    if (data && data.items && data.items.length > 0) {
      data.items.forEach(item => {
        uomTableBody.push([
          { text: emptyIfNull(item.id), style: 'tableData' },
          { text: emptyIfNull(item.name), style: 'tableData' },
        ]);
      });
    }

    let body = <Content[]>[
      {
        text: 'Unit of Measure',
        style: 'title'
      },
      {
        table: {
          headerRows: 1,
          widths: ['auto', 'auto'],
          body: uomTableBody
        },
        layout: 'lightHorizontalLines',
        style: 'tableExample',
      },
    ];

    return Promise.resolve(body);
  }
}
