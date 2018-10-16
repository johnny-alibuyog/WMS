update orders
set branchid = (select branchid from branches LIMIT 1)
where branchid is null;