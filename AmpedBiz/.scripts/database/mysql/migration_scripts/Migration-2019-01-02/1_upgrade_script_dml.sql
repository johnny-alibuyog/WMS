set SQL_SAFE_UPDATES = 0;

update orders
set 
	Balance_Amount = ifnull(Total_Amount - Payment_Amount, Total_Amount),
    Balance_CurrencyId = Total_CurrencyId
where
	Balance_CurrencyId is null or
	Balance_Amount is null;


update purchaseorders
set 
	Balance_Amount = ifnull(Total_Amount - Paid_Amount, Total_Amount),
    Balance_CurrencyId = Total_CurrencyId
where
	Balance_CurrencyId is null or
	Balance_Amount is null;


set SQL_SAFE_UPDATES = 1;