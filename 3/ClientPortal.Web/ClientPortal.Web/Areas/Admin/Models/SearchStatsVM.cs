using System;
using System.Collections.Generic;
using ClientPortal.Data.Contexts;

namespace ClientPortal.Web.Areas.Admin.Models
{
    public class SearchStatsVM
    {
        public SearchProfile SearchProfile { get; set; }

        public IEnumerable<ColumnConfig> ColumnConfigs { get; set; }
    }

    //TODO? Put in common project - combine with DAWeb.Areas.TD.Models...
    public class ColumnConfig
    {
        public const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string Format_Date = "dd/mm/yyyy";
        public const string Format_Integer = "#,0";
        public const string Format_DollarCents = "$#,0.00";
        public const string Format_Percent2Dec = "#,0.00%";
        public const string Format_Max2Dec = "#,0.##";

        public string PropName { get; set; }
        public string DisplayName { get; set; }
        public string Letter { get; set; }
        public string Format { get; set; }
        public string KendoType { get; set; }

        public ColumnConfig(string propName, string displayName, char letter, string format, string kendoType = "number")
        {
            this.PropName = propName;
            this.DisplayName = displayName;
            this.Letter = new String(letter, 1);
            this.Format = format;
            this.KendoType = kendoType;
        }
    }
}