using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories;
using AmpedBiz.Core.Products;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Inventories
{
	public class GetInventoryMovementsReportPage
	{
		public class Request : PageRequest, IRequest<Response> { }

		public class Response : PageResponse<Dto.InventoryMovementsReportPageItem> { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				if (message.Filter == null)
					message.Filter = new Filter();

				using (var session = this.SessionFactory.RetrieveSharedSession(this.Context))
				using (var transaction = session.BeginTransaction())
				{

					// TODO: use decomposition when upgraded to C# 6 or up
					var whereResult = this.BuildWhereClause(message.Filter);

					var whereClause = whereResult.Item1;

					var orderByClause = this.BuildOrderByClause(message.Sorter);

					var sqlString = BuidlSqlQueryString(whereClause, orderByClause);

					var sqlQuery = session.CreateSQLQuery(sqlString);

					var parameters = new
					{
						BranchId = whereResult.Item2.Item1,
						ProductId = whereResult.Item2.Item2,
						FromDate = whereResult.Item2.Item3,
						ToDate = whereResult.Item2.Item4,
					};

					if (parameters.BranchId != Guid.Empty)
						sqlQuery.SetParameter("branchId", parameters.BranchId);

					if (parameters.ProductId != Guid.Empty)
						sqlQuery.SetParameter("productId", parameters.ProductId);

					if (parameters.FromDate != null)
						sqlQuery.SetParameter("fromDate", parameters.FromDate.Value);

					if (parameters.ToDate != null)
						sqlQuery.SetParameter("toDate", parameters.ToDate.Value);

					//var result = sqlQuery
					//    .SetResultTransformer(Transformers.AliasToBean<Dto.InventoryMovementReportPageItemRaw>())
					//    .List<Dto.InventoryMovementReportPageItemRaw>();

					// Note: since there is a type mismatch (Guid for example) in some databases (MySql), we needed 
					//       to manually convert columns using AliasToEntityMap instead of AliasToBean

					var result = sqlQuery
						.SetResultTransformer(Transformers.AliasToEntityMap)
						.List<IDictionary>()
						.Select(Dto.InventoryMovementReportPageItemRaw.Parse)
						.ToList();

					var inventory = (Inventory)null;
					var product = (Product)null;
					var branch = (Branch)null;

					var conditions = result.Aggregate(Restrictions.Disjunction(), (next, item) =>
					{
						next.Add(Restrictions.Conjunction()
							.Add(Restrictions.Where(() => product.Id == item.ProductId))
							.Add(Restrictions.Where(() => branch.Id == item.BranchId))
						);

						return next;
					});

					var inventories = session
						.QueryOver(() => inventory)
						.JoinAlias(() => inventory.Product, () => product)
						.JoinAlias(() => inventory.Branch, () => branch)
						.Fetch(x => x.Product.UnitOfMeasures).Eager
						.Fetch(x => x.Product.UnitOfMeasures.First().UnitOfMeasure).Eager
						.Where(conditions)
						.TransformUsing(Transformers.DistinctRootEntity)
						.Future();

					var lookups = new
					{
						Branchs = inventories.Select(x => x.Branch).Distinct().ToDictionary(x => x.Id),
						Products = inventories.Select(x => x.Product).ToDictionary(x => x.Id),
						Units = inventories.SelectMany(x => x.Product.UnitOfMeasures.Select(o => o.UnitOfMeasure)).Distinct().ToDictionary(x => x.Id)
					};

					// TODO: this is not performant, this is just a work around on groupby count issue of nhibernate. find a solution soon
					var totalItems = result
						.Select(x => Dto.InventoryMovementsReportPageItem.Create(
							branchLookup: lookups.Branchs,
							productLookup: lookups.Products,
							unitLookup: lookups.Units,
							raw: x
						))
						.ToList();

					var count = totalItems.Count;

					if (message.Pager.IsPaged() != true)
						message.Pager.RetrieveAll(count);

					var items = totalItems
						.Skip(message.Pager.SkipCount)
						.Take(message.Pager.Size)
						.ToList();

					response = new Response()
					{
						Count = count,
						Items = items
					};

					transaction.Commit();
				}

				return response;
			}

			private static string BuidlSqlQueryString(string whereClause, string orderByClause)
			{
				switch (DatabaseConfig.Instance.Database)
				{
					case DatabaseProvider.MsSql:
						return $@"
                            WITH InventoryMovements (
	                            BranchId,
	                            BranchName,
	                            ProductId,
	                            ProductName,
	                            Date,
	                            InputUnitId,
	                            InputValue,
	                            OutputUnitId,
	                            OutputValue
                            ) 
                            AS (

                                SELECT 
	                                BranchId = COALESCE(INPUT.BranchId, OUTPUT.BranchId),
	                                BranchName = COALESCE(INPUT.BranchName, OUTPUT.BranchName),
	                                ProductId = COALESCE(INPUT.ProductId, OUTPUT.ProductId),
	                                ProductName = COALESCE(INPUT.ProductName, OUTPUT.ProductName),
	                                Date = COALESCE(INPUT.Date, OUTPUT.Date),
	                                InputUnitId = INPUT.UnitId,
	                                InputValue = INPUT.Value,
	                                OutputUnitId = OUTPUT.UnitId,
	                                OutputValue = OUTPUT.Value

                                FROM
                                    (
                                        SELECT
                                            BranchId = PO.BranchId,
                                            BranchName = MAX(B.Name),
                                            ProductId = POR.ProductId,
                                            ProductName = MAX(P.Name),
                                            Date = CAST(POR.ReceivedOn AS DATE),
                                            UnitId = POR.QuantityStandardEquivalent_UnitId,
                                            Value = SUM(POR.QuantityStandardEquivalent_Value)

                                        FROM
                                            PurchaseOrderReceipts AS POR

                                        INNER JOIN
                                            PurchaseOrders AS PO
                                                ON PO.PurchaseOrderId = POR.PurchaseOrderId

                                        INNER JOIN
                                            Branches AS B
                                                ON B.BranchId = PO.BranchId

                                        INNER JOIN
                                            Products AS P
                                                ON P.ProductId = POR.ProductId

                                        GROUP BY
                                            PO.BranchId,
                                            POR.ProductId,
                                            CAST(POR.ReceivedOn AS DATE),
		                                    POR.QuantityStandardEquivalent_UnitId
                                    ) AS INPUT

                                FULL JOIN
                                    (
                                        SELECT
                                            BranchId = O.BranchId,
                                            BranchName = MAX(B.Name),
                                            ProductId = OI.ProductId,
                                            ProductName = MAX(P.Name),
                                            Date = CAST(O.ShippedOn AS DATE),
                                            UnitId = OI.QuantityStandardEquivalent_UnitId,
                                            Value = SUM(OI.QuantityStandardEquivalent_Value)

                                        FROM
                                            OrderItems AS OI

                                        INNER JOIN
                                            Orders AS O
                                                ON O.OrderId = OI.OrderId

                                        INNER JOIN
                                            Branches AS B
                                                ON B.BranchId = O.BranchId

                                        INNER JOIN
                                            Products AS P
                                                ON P.ProductId = OI.ProductId

                                        WHERE
                                            O.ShippedOn IS NOT NULL

                                        GROUP BY
                                            O.BranchId,
                                            OI.ProductId,
                                            CAST(O.ShippedOn AS DATE),
		                                    OI.QuantityStandardEquivalent_UnitId
                                    ) AS OUTPUT

                                    ON INPUT.BranchId = OUTPUT.BranchId 
                                       AND INPUT.ProductId = OUTPUT.ProductId 
                                       AND INPUT.Date = OUTPUT.Date
                            )

                            SELECT *
                        
                            FROM    
                                InventoryMovements

                            {whereClause}

                            {orderByClause}
                    ";

					case DatabaseProvider.MySql:
						return $@"
                            CREATE TEMPORARY TABLE IF NOT EXISTS INPUT_TABLE
	                            SELECT
		                            PO.BranchId AS BranchId,
		                            MAX(B.Name) AS BranchName,
		                            POR.ProductId AS ProductId,
		                            MAX(P.Name) AS ProductName,
		                            CAST(POR.ReceivedOn AS DATE) AS Date,
		                            POR.QuantityStandardEquivalent_UnitId AS UnitId,
		                            SUM(POR.QuantityStandardEquivalent_Value) AS Value

	                            FROM
		                            PurchaseOrderReceipts AS POR

	                            INNER JOIN
		                            PurchaseOrders AS PO
			                            ON PO.PurchaseOrderId = POR.PurchaseOrderId

	                            INNER JOIN
		                            Branches AS B
			                            ON B.BranchId = PO.BranchId

	                            INNER JOIN
		                            Products AS P
			                            ON P.ProductId = POR.ProductId

	                            GROUP BY
		                            PO.BranchId,
		                            POR.ProductId,
		                            CAST(POR.ReceivedOn AS DATE),
		                            POR.QuantityStandardEquivalent_UnitId;
        


                            CREATE TEMPORARY TABLE IF NOT EXISTS OUTPUT_TABLE
	                            SELECT
		                            O.BranchId AS BranchId,
		                            MAX(B.Name) AS BranchName,
		                            OI.ProductId AS ProductId,
		                            MAX(P.Name) AS ProductName,
		                            CAST(O.ShippedOn AS DATE) AS Date,
		                            OI.QuantityStandardEquivalent_UnitId AS UnitId,
		                            SUM(OI.QuantityStandardEquivalent_Value) AS Value

	                            FROM
		                            OrderItems AS OI

	                            INNER JOIN
		                            Orders AS O
			                            ON O.OrderId = OI.OrderId

	                            INNER JOIN
		                            Branches AS B
			                            ON B.BranchId = O.BranchId

	                            INNER JOIN
		                            Products AS P
			                            ON P.ProductId = OI.ProductId

	                            WHERE
		                            O.ShippedOn IS NOT NULL

	                            GROUP BY
		                            O.BranchId,
		                            OI.ProductId,
		                            CAST(O.ShippedOn AS DATE),
		                            OI.QuantityStandardEquivalent_UnitId;
        
        
                            CREATE TEMPORARY TABLE IF NOT EXISTS RESULT_TABLE        
	                            SELECT 
		                            COALESCE(INPUT.BranchId, OUTPUT.BranchId) AS BranchId,
		                            COALESCE(INPUT.BranchName, OUTPUT.BranchName) AS BranchName,
		                            COALESCE(INPUT.ProductId, OUTPUT.ProductId) AS ProductId,
		                            COALESCE(INPUT.ProductName, OUTPUT.ProductName) AS ProductName,
		                            COALESCE(INPUT.Date, OUTPUT.Date) AS Date,
		                            INPUT.UnitId AS InputUnitId,
		                            INPUT.Value AS InputValue,
		                            OUTPUT.UnitId AS OutputUnitId,
		                            OUTPUT.Value AS OutputValue
	
	                            FROM
		                            INPUT_TABLE AS INPUT
		
	                            RIGHT JOIN
		                            OUTPUT_TABLE AS OUTPUT
			                            ON INPUT.BranchId = OUTPUT.BranchId 
			                            AND INPUT.ProductId = OUTPUT.ProductId 
			                            AND INPUT.Date = OUTPUT.Date;

                            INSERT INTO RESULT_TABLE
	                            SELECT 
		                            COALESCE(INPUT.BranchId, OUTPUT.BranchId) AS BranchId,
		                            COALESCE(INPUT.BranchName, OUTPUT.BranchName) AS BranchName,
		                            COALESCE(INPUT.ProductId, OUTPUT.ProductId) AS ProductId,
		                            COALESCE(INPUT.ProductName, OUTPUT.ProductName) AS ProductName,
		                            COALESCE(INPUT.Date, OUTPUT.Date) AS Date,
		                            INPUT.UnitId AS InputUnitId,
		                            INPUT.Value AS InputValue,
		                            OUTPUT.UnitId AS OutputUnitId,
		                            OUTPUT.Value AS OutputValue
	
	                            FROM
		                            INPUT_TABLE AS INPUT
		
	                            RIGHT JOIN
		                            OUTPUT_TABLE AS OUTPUT
			                            ON INPUT.BranchId = OUTPUT.BranchId 
			                            AND INPUT.ProductId = OUTPUT.ProductId 
			                            AND INPUT.Date = OUTPUT.Date
			
	                            WHERE
		                            INPUT.BranchId IS NULL 
		                            OR INPUT.ProductId IS NULL 
		                            OR INPUT.Date IS NULL;

                            SELECT 
                                *
                            
                            FROM RESULT_TABLE

                            { whereClause}

                            { orderByClause};

                            DROP TEMPORARY TABLE INPUT_TABLE;
                            DROP TEMPORARY TABLE OUTPUT_TABLE;
                            DROP TEMPORARY TABLE RESULT_TABLE;
                    ";

					default:
						throw new NotSupportedException($"{DatabaseConfig.Instance.Database} is not yet supported by this report.");
				}
			}

			private Tuple<string, Tuple<Guid, Guid, DateTime?, DateTime?>> BuildWhereClause(Filter filter)
			{
				var conditions = string.Empty;

				var addConjuction = new Func<string, string, string>((left, right) =>
				{
					if (string.IsNullOrWhiteSpace(left))
						return right;

					return $"{left} AND {right}";
				});

				// compose filter
				var branchId = default(Guid);
				filter.Compose<Guid>("branchId", value =>
				{
					branchId = value;
					conditions = $"BranchId = :branchId";
				});

				var productId = default(Guid);
				filter.Compose<Guid>("productId", value =>
				{
					productId = value;
					conditions = addConjuction(conditions, $"ProductId = :productId");
				});

				var fromDate = default(DateTime?);
				filter.Compose<DateTime>("fromDate", value =>
				{
					fromDate = value;
					conditions = addConjuction(conditions, $"date >= :fromDate");
				});

				var toDate = default(DateTime?);
				filter.Compose<DateTime>("toDate", value =>
				{
					toDate = value;
					conditions = addConjuction(conditions, $"date <= :toDate");
				});

				var whereClause = !string.IsNullOrWhiteSpace(conditions)
						? $"WHERE {conditions}" : string.Empty;

				return new Tuple<string, Tuple<Guid, Guid, DateTime?, DateTime?>>(
					whereClause,
					new Tuple<Guid, Guid, DateTime?, DateTime?>(
						branchId, productId, fromDate, toDate
					)
				);
			}

			private string BuildOrderByClause(Sorter sorter)
			{
				var orderFields = new List<string>();

				// compose order
				sorter.Compose("branchName", direction =>
				{
					orderFields.Add(direction == SortDirection.Ascending
						? "BranchName ASC"
						: "BranchName DESC"
					);
				});

				sorter.Compose("productName", direction =>
				{
					orderFields.Add(direction == SortDirection.Ascending
						? "ProductName ASC"
						: "ProductName DESC"
					);
				});

				sorter.Compose("date", direction =>
				{
					orderFields.Add(direction == SortDirection.Ascending
						? "Date ASC"
						: "Date DESC"
					);
				});

				var orderClause = orderFields.Any()
					? $"ORDER BY {string.Join(", ", orderFields)}"
					: string.Empty;

				return orderClause;
			}
		}
	}
}

/* Linq (not working)
 * 
	var inputQuery = session.Query<PurchaseOrderReceipt>()
		.GroupBy(x => new
		{
			BranchId = x.PurchaseOrder.Branch.Id,
			ProductId = x.Product.Id,
			Date = x.ReceivedOn.Value.Date,
		})
		.Select(x => new
		{
			Id = x.Key.BranchId.ToString() + "_" +
				x.Key.ProductId.ToString() + "_" +
				x.Key.Date.ToString(),
			BranchId = x.Key.BranchId,
			ProductId = x.Key.ProductId,
			Date = x.Key.Date,
			Quantity = new Measure(
				x.Sum(o => o.QuantityStandardEquivalent.Value),
				x.Max(o => o.QuantityStandardEquivalent.Unit)
			)
		});

	var outputQuery = session.Query<OrderItem>()
		.Where(x => 
			x.Order.ShippedOn != null &&
			x.Order.ShippedBy != null
		)
		.GroupBy(x => new
		{
			BranchId = x.Order.Branch.Id,
			ProductId = x.Product.Id,
			Date = x.Order.ShippedOn.Value.Date,
		})
		.Select(x => new
		{
			Id = x.Key.BranchId.ToString() + "_" + 
				x.Key.ProductId.ToString() + "_" + 
				x.Key.Date.ToString(),
			BranchId = x.Key.BranchId,
			ProductId = x.Key.ProductId,
			Date = x.Key.Date,
			Quantity = new Measure(
				x.Sum(o => o.QuantityStandardEquivalent.Value),
				x.Max(o => o.QuantityStandardEquivalent.Unit)
			)
		});

	var movementQuery = inputQuery.Join(outputQuery,
		(input) => input.Id,
		(output) => output.Id,
		(input, output) => new
		{
			BranchId = input.BranchId,// ?? output.BranchId,
			ProductId = input.ProductId,// ?? output.ProductId,
			Date = input.Date,
			InputQuantity = input.Quantity,
			OutputQuantity = output.Quantity
		}
	);
 */

/* HQL (not working) from clause doesn't accept sub queries
 * 
	select 
		coalesce(input.BranchId, output.BranchId) as BranchId,
		coalesce(input.ProductId, output.ProductId) as ProductId,
		coalesce(input.Date, output.Date) as Date,
		input.UnitId as InputUnitId,
		input.Value as InputValue,
		output.UnitId as OutputUnitId,
		output.Value as OutputUnitId

	from
		(
			select
				por.PurchaseOrder.Branch.Id as BranchId,
				por.Product.Id as ProductId,
				cast(por.ReceivedOn AS date) as Date,
				por.QuantityStandardEquivalent.Unit.Id as UnitId,
				sum(por.QuantityStandardEquivalent.Value) as Value

			from
				AmpedBiz.Core.Entities.PurchaseOrderReceipt as por

			group by
				por.PurchaseOrder.Branch,
				por.Product,
				cast(por.ReceivedOn as date),
				por.QuantityStandardEquivalent.Unit.Id

		) as input,

		(
			select
				oi.Order.Branch.Id as BranchId,
				oi.Product.Id as ProductId,
				cast(O.ShippedOn as date) as Date,
				oi.QuantityStandardEquivalent.Unit.Id as UnitId,
				sum(oi.QuantityStandardEquivalent.Value) as Value

			from
				AmpedBiz.Core.Entities.OrderItem as oi

			where
				oi.Order.ShippedOn is not null

			group by
				oi.Order.Branch.Id,
				oi.Product.Id,
				cast(oi.Order.ShippedOn as date),
				oi.QuantityStandardEquivalent.Unit.Id

		) as output

	where
		input.Branch.Id = output.Branch.Id 
		and input.Product.Id = output.Product.Id 
		and input.Date = output.Date
*/

/* T-SQL
 * 
	SELECT 
		BranchId = COALESCE(INPUT.BranchId, OUTPUT.BranchId),
		ProductId = COALESCE(INPUT.ProductId, OUTPUT.ProductId),
		Date = COALESCE(INPUT.Date, OUTPUT.Date),
		Input_UnitId = INPUT.UnitId,
		Input_Value = INPUT.Value,
		Output_UnitId = OUTPUT.UnitId,
		Output_Value = OUTPUT.Value

	FROM
		(
			SELECT
				BranchId = PO.BranchId,
				ProductId = POR.ProductId,
				Date = CAST(POR.ReceivedOn AS DATE),
				UnitId = POR.QuantityStandardEquivalent_UnitId,
				Value = SUM(POR.QuantityStandardEquivalent_Value)

			FROM
				PurchaseOrderReceipts AS POR

			INNER JOIN
				PurchaseOrders AS PO
					ON PO.PurchaseOrderId = POR.PurchaseOrderId

			GROUP BY
				PO.BranchId,
				POR.ProductId,
				CAST(POR.ReceivedOn AS DATE),
				POR.QuantityStandardEquivalent_UnitId
		) AS INPUT

	FULL JOIN
		(
			SELECT
				BranchId = O.BranchId,
				ProductId = OI.ProductId,
				Date = CAST(O.ShippedOn AS DATE),
				UnitId = OI.QuantityStandardEquivalent_UnitId,
				Value = SUM(OI.QuantityStandardEquivalent_Value)

			FROM
				OrderItems AS OI

			INNER JOIN
				Orders AS O
					ON O.OrderId = OI.OrderId

			WHERE
				O.ShippedOn IS NOT NULL

			GROUP BY
				O.BranchId,
				OI.ProductId,
				CAST(O.ShippedOn AS DATE),
				OI.QuantityStandardEquivalent_UnitId
		) AS OUTPUT

		ON INPUT.BranchId = OUTPUT.BranchId 
		   AND INPUT.ProductId = OUTPUT.ProductId 
		   AND INPUT.Date = OUTPUT.Date
*/
