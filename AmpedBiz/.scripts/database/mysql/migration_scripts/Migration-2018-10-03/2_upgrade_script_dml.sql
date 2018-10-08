	use ampedbizdb;

	set SQL_SAFE_UPDATES = 0;

	update returns
    set
		TotalReturned_Amount = Total_Amount,
        TotalReturned_CurrencyId = Total_CurrencyId
	where 
		TotalReturned_Amount is null or
        TotalReturned_CurrencyId is null;

	insert into returnitembases 
	(
		ReturnItemBaseId,
		ProductId,
		ReasonId,
		Quantity_Value,
		Quantity_UnitId,
		Standard_Value,
		Standard_UnitId,
		QuantityStandardEquivalent_Value,
		QuantityStandardEquivalent_UnitId,
		Returned_Amount,
		Returned_CurrencyId
	)
	(
		select 
			returnitemid,
			ProductId,
			ReturnReasonId,
			Quantity_Value,
			Quantity_UnitId,
			Standard_Value,
			Standard_UnitId,
			QuantityStandardEquivalent_Value,
			QuantityStandardEquivalent_UnitId,
			TotalPrice_Amount,
			TotalPrice_CurrencyId
		from 
			returnitems
			
		union
		
		select 
			orderreturnid,
			ProductId,
			ReasonId,
			Quantity_Value,
			Quantity_UnitId,
			Standard_Value,
			Standard_UnitId,
			QuantityStandardEquivalent_Value,
			QuantityStandardEquivalent_UnitId,
			Returned_Amount,
			Returned_CurrencyId
		from 
			orderreturns
		
	);
    
    
	update returnitems
    set 
		returnitembaseid = returnitemid
    where 
		returnitembaseid is null;

    
	update orderreturns
    set 
		returnitembaseid = orderreturnid
    where 
		returnitembaseid is null;


	set SQL_SAFE_UPDATES = 1;

    