using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Domain.Entities;

namespace AmpedBiz.Data.Configurations
{
    public interface IAuditProvider
    {
        Audit CreateNew();
        Audit CreateUpdate();
        Audit CurrentAudit { get; set; }
        PropertyInfo PropertyInfo { get; set; }

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
