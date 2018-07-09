using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using AmpedBiz.Data.Seeders.DataProviders;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _002_DefaultBranchSeeder : IDefaultDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _002_DefaultBranchSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            this._contextProvider = contextProvider;
            this._sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => false;

        public void Seed()
        {
            var context = this._contextProvider.Build();

            using (var session = _sessionFactory.RetrieveSharedSession(context))
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(10);

                var saveBranch = new Action<Branch>((branch) =>
                {
                    branch.EnsureValidity();
                    session.Save(branch, branch.Id); //session.Save(branch);
                });

                var seed = new[] { Branch.Default }; //BranchData.Get(context, session);

                var existingBranches = session.Query<Branch>().Cacheable().ToList();

                var branchesToInsert = seed.Where(x => !existingBranches.Any(o => o.Name == x.Name));

                branchesToInsert.ForEach(saveBranch);

                transaction.Commit();

                _sessionFactory.ReleaseSharedSession();
            }
        }
    }

    internal class BranchData : DataProvider<Branch>
    {
        public static IEnumerable<Branch> Get(IContext context, ISession session) => new BranchData(context, session).Get();

        public BranchData(IContext context, ISession session) : base(@"branches.xlsx", context, session) { }

        public override Branch Map(Row row)
        {
            return new Branch()
            {
                Name = row[nameof(Branch.Name)],
                Description = row[nameof(Branch.Description)],
                TaxpayerIdentificationNumber = row[nameof(Branch.TaxpayerIdentificationNumber)],
                Contact = new Contact()
                {
                    Email = row[nameof(Contact.Email)],
                    Landline = row[nameof(Contact.Landline)],
                    Fax = row[nameof(Contact.Fax)],
                    Mobile = row[nameof(Contact.Mobile)],
                    Web = row[nameof(Contact.Web)]
                },
                Address = new Address()
                {
                    Street = row[nameof(Address.Street)],
                    Barangay = row[nameof(Address.Barangay)],
                    City = row[nameof(Address.City)],
                    Province = row[nameof(Address.Province)],
                    Region = row[nameof(Address.Region)],
                    Country = row[nameof(Address.Country)],
                    ZipCode = row[nameof(Address.ZipCode)],
                }
            };
        }
    }
}
