using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Formss.AB.Views
{
    interface IABView
    {
        List<Data.Advertiser> Advertisers { set; }
        List<Data.ABItem> ABItems { set; }
        bool ConvertToTargetCurrency { get; }
        decimal Total { set; }
    }
}
