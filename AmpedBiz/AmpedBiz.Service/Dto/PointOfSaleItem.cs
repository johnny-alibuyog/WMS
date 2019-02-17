using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
	public class PointOfSaleItem
	{
		public Guid Id { get; set; }

		public Guid PointOfSaleId { get; set; }

		public Lookup<Guid> Product { get; set; }

		public Measure Quantity { get; set; }

		public Measure Standard { get; set; }

		public decimal DiscountRate { get; set; }

		public decimal DiscountAmount { get; set; }

		public decimal UnitPriceAmount { get; set; }

		public decimal ExtendedPriceAmount { get; set; }

		public decimal TotalPriceAmount { get; set; }
	}
}
