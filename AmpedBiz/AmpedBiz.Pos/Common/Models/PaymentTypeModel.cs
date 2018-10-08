using System;

namespace AmpedBiz.Pos.Common.Models
{
	public class PaymentTypeModel : LookupModel<string>
	{
		public PaymentTypeModel(string id, string name) : base(id, name) { }
	}
}
