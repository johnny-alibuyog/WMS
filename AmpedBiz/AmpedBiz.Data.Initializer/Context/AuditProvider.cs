using AmpedBiz.Data.Context;

namespace AmpedBiz.Data.Initializer.Context
{
    public class AuditProvider : IAuditProvider
    {
        private readonly IContext _context;

        public AuditProvider(IContext context)
        {
            this._context = context;
        }

        public object GetCurrentUserId()
        {
            return this._context.UserId;
        }
    }
}