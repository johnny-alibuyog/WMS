import { autoinject } from 'aurelia-framework';
import { formatDate, formatNumber, emptyIfNull } from '../../services/formaters';
import { Report, Content } from '../report';

export interface ProductSalesReportModel {
  productName?: string;
  fromDate?: Date;
  toDate?: Date;
  totalSoldPrice: string;
  items?: ProductSalesReportModelItem[];
}

export interface ProductSalesReportModelItem {
  productId?: string;
  productName?: string;
  totalSoldItems?: string;
  totalSoldPrice?: string;
  details?: ProductSalesReportPageDetailModelItem[];
}

export interface ProductSalesReportPageDetailModelItem {
  salesDate?: Date;
  customerName?: string;
  invoiceNumber?: string;
  soldItems?: string;
  soldPrice?: string;
}

@autoinject
export class ProductSalesReport extends Report<ProductSalesReportModel> {

  protected buildBody(data: ProductSalesReportModel): Promise<any[] | Content[]> {

    var body = <Content[]>[
      {
        text: 'Product Sales',
        style: 'title'
      },
      {
        text: 'From: ' + formatDate(data.fromDate),
        style: 'header2'
      },
      {
        text: 'To: ' + formatDate(data.toDate),
        style: 'header2'
      },
      {
        text: 'Total Sales: ' + emptyIfNull(data.totalSoldPrice),
        style: 'header2'
      },
      {
        text: ' ',
        style: 'spacer'
      },
      this.buildItems2(data)
    ];

    return Promise.resolve(body);
  }

  private buildItems2(data: ProductSalesReportModel): any {
    let tableBody: any[] = [];

    if (data && data.items && data.items.length > 0) {
      // table heading
      tableBody = [
        [
          { text: 'Product', style: 'tableHeader' },
          { text: 'Date', style: 'tableHeader' },
          { text: 'Invoice', style: 'tableHeader' },
          { text: 'Items', style: 'tableHeader', alignment: 'right' },
          { text: 'Price', style: 'tableHeader', alignment: 'right' },
        ],
      ];

      data.items.forEach(header => {
        // details
        header.details.forEach((detail, index) => {
          tableBody.push([
            (index == 0)
              ? { text: emptyIfNull(header.productName), rowSpan: header.details.length + 1, style: 'tableData' }
              : { text: '', style: 'tableData' },
            { text: formatDate(detail.salesDate), style: 'tableData' },
            { text: emptyIfNull(detail.invoiceNumber), style: 'tableData' },
            { text: emptyIfNull(detail.soldItems), style: 'tableData', alignment: 'right' },
            { text: emptyIfNull(detail.soldPrice), style: 'tableData', alignment: 'right' },
          ]);
        });
        // total
        tableBody.push([
          { text: '', style: 'tableData' },
          { text: '', style: 'tableData' },
          { text: 'Total:', style: 'label' },
          { text: emptyIfNull(header.totalSoldItems), style: 'label', alignment: 'right' },
          { text: emptyIfNull(header.totalSoldPrice), style: 'label', alignment: 'right' },
        ]);
      });
    }

    return {
      table: {
        headerRows: 1,
        widths: ['*', 'auto', 'auto', 'auto', 'auto'],
        body: tableBody
      },
      layout: 'lightHorizontalLines',
      style: 'tableExample',
    };
  }
}
