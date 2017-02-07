using System;
using System.Collections.Generic;
using System.IO;
using Westwind.Utilities.Configuration;

namespace AmpedBiz.Common.Configurations
{
    public class StateConfig : AppConfiguration
    {
        public static Lazy<StateConfig> Instance = new Lazy<StateConfig>(() =>
        {
            var config = new StateConfig();
            config.Initialize();
            return config;
        });

        public Dictionary<string, Definition> OrderConfig = new Dictionary<string, Definition>()
        {
            {
                Order.Status.New, new Definition()
                {
                    AllowedModifications = new string[]
                    {
                        Order.Aggregates.Items,
                        Order.Aggregates.Payments,
                    },
                    AllowedTransitions = new string[]
                    {
                        Order.Status.Invoiced,
                        Order.Status.Cancelled,
                    }
                }
            },
            {
                Order.Status.Invoiced,  new Definition()
                {
                    AllowedModifications = new string[]
                    {
                        Order.Aggregates.Payments,
                    },
                    AllowedTransitions = new string[]
                    {
                        Order.Status.Staged,
                        Order.Status.Cancelled,
                    }
                }
            },
            {
                Order.Status.Staged,  new Definition()
                {
                    AllowedModifications = new string[]
                    {
                        Order.Aggregates.Payments,
                    },
                    AllowedTransitions = new string[]
                    {
                        Order.Status.Shipped,
                        Order.Status.Cancelled,
                    }
                }
            },
            {
                Order.Status.Shipped,  new Definition()
                {
                    AllowedModifications = new string[]
                    {
                        Order.Aggregates.Payments,
                        Order.Aggregates.Returns,
                    },
                    AllowedTransitions = new string[]
                    {
                        Order.Status.Completed,
                        Order.Status.Cancelled,
                    }
                }
            },
            {
                Order.Status.Completed,  new Definition()
                {
                    AllowedModifications = new string[]
                    {

                    },
                    AllowedTransitions = new string[]
                    {

                    }
                }
            },
            {
                Order.Status.Cancelled,  new Definition()
                {
                    AllowedModifications = new string[]
                    {

                    },
                    AllowedTransitions = new string[]
                    {

                    }
                }
            },
        };

        public Dictionary<string, Definition> PurchaseOrderConfig = new Dictionary<string, Definition>()
        {
            {
                PurchaseOrder.Status.New, new Definition()
                {
                    AllowedModifications = new string[]
                    {
                        PurchaseOrder.Aggregates.Items,
                        PurchaseOrder.Aggregates.Payments,
                    },
                    AllowedTransitions = new string[]
                    {
                        PurchaseOrder.Status.New,
                        PurchaseOrder.Status.Submitted,
                        PurchaseOrder.Status.Cancelled,
                    }
                }
            },
            {
                PurchaseOrder.Status.Submitted, new Definition()
                {
                    AllowedModifications = new string[]
                    {
                        PurchaseOrder.Aggregates.Payments,
                    },
                    AllowedTransitions = new string[]
                    {
                        PurchaseOrder.Status.New,
                        PurchaseOrder.Status.Approved,
                        PurchaseOrder.Status.Cancelled,
                    }
                }
            },
            {
                PurchaseOrder.Status.Approved, new Definition()
                {
                    AllowedModifications = new string[]
                    {
                        PurchaseOrder.Aggregates.Payments,
                        PurchaseOrder.Aggregates.Receipts,
                    },
                    AllowedTransitions = new string[]
                    {
                        PurchaseOrder.Status.Paid,
                        PurchaseOrder.Status.Cancelled,
                    }
                }
            },
            {
                PurchaseOrder.Status.Paid, new Definition()
                {
                    AllowedModifications = new string[]
                    {
                        PurchaseOrder.Aggregates.Payments,
                        PurchaseOrder.Aggregates.Receipts,
                    },
                    AllowedTransitions = new string[]
                    {
                        PurchaseOrder.Status.Paid,
                        PurchaseOrder.Status.Received,
                        PurchaseOrder.Status.Completed,
                        PurchaseOrder.Status.Cancelled,
                    }
                }
            },
            {
                PurchaseOrder.Status.Received, new Definition()
                {
                    AllowedModifications = new string[]
                    {
                        PurchaseOrder.Aggregates.Payments,
                        PurchaseOrder.Aggregates.Receipts,
                    },
                    AllowedTransitions = new string[]
                    {
                        PurchaseOrder.Status.Paid,
                        PurchaseOrder.Status.Received,
                        PurchaseOrder.Status.Completed,
                        PurchaseOrder.Status.Cancelled,
                    }
                }
            },
            {
                PurchaseOrder.Status.Completed, new Definition()
                {
                    AllowedModifications = new string[]
                    {

                    },
                    AllowedTransitions = new string[]
                    {

                    }
                }
            },
            {
                PurchaseOrder.Status.Cancelled, new Definition()
                {
                    AllowedModifications = new string[]
                    {

                    },
                    AllowedTransitions = new string[]
                    {

                    }
                }
            },
        };

        protected override IConfigurationProvider OnCreateDefaultProvider(string sectionName, object configData)
        {
            this.Provider = new JsonFileConfigurationProvider<StateConfig>()
            {
                JsonConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "state.config.json"),
            };

            return this.Provider;
        }
    }

    public class Definition
    {
        public string[] AllowedTransitions { get; set; }

        public string[] AllowedModifications { get; set; }
    }

    internal static class Order
    {
        public static class Status
        {
            public static string New = "New";

            public static string Invoiced = "Invoiced";

            public static string Staged = "Staged";

            public static string Routed = "Routed";

            public static string Shipped = "Shipped";

            public static string Completed = "Completed";

            public static string Cancelled = "Cancelled";
        }

        public static class Aggregates
        {
            public static string Items = "Items";

            public static string Payments = "Payments";

            public static string Returns = "Returns";
        }
    }

    internal static class PurchaseOrder
    {
        public static class Status
        {
            public static string New = "New";

            public static string Submitted = "Submitted";

            public static string Approved = "Approved";

            public static string Paid = "Paid";

            public static string Received = "Received";

            public static string Completed = "Completed";

            public static string Cancelled = "Cancelled";
        }

        public static class Aggregates
        {
            public static string Items = "Items";

            public static string Payments = "Payments";

            public static string Receipts = "Receipts";
        }
    }
}
