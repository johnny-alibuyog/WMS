update Orders set Status = 'Created' where Status = 'New';

update PurchaseOrders set Status = 'Created' where Status = 'New';

update OrderTransactions set Status = 'Created' where Status = 'New';

update PurchaseOrderTransactions set Status = 'Created' where Status = 'New';



select * from Orders;

select * from PurchaseOrders;

select * from OrderTransactions;

select * from PurchaseOrderTransactions;