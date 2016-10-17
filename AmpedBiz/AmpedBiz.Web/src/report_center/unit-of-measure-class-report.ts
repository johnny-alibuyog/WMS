import {autoinject} from 'aurelia-framework';
import {formatDate, formatNumber, emptyIfNull} from '../services/formaters';
import {ReportBuilder, Report, DocumentDefinition} from '../services/report-builder';
import {UnitOfMeasureClass} from '../common/models/unit-of-measure-class';

export interface UnitOfMeasureClassReportModel {
  items: UnitOfMeasureClassReportModelItem[];
}

export interface UnitOfMeasureClassReportModelItem extends UnitOfMeasureClass {
}

@autoinject
export class UnitOfMeasureClassReport implements Report<UnitOfMeasureClassReportModel> {
  private readonly _reportBuilder: ReportBuilder;

  constructor(reportBuilder: ReportBuilder) {
    this._reportBuilder = reportBuilder;
  }

  public show(data: UnitOfMeasureClassReportModel): void {
    var document = this.buildDocument(data);
    this._reportBuilder.build({ title: 'Unit of Measure', document: document });
  }

  private buildDocument(data: UnitOfMeasureClassReportModel): DocumentDefinition {
    // header
    let orderTableBody: any[] = [
      [
        { text: 'Class', style: 'tableHeader' },
        { text: 'Symbol', style: 'tableHeader' },
        { text: 'Unit', style: 'tableHeader' },
        { text: 'Is Base', style: 'tableHeader' },
        { text: 'Conversion Factor', style: 'tableHeader', alignment: 'right' },
      ],
    ];

    // items
    if (data && data.items && data.items.length > 0) {
      data.items.forEach(classItem => {

        orderTableBody.push([
          { text: emptyIfNull(classItem.name), rowSpan: classItem.units.length + 1, style: 'header2' },
          { text: '', style: 'tableData' },
          { text: '', style: 'tableData' },
          { text: '', style: 'tableData' },
          { text: '', style: 'tableData' },
        ]);

        classItem.units.forEach(unitItem =>
          orderTableBody.push([
            { text: '', style: 'tableData' },
            { text: emptyIfNull(unitItem.id), style: 'tableData' },
            { text: emptyIfNull(unitItem.name), style: 'tableData' },
            { text: unitItem.isBaseUnit ? 'True' : 'False', style: 'tableData' },
            { text: formatNumber(unitItem.conversionFactor), style: 'tableData', alignment: 'right' },
          ])
        );
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
            widths: ['auto', 'auto', 'auto', 'auto', 'auto'],
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