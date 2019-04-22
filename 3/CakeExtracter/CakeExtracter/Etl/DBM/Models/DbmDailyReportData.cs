using System;
using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;

namespace CakeExtracter.Etl.DBM.Models
{
    /// <summary>
    /// Summary entity for account daily data
    /// </summary>
    public class DbmDailyReportData
    {
        public DateTime Date { get; set; }

        public List<DbmAdvertiser> Advertisers { get; set; }

        public List<DbmCampaign> Campaigns { get; set; }

        public List<DbmInsertionOrder> InsertionOrders { get; set; }

        public List<DbmLineItem> LineItems { get; set; }

        public List<DbmCreative> Creatives { get; set; }
    }
}
