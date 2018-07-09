using AmpedBiz.Data.Context;
using LinqToExcel;
using NHibernate;
using System.Collections.Generic;

namespace AmpedBiz.Data.Seeders.DataProviders
{
    internal abstract class DataProvider<T>
    {
        private readonly string _filename;

        private readonly ExcelDataImporter _importer;

        public abstract T Map(Row row);

        public DataProvider(string filename, IContext context, ISession session)
        {
            this._filename = filename;
            this._importer = new ExcelDataImporter(context, session);
        }

        public IEnumerable<T> Get() => this._importer.Import(_filename, this.Map);
    }
}
