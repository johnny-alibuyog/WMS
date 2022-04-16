- POS 
  - Can read product from barcode 
  - Should print receipt after purchase has been finalized
  - Can void transaction
  - Fields (PointOfSale)
    - Branch
	- Customer
	- Invoice Number
	- Sales By
	- Sales On
	- Items (PointOfSaleItem)
	  - Product Name
	  - Barcode
	  - Unit
	  - Quantity
	  - Price
	  - Total
    - Grand Total

- NHibernate Session usage should go to base class RequestHandlerBase
- Fix multitenancy for data seeding
- use EvalOrDefault for complex data evaluation
- When fething multiple aggregates from root, introduce multiple futures since one query will result into a unwanted multiple result sets (un performant)
- Rename reports files CustumerReport CustomerListingReport
- Rename reports files SupplierReport SupplierListingReport
- Make PurchaseOrder and Order Auditable
- There should be no negative Receipt Value (PurchaseOrderReceipt)
- refactor paging


https://www.cybrosys.com/blog/how-to-generate-manufacturing-and-purchase-order-from-sales-order-in-odoo
https://master.odoo.com/saas_master/demo/



Sales
 - SalesOrder (Pre-Order/InvoiceDirect)
	- SalesInvoices
 - SalesReturn
 

Purchases
 - PurchaseOrder
	- Voucher
 - PurchaseReturn


