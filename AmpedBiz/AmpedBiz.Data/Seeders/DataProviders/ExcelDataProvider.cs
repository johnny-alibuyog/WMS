using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using LinqToExcel;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DataProviders
{
    public class ExcelDataProvider
    {
        private readonly IContext _context;
        private readonly ISession _session;
        private readonly DatabaseConfig.SeederConfig _config;

        public ExcelDataProvider(IContext context, ISession session)
        {
            this._context = context;
            this._session = session;
            this._config = DatabaseConfig.Instance.Seeder;
        }

        private string ResolveFilePath(string filename)
        {
            var branchName = this._session.Get<Branch>(this._context.BranchId)?.Name ?? string.Empty;

            var path = new
            {
                Branch = Path.Combine(this._config.ExternalFilesAbsolutePath, this._context.TenantId, "Branches", branchName, filename),
                Tenant = Path.Combine(this._config.ExternalFilesAbsolutePath, this._context.TenantId, filename),
                Common = Path.Combine(this._config.ExternalFilesAbsolutePath, "Common", filename)
            };

            if (File.Exists(path.Branch))
                return path.Tenant;

            if (File.Exists(path.Tenant))
                return path.Tenant;

            if (File.Exists(path.Common))
                return path.Common;

            throw new FileNotFoundException($"Files {path.Branch}, {path.Tenant} or {path.Common} does not exists");
        }

        public IReadOnlyCollection<T> Import<T>(string filename, Func<Row, T> mapper)
        {
            var filepath = this.ResolveFilePath(filename);

            Console.WriteLine("======================================================");
            Console.WriteLine(filepath);
            Console.WriteLine("======================================================");

            var excel = new ExcelQueryFactory(filepath);

            return excel.Worksheet().Select(mapper).ToList().AsReadOnly();
        }
    }
}
