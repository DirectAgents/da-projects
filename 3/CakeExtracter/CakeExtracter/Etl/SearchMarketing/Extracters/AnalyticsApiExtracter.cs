using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.SearchMarketing.Extracters
{
    public class AnalyticsApiExtracter : Extracter<Dictionary<string, string>>
    {
        private readonly string clientCustomerId;
        private readonly DateTime beginDate;
        private readonly DateTime endDate;

        public AnalyticsApiExtracter(string clientCustomerId, CakeExtracter.Common.DateRange dateRange)
        {
            this.clientCustomerId = clientCustomerId;
            this.beginDate = dateRange.FromDate;
            this.endDate = dateRange.ToDate;
        }

        protected override void Extract()
        {
            throw new NotImplementedException();
        }
    }
}
