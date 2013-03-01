using System;

namespace ClientPortal.Data.DTOs
{
    public class MonthlyInfo
    {
        public string Id { get { return OfferId + "_" + Period.ToString("yyyyMM"); } }

        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime Period
        {
            get { return new DateTime(this.Year, this.Month, 1); }
        }
        public int AdvertiserId { get; set; }
        //public string AdvertiserName { get; set; }
        public int OfferId { get; set; }
        public string Offer { get; set; }
        //public string UnitType { get; set; }
        //public int TotalUnits { get; set; }
        public int CampaignStatusId { get; set; }
        public int AccountingStatusId { get; set; }

        public decimal Revenue { get; set; }
        public string Currency
        {
            set { Culture = OfferInfo.CurrencyToCulture(value); }
        }
        public string Culture { get; set; }
    }

    public partial class CampaignStatus
    {
        public static int Default = 1;
        public static int Finalized = 2;
        public static int Active = 3;
        public static int Verified = 4;
    }
}
