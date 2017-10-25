﻿using AmpedBiz.Core.Entities;
using System;

namespace AmpedBiz.Data.Context
{
    public class DefaultContext : IContext
    {
        public Guid UserId => User.Admin.Id;

        public Guid BranchId => Branch.Default.Id;

        public string TenantId => Tenant.Default.Id;

        public static readonly DefaultContext Instance = new DefaultContext();
    }
}