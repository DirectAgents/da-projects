using ClientPortal.Data.DTOs;
using System.Globalization;
namespace CakeExtracter.Reports
{
    public partial class SearchReportRuntimeTextTemplate : SearchReportRuntimeTextTemplateBase
    {
        public string AdvertiserName { get; set; }
        public bool ShowCalls { get; set; }
        
        public SearchStat Line1stat { get; set; }
        public SearchStat Line2stat { get; set; }
        public SimpleSearchStat ChangeStat { get; set; }

        public string AcctMgrName { get; set; }
        public string AcctMgrEmail { get; set; }

        public string Currency(decimal val)
        {
            return val.ToString("C");
        }

        private NumberFormatInfo _noParensFormatInfo;
        public NumberFormatInfo NoParensFormatInfo
        {
            get
            {
                if (_noParensFormatInfo == null)
                {
                    _noParensFormatInfo = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
                    _noParensFormatInfo.CurrencyNegativePattern = 1;
                }
                return _noParensFormatInfo;
            }
        }

        private string NoChangeSymbol = "~";

        public string CurrencyPlusMinus(decimal val)
        {
            if (val > 0)
                return "+" + val.ToString("C");
            else if (val < 0)
                return val.ToString("C", NoParensFormatInfo);
            else
                return NoChangeSymbol;
        }

        public string IntegerPlusMinus(int val)
        {
            if (val > 0)
                return "+" + val.ToString();
            else if (val < 0)
                return val.ToString();
            else
                return NoChangeSymbol;
        }
    }
}
