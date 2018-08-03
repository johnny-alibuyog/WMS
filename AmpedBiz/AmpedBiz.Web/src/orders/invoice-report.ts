import { DocumentDefinition, ReportBuilder, Report, Content, } from '../report-center/report';
import { emptyIfNull, formatDate, formatNumber } from '../services/formaters';

import { OrderInvoiceDetail } from '../common/models/order';
import { ServiceApi } from '../services/service-api';
import { InvoiceReportSetting } from '../common/models/invoice-report-setting';
import { autoinject } from 'aurelia-framework';
import { AuthService } from '../services/auth-service';

@autoinject
export class InvoiceReport extends Report<OrderInvoiceDetail> {

  constructor(private readonly _api: ServiceApi) {
    super();
  }

  protected async buildBody(data: OrderInvoiceDetail): Promise<any[] | Content[]> {

    let settings = await this._api.settings.getInvoiceReportSetting();

    let productTableBody: any[] = [
      [
        { text: "Product", style: "tableHeader" },
        { text: "Unit", style: "tableHeader", alignment: "right" },
        { text: "Qty", style: "tableHeader", alignment: "right" },
        { text: "Unit Price", style: "tableHeader", alignment: "right" },
        { text: "Discount", style: "tableHeader", alignment: "right" },
        { text: "Price", style: "tableHeader", alignment: "right" }
      ],
    ];

    if (data && data.items && data.items.length > 0) {
      // table content
      productTableBody.push(...data.items.map(x => [
        { text: x.product && x.product.name || "", style: "tableData" },
        { text: x.quantity && x.quantity.unit && x.quantity.unit.name || "", style: "tableData" },
        { text: formatNumber(x.quantity.value, "0"), style: "tableData", alignment: "right" },
        { text: formatNumber(x.unitPriceAmount), style: "tableData", alignment: "right" },
        { text: formatNumber(x.discountAmount), style: "tableData", alignment: "right" },
        { text: formatNumber(x.totalPriceAmount), style: "tableData", alignment: "right" },
      ]));
    }

    let pagedTables = [];

    let pageSize = settings.pageItemSize;
    let total = data.items.length;

    for (let i = 0; i < total; i += pageSize) {
      let pageItems = data.items.slice(i, i + pageSize);
      let tableBody = [];

      // table header
      tableBody.push([
        { text: "Product", style: "tableHeader" },
        { text: "Unit", style: "tableHeader", alignment: "right" },
        { text: "Qty", style: "tableHeader", alignment: "right" },
        { text: "Unit Price", style: "tableHeader", alignment: "right" },
        { text: "Discount", style: "tableHeader", alignment: "right" },
        { text: "Price", style: "tableHeader", alignment: "right" }
      ]);

      // table content
      tableBody.push(...pageItems.map(x => [
        { text: x.product && x.product.name || "", style: "tableData" },
        { text: x.quantity && x.quantity.unit && x.quantity.unit.name || "", style: "tableData" },
        { text: formatNumber(x.quantity.value, "0"), style: "tableData", alignment: "right" },
        { text: formatNumber(x.unitPriceAmount), style: "tableData", alignment: "right" },
        { text: formatNumber(x.discountAmount), style: "tableData", alignment: "right" },
        { text: formatNumber(x.totalPriceAmount), style: "tableData", alignment: "right" },
      ]));

      // table footer
      tableBody.push([
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: "", style: "tableData" },
        { text: formatNumber(pageItems.map(o => o.unitPriceAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
        { text: formatNumber(pageItems.map(o => o.discountAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
        { text: formatNumber(pageItems.map(o => o.totalPriceAmount).reduce((pre, cur) => pre + cur)), style: "tableData", alignment: "right" },
      ]);

      pagedTables.push({
        style: "tableExample",
        table: {
          headerRows: 1,
          widths: ["*", "auto", "auto", "auto", "auto", "auto"],
          body: tableBody
        },
        layout: "lightHorizontalLines",
        pageBreak: "after"
      });

    }

    let body = [{
      columns:
        [
          {
            style: "tablePlain",
            layout: "noBorders",
            table:
            {
              body:
                [
                  [
                    { text: "Customer: ", style: "label" },
                    { text: emptyIfNull(data.customerName), style: "value" }
                  ],
                  [
                    { text: "Invoice Number: ", style: "label" },
                    { text: data.invoiceNumber || "", style: "value" }
                  ],
                  [
                    { text: "Inviced On: ", style: "label" },
                    { text: formatDate(data.invoicedOn), style: "value" }
                  ],
                  [
                    { text: "Invoiced By: ", style: "label" },
                    { text: data.invoicedByName || "", style: "value" }
                  ],
                ],
            }
          },
          {
            style: "tablePlain",
            layout: "noBorders",
            table:
            {
              body:
                [
                  [
                    { text: "Branch Name: ", style: "label" },
                    { text: data.branchName || "", style: "value" }
                  ],
                  [
                    { text: "Ordered On: ", style: "label" },
                    { text: formatDate(data.orderedOn), style: "value" }
                  ],
                  [
                    { text: "Staff: ", style: "label" },
                    { text: data.orderedByName || "", style: "value" }
                  ],
                  [
                    { text: "Delivery Address: ", style: "label" },
                    { text: data.shippingAddress || "", style: "value" }
                  ],
                ],
            }
          },
        ]
    },
    {
      text: " ",
      style: "spacer"
    },
    ...pagedTables,
    {
      columns:
        [
          { text: "" },
          { text: "" },
          { text: "" },
          {
            style: "tablePlain",
            layout: "noBorders",
            table:
            {
              body:
                [
                  // [
                  //   { text: 'Tax: ', style: 'label' },
                  //   { text: formatNumber(data.taxAmount), style: 'value', alignment: 'right' }
                  // ],
                  [
                    { text: "Shipping Fee: ", style: "label" },
                    { text: formatNumber(data.shippingFeeAmount), style: "value", alignment: "right" }
                  ],
                  [
                    { text: "Discount: ", style: "label" },
                    { text: formatNumber(data.discountAmount), style: "value", alignment: "right" }
                  ],
                  [
                    { text: "Sub Total: ", style: "label" },
                    { text: formatNumber(data.subTotalAmount), style: "value", alignment: "right" }
                  ],
                  [
                    { text: "Grand Total: ", style: "label" },
                    { text: formatNumber(data.totalAmount), style: "value", alignment: "right" }
                  ],
                  [
                    { text: "Returned: ", style: "label" },
                    { text: formatNumber(data.returnedAmount), style: "value", alignment: "right" }
                  ],
                ],
            }
          },
        ]
    }];

    return body;
  }
}


/* page break sample */
/*
var dd = {
	content: [
		{text: 'lightHorizontalLines:', fontSize: 14, bold: true, margin: [0, 20, 0, 8]},
		{
			style: 'tableExample',
			table: {
				headerRows: 1,
				body: [
					[{text: 'Header 1', style: 'tableHeader'}, {text: 'Header 2', style: 'tableHeader'}, {text: 'Header 3', style: 'tableHeader'}],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
				]
			},
			layout: 'lightHorizontalLines',
			pageBreak: 'after'
		},
		{
			style: 'tableExample',
			table: {
				headerRows: 1,
				body: [
					[{text: 'Header 1', style: 'tableHeader'}, {text: 'Header 2', style: 'tableHeader'}, {text: 'Header 3', style: 'tableHeader'}],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
				]
			},
			layout: 'lightHorizontalLines',
			pageBreak: 'after'
		},
		{
			style: 'tableExample',
			table: {
				headerRows: 1,
				body: [
					[{text: 'Header 1', style: 'tableHeader'}, {text: 'Header 2', style: 'tableHeader'}, {text: 'Header 3', style: 'tableHeader'}],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
				]
			},
			layout: 'lightHorizontalLines',
			pageBreak: 'after'
		},
		{
			style: 'tableExample',
			table: {
				headerRows: 1,
				body: [
					[{text: 'Header 1', style: 'tableHeader'}, {text: 'Header 2', style: 'tableHeader'}, {text: 'Header 3', style: 'tableHeader'}],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
				]
			},
			layout: 'lightHorizontalLines',
			pageBreak: 'after'
		},
		{
			style: 'tableExample',
			table: {
				headerRows: 1,
				body: [
					[{text: 'Header 1', style: 'tableHeader'}, {text: 'Header 2', style: 'tableHeader'}, {text: 'Header 3', style: 'tableHeader'}],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
					['Sample value 1', 'Sample value 2', 'Sample value 3'],
				]
			},
			layout: 'lightHorizontalLines',
			pageBreak: 'after'
		},
	],
	styles: {
		header: {
			fontSize: 18,
			bold: true,
			margin: [0, 0, 0, 10]
		},
		subheader: {
			fontSize: 16,
			bold: true,
			margin: [0, 10, 0, 5]
		},
		tableExample: {
			margin: [0, 5, 0, 15]
		},
		tableHeader: {
			bold: true,
			fontSize: 13,
			color: 'black'
		}
	},
	defaultStyle: {
		// alignment: 'justify'
	}
	
}

*/