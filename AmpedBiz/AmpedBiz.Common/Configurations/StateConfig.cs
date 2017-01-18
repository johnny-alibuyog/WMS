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
                    Modifications = new string[]
                    {
                        Order.Aggregates.Items,
                        Order.Aggregates.Payments,
                    },
                    Transitions = new string[]
                    {
                        Order.Status.New,
                        Order.Status.Invoiced,
                        Order.Status.Cancelled,
                    }
                }
            },
            {
                Order.Status.Invoiced,  new Definition()
                {
                    Modifications = new string[]
                    {
                        Order.Aggregates.Payments,
                    },
                    Transitions = new string[]
                    {
                        Order.Status.Paid,
                        Order.Status.Cancelled,
                    }
                }
            },
            {
                Order.Status.Paid, new Definition()
                {
                    Modifications = new string[]
                    {
                        Order.Aggregates.Items,
                    },
                    Transitions =  new string[]
                    {
                        Order.Status.Paid,
                        Order.Status.Staged,
                        Order.Status.Shipped,
                        Order.Status.Cancelled,
                    }
                }
            },
            {
                Order.Status.Staged,  new Definition()
                {
                    Modifications = new string[]
                    {
                        Order.Aggregates.Payments,
                    },
                    Transitions = new string[]
                    {
                        Order.Status.Paid,
                        Order.Status.Shipped,
                        Order.Status.Cancelled,
                    }
                }
            },
            {
                Order.Status.Shipped,  new Definition()
                {
                    Modifications = new string[]
                    {
                        Order.Aggregates.Payments,
                        Order.Aggregates.Returns,
                    },
                    Transitions = new string[]
                    {
                        Order.Status.Paid,
                        Order.Status.Returned,
                        Order.Status.Completed,
                        Order.Status.Cancelled,
                    }
                }
            },
            {
                Order.Status.Returned,  new Definition()
                {
                    Modifications = new string[]
                    {
                        Order.Aggregates.Payments,
                        Order.Aggregates.Returns,
                    },
                    Transitions = new string[]
                    {
                        Order.Status.Paid,
                        Order.Status.Returned,
                        Order.Status.Completed,
                        Order.Status.Cancelled,
                    }
                }
            },
            {
                Order.Status.Completed,  new Definition()
                {
                    Modifications = new string[]
                    {

                    },
                    Transitions = new string[]
                    {

                    }
                }
            },
            {
                Order.Status.Cancelled,  new Definition()
                {
                    Modifications = new string[]
                    {

                    },
                    Transitions = new string[]
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
                    Modifications = new string[]
                    {
                        PurchaseOrder.Aggregates.Items,
                        PurchaseOrder.Aggregates.Payments,
                    },
                    Transitions = new string[]
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
                    Modifications = new string[]
                    {
                        PurchaseOrder.Aggregates.Payments,
                    },
                    Transitions = new string[]
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
                    Modifications = new string[]
                    {
                        PurchaseOrder.Aggregates.Payments,
                        PurchaseOrder.Aggregates.Receipts,
                    },
                    Transitions = new string[]
                    {
                        PurchaseOrder.Status.Paid,
                        PurchaseOrder.Status.Cancelled,
                    }
                }
            },
            {
                PurchaseOrder.Status.Paid, new Definition()
                {
                    Modifications = new string[]
                    {
                        PurchaseOrder.Aggregates.Payments,
                        PurchaseOrder.Aggregates.Receipts,
                    },
                    Transitions = new string[]
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
                    Modifications = new string[]
                    {
                        PurchaseOrder.Aggregates.Payments,
                        PurchaseOrder.Aggregates.Receipts,
                    },
                    Transitions = new string[]
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
                    Modifications = new string[]
                    {

                    },
                    Transitions = new string[]
                    {

                    }
                }
            },
            {
                PurchaseOrder.Status.Cancelled, new Definition()
                {
                    Modifications = new string[]
                    {

                    },
                    Transitions = new string[]
                    {

                    }
                }
            },
        };

        protected override IConfigurationProvider OnCreateDefaultProvider(string sectionName, object configData)
        {
            this.Provider = new JsonFileConfigurationProvider<TransitionConfig>()
            {
                JsonConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "state.config.json"),
            };

            return this.Provider;
        }
    }

    public class Definition
    {
        public string[] Transitions { get; set; }

        public string[] Modifications { get; set; }
    }

    internal static class Order
    {
        public static class Status
        {
            public static string New = "New";

            public static string Invoiced = "Invoiced";

            public static string Paid = "Paid";

            public static string Staged = "Staged";

            public static string Routed = "Routed";

            public static string Shipped = "Shipped";

            public static string Returned = "Returned";

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
