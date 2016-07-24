using System.Collections.ObjectModel;
using Microsoft.Reporting.WebForms;

namespace AmpedBiz.Reports.App_Code
{
    public class Reports
    {
        public class ReportKey
        {
            public const string Name = "6C1EFECE-4A98-4CDE-BE4D-788794E1320E";
            public const string Parameter = "11C54A42-E909-4A02-9F19-580F8CE61CCB";
            public const string Path = "B4F81FC6-958E-41D0-A4B2-7372EB66FCC3";
            public const string Selector = "DFD18FD7-9D34-4692-B7F9-7E54BFB4B0EB";
            public const string DataSource = "AD6C8DCF-BB0B-4923-A53D-725AEA8871CE";
        }

        public class ReportParameter
        {
            public string Key { get; set; }
            public string Value { get; set; }

            public Instruction Instruction { get; set; }
        }

        public class ReportParameterCollection : Collection<ReportParameter>
        {
        }

        public enum Instruction
        {
            Equal,
            Contains,
            Begins,
            GreaterThan,
            LessThan,
            Between,
            NoEqual
        }
    }
}