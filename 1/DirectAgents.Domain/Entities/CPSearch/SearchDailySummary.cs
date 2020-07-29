namespace DirectAgents.Domain.Entities.CPSearch
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SearchDailySummary")]
    public partial class SearchDailySummary : DailySummaryBase
    {

        [Column(TypeName = "money")]
        public decimal Revenue { get; set; }

        [Column(TypeName = "money")]
        public decimal Cost { get; set; }

        public int Orders { get; set; }

        public int Clicks { get; set; }

        public int Impressions { get; set; }

        public int CurrencyId { get; set; }

        public int ViewThrus { get; set; }

        public int CassConvs { get; set; }

        public double CassConVal { get; set; }

        public virtual SearchCampaign SearchCampaign { get; set; }
    }
}
