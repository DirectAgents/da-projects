using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters;
using DirectAgents.Domain.Entities.CPProg;
using Yahoo;

namespace CakeExtracter.Etl.YAM.Extractors.ConValExtractors
{
    public abstract class BaseYamConValExtractor<T> : Extracter<T>
        where T: DatedStatsSummaryWithRev, new()
    {
        public const string ConValPattern = @"gv=(\d*\.?\d*)";

        protected readonly YAMUtility _yamUtility;
        protected readonly DateRange dateRange;
        protected readonly int yamAdvertiserId;
        protected readonly int accountId;

        protected BaseYamConValExtractor(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
        {
            this._yamUtility = yamUtility;
            this.dateRange = dateRange;
            this.accountId = account.Id;
            this.yamAdvertiserId = int.Parse(account.ExternalId);
        }

        protected void InitSummary(T summary, DateTime date, IEnumerable<YAMRow> rows)
        {
            var clickThrus = rows.Where(x => x.ClickThruConvs > 0);
            var viewThrus = rows.Where(x => x.ViewThruConvs > 0);
            summary.Date = date;
            summary.PostClickRev = clickThrus.Sum(x => GetConVal(x.PixelParameter));
            summary.PostViewRev = viewThrus.Sum(x => GetConVal(x.PixelParameter));
        }

        protected decimal GetConVal(string pixelParameter)
        {
            if (pixelParameter == null)
            {
                return 0;
            }
            var match = Regex.Match(pixelParameter, ConValPattern);
            if (!match.Success)
            {
                return 0;
            }
            decimal conval;
            return decimal.TryParse(match.Groups[1].Value, out conval) ? conval : 0;
        }
    }
}