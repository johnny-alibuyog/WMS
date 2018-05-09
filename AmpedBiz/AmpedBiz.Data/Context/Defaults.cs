using AmpedBiz.Core.Entities;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Data.Context
{
    //public class Defaults
    //{
    //    private readonly IContext _context;

    //    private readonly ISession _session;

    //    public Lazy<User> User => new Lazy<User>(() => _session.Get<User>(_context.UserId));

    //    public Lazy<Branch> Branch => new Lazy<Branch>(() => _session.Get<Branch>(_context.BranchId));

    //    public Lazy<Tenant> Tenant => new Lazy<Tenant>(() => _session.Get<Tenant>(_context.TenantId));

    //    public Lazy<Currency> Currency => new Lazy<Currency>(() =>
    //    {
    //        var 
    //    });

    //    public Defaults(IContextProvider contextProvider, ISessionFactory sessionFactory)
    //    {
    //        this._contextProvider = contextProvider;
    //        this._sessionFactory = sessionFactory;
    //    }
    //}
}
