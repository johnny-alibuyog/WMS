using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using LinqToExcel;
using NHibernate;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _016_InventoryAdjustmentReasonSeeder : IDefaultDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _016_InventoryAdjustmentReasonSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            this._contextProvider = contextProvider;
            this._sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => true;

        public void Seed()
        {
            //throw new System.NotImplementedException();
        }

        //public class DataMapper
        //{
        //    public static InventoryAdjustmentReason Map(Row row)
        //    {
        //        return new InventoryAdjustmentReason(
        //            id: row[nameof(Tenant.Id)],
        //            name: row[nameof(Tenant.Name)],
        //            description: row[nameof(Tenant.Description)]
        //        );
        //    }
        //}
    }
}
