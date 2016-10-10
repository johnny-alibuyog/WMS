namespace AmpedBiz.Core
{
    public interface IAuditProvider
    {
        object GetCurrentUserId();

        //Audit CreateNew();
        //Audit CreateUpdate();
        //Audit CurrentAudit { get; set; }
        //PropertyInfo PropertyInfo { get; set; }

        //public virtual Audit CreateNew()
        //{
        //    var createdOn = DateTime.Now;
        //    var createdBy = WindowsIdentity.GetCurrent().Name;
        //    return Audit.Create(createdBy, createdOn);
        //}

        //public virtual Audit CreateUpdate()
        //{
        //    var updatedOn = DateTime.Now;
        //    var updatedBy = WindowsIdentity.GetCurrent().Name;
        //    return Audit.Create(updatedBy, updatedOn, this.CurrentAudit);
        //}
    }

}
