﻿using AmpedBiz.Data;

namespace AmpedBiz.Service.Host.Plugins.Providers
{
    public class AuditProvider : IAuditProvider
    {
        public object GetCurrentUserId()
        {
            return "admin1";
        }
    }
}