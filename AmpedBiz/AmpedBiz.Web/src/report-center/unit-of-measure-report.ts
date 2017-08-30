import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../services/formaters';
import { ReportBuilder, Report, DocumentDefinition } from '../services/report-builder';
import { UnitOfMeasure } from '../common/models/unit-of-measure';

export interface UnitOfMeasureReportModel {
  items: UnitOfMeasureReportModelItem[];
}

export interface UnitOfMeasureReportModelItem extends UnitOfMeasure {
}

@autoinject
export class UnitOfMeasureReport implements Report<UnitOfMeasureReportModel> {
  private readonly _reportBuilder: ReportBuilder;

  constructor(reportBuilder: ReportBuilder) {
    this._reportBuilder = reportBuilder;
  }

  public show(data: UnitOfMeasureReportModel): void {
    var document = this.buildDocument(data);
    this._reportBuilder.build({ title: 'Unit of Measure', document: document });
  }

  private buildDocument(data: UnitOfMeasureReportModel): DocumentDefinition {
    // header
    let orderTableBody: any[] = [
      [
        { text: 'Code', style: 'tableHeader' },
        { text: 'Name', style: 'tableHeader' },
      ],
    ];

    // items
    if (data && data.items && data.items.length > 0) {
      data.items.forEach(item => {
        orderTableBody.push([
          { text: emptyIfNull(item.id), style: 'tableData' },
          { text: emptyIfNull(item.name), style: 'tableData' },
        ]);
      });
    }

    return <DocumentDefinition>{
      content:
      [
        {
          text: 'Unit of Measure',
          style: 'title'
        },
        {
          table: {
            headerRows: 1,
            widths: ['auto', 'auto'],
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
        header2:
        {
          fontSize: 12,
          bold: true,
          margin: [0, 5, 0, 5]
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