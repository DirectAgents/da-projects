using System;
using System.Collections.Generic;
using DirectAgents.Domain.Entities.Cake;

namespace CakeExtracter.CakeMarketingApi.Entities
{
    public class SubIdSummaryResponse
    {
        public bool Success { get; set; }
        public int RowCount { get; set; }
        public List<SubIdSummary> SubIds { get; set; }
    }

    public class SubIdSummary
    {
        //SubId
        public string SubIdName { get; set; }
        public int Views { get; set; }
        public int Clicks { get; set; }
        public decimal MacroEventConversions { get; set; }
        public decimal PaidMacroEventConversions { get; set; }
        public decimal SellableMacroEventConversions { get; set; }
        //MicroEvents
        public decimal Cost { get; set; }
        public decimal Revenue { get; set; }
        //Pending,Rejected,Approved,Returned
        //Orders,OrderTotal,AverageOrderValue
        //AverageMacroEventConversionScore,PaidMicroEvents
        public decimal TotalPaid { get; set; }

        //Not returned from the API. (Set in the extracter)
        public DateTime Date { get; set; }
        public int affiliateId { get; set; }
        public int offerId { get; set; }

        public bool AllZeros()
        {
            return (Views == 0 && Clicks == 0 && MacroEventConversions == 0 && PaidMacroEventConversions == 0 && SellableMacroEventConversions == 0
                    && Cost == 0 && Revenue == 0 && TotalPaid == 0);
        }

        // copy everything expect the primary key
        public void CopyValuesTo(AffSubSummary affSubSum)
        {
            affSubSum.Views = this.Views;
            affSubSum.Clicks = this.Clicks;
            affSubSum.Conversions = this.MacroEventConversions;
            affSubSum.Paid = this.PaidMacroEventConversions;
            affSubSum.Sellable = this.SellableMacroEventConversions;
            affSubSum.Revenue = this.Revenue;
            affSubSum.Cost = this.Cost;
        }
    }
}
