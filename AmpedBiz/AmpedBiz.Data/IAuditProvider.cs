namespace AmpedBiz.Data
{
    public interface IAuditProvider
    {
        object GetCurrentUserId();
    }
}
