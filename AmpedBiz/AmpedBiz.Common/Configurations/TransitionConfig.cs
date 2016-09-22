using System;
using System.Collections.Generic;
using System.IO;
using Westwind.Utilities.Configuration;

namespace AmpedBiz.Common.Configurations
{
    public class TransitionConfig : AppConfiguration
    {
        public static TransitionConfig Instance = CreateInstance();

        private static TransitionConfig CreateInstance()
        {
            var config = new TransitionConfig();
            config.Initialize();
            return config;
        }

        public Dictionary<string, Dictionary<string, string>> OrderTransitions = new Dictionary<string, Dictionary<string, string>>()
        {
            {
                "New", new Dictionary<string, string>()
                {
                    { "New", "Save" },
                    { "Invoiced", "Invoice" },
                    { "Cancelled", "Cancel" },
                }
            },
            {
                "Invoiced", new Dictionary<string, string>()
                {
                    { "Paid", "Pay" },
                    { "Cancelled", "Cancel" },
                }
            },
            {
                "Paid", new Dictionary<string, string>()
                {
                    { "Paid", "Pay" },
                    { "Staged", "Stage" },
                    { "Shipped", "Ship" },
                    { "Cancelled", "Cancel" },
                }
            },
            {
                "Staged", new Dictionary<string, string>()
                {
                    { "Paid", "Pay" },
                    { "Shipped", "Ship" },
                    { "Cancelled", "Cancel" },
                }
            },
            {
                "Shipped", new Dictionary<string, string>()
                {
                    { "Paid", "Pay" },
                    { "Returned", "Save" },
                    { "Completed", "Complete" },
                    { "Cancelled", "Cancel" },
                }
            },
            {
                "Returned", new Dictionary<string, string>()
                {
                    { "Paid", "Pay" },
                    { "Returned", "Save" },
                    { "Completed", "Complete" },
                    { "Cancelled", "Cancel" },
                }
            },
            {
                "Completed", new Dictionary<string, string>() { }
            },
            {
                "Cancelled", new Dictionary<string, string>() { }
            },
        };

        public Dictionary<string, Dictionary<string, string>> PurchaseOrderTransitions = new Dictionary<string, Dictionary<string, string>>()
        {
            {
                "New", new Dictionary<string, string>()
                {
                    { "New", "Save" },
                    { "Submitted", "Submitt" },
                    { "Cancelled", "Cancel" },
                }
            },
            {
                "Submitted", new Dictionary<string, string>()
                {
                    { "New", "Reject" },
                    { "Approved", "Approve" },
                    { "Cancelled", "Cancel" },
                }
            },
            {
                "Approved", new Dictionary<string, string>()
                {
                    { "Paid", "Pay" },
                    { "Cancelled", "Cancel" },
                }
            },
            {
                "Paid", new Dictionary<string, string>()
                {
                    { "Paid", "Pay" },
                    { "Received", "Receive" },
                    { "Completed", "Complete" },
                    { "Cancelled", "Cancel" },
                }
            },
            {
                "Received", new Dictionary<string, string>()
                {
                    { "Paid", "Pay" },
                    { "Received", "Receive" },
                    { "Completed", "Complete" },
                    { "Cancelled", "Cancel" },
                }
            },
            {
                "Completed", new Dictionary<string, string>() { }
            },
            {
                "Cancelled", new Dictionary<string, string>() { }
            },
        };

        protected override IConfigurationProvider OnCreateDefaultProvider(string sectionName, object configData)
        {
            this.Provider = new JsonFileConfigurationProvider<TransitionConfig>()
            {
                JsonConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "transitions.config.json"),
            };

            return this.Provider;
        }
    }
}
